using System;
using System.Data;
using System.Collections;

namespace Spss
{
	/// <summary>
	/// Manages the cases in an SPSS data file.  
	/// Supports reading, writing, and appending data.
	/// </summary>
	public sealed class SpssCasesCollection : IEnumerable
	{
		private int caseCountInWriteMode;

		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssCasesCollection"/> class.
		/// </summary>
		/// <param name="document">
		/// The hosting document.
		/// </param>
		internal SpssCasesCollection(SpssDataDocument document)
		{
			if( document == null ) throw new ArgumentNullException("document");
			this.document = document;
		}
		#endregion

		#region Attributes
		/// <summary>
		/// Gets whether the set of cases is read only.  
		/// If no cases can be added, changed or removed, IsReadOnly is true.
		/// <seealso cref="IsAppendOnly"/>
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return Document.AccessMode == SpssFileAccess.Read;
			}
		}
		/// <summary>
		/// Gets whether the set of cases can only be appended to.
		/// If no cases can be removed or changed, IsAppendOnly is true.
		/// <seealso cref="IsReadOnly"/>
		/// </summary>
		public bool IsAppendOnly
		{
			get
			{
				return Document.AccessMode != SpssFileAccess.Read; // Create or Append
			}
		}
		private readonly SpssDataDocument document;
		/// <summary>
		/// The SPSS data document whose cases are being managed.
		/// </summary>
		public SpssDataDocument Document { get { return document; } }
		/// <summary>
		/// The file handle of the SPSS data document whose cases are being managed.
		/// </summary>
		internal Int32 FileHandle
		{
			get
			{
				return Document.Handle;
			}
		}

		/// <summary>
		/// The number of cases in the document.
		/// </summary>
		public int Count
		{
			get 
			{
				if (this.Document.AccessMode == SpssFileAccess.Create) {
					return this.caseCountInWriteMode;
				}

				Int32 casecount = 0;
				ReturnCode result = SpssSafeWrapper.spssGetNumberofCases(FileHandle, out casecount);
				if( result != ReturnCode.SPSS_OK )
					throw new SpssException(result, "spssGetNumberofCases");
				return casecount;
			}
		}
		private int position = -1;
		/// <summary>
		/// Gets/sets the case that SPSS is pointing at to get/set variable data on.
		/// </summary>
		internal int Position
		{
			get
			{
				return position;
			}
			set
			{
				if( IsAppendOnly ) throw new InvalidOperationException("Not available while in Append mode.");
				if( value < 0 ) 
					throw new ArgumentOutOfRangeException("Position", value, "Must be a non-negative integer.");
				if( value >= Count ) 
					throw new ArgumentOutOfRangeException("Position", value, "Must be less than the number of cases in the file.");
				if( value == position ) return; // nothing to do!
				ReturnCode result = SpssSafeWrapper.spssSeekNextCase(FileHandle, value);
				if( result != ReturnCode.SPSS_OK )
					throw new SpssException(result, "spssSeekNextCase");
				result = SpssSafeWrapper.spssReadCaseRecord(FileHandle);
				if( result != ReturnCode.SPSS_OK )
					throw new SpssException(result, "spssReadCaseRecord");
				position = value;
			}
		}
		/// <summary>
		/// Gets the case record at a specific 0-based index.
		/// </summary>
		public SpssCase this [int index]
		{
			get
			{
				return new SpssCase(this, index);
			}
		}
		#endregion

		#region Operations
		/// <summary>
		/// Creates a new SPSS case to assign values to.
		/// </summary>
		/// <returns>
		/// The <see cref="SpssCase"/> object used to access the new row.
		/// </returns>
		/// <remarks>
		/// This call must be followed (eventually) with a call to
		/// <see cref="SpssCase.Commit"/> or no new row will actually be added.
		/// </remarks>
		/// <example>
		/// To add a case to an existing SPSS file called mydata.sav, the following code could apply:
		/// <code>
		/// using( SpssDataDocument doc = SpssDataDocument.Open("mydata.sav", SpssFileAccess.Append) )
		/// {
		///		SpssCase Case = doc.Cases.New();
		///		Case["var1"] = 5;
		///		Case["var2"] = 3;
		///		Case["name"] = "Andrew";
		///		Case.Commit();
		///	}
		/// </code>
		/// </example>
		public SpssCase New()
		{
			if( !IsAppendOnly )
				throw new InvalidOperationException("Only available in append mode.");
			position = Count;
			return new SpssCase(this, position);
		}
		/// <summary>
		/// Creates a <see cref="DataTable"/> filled with all the data in the SPSS document.
		/// </summary>
		/// <returns>
		/// The created DataTable.
		/// </returns>
		public DataTable ToDataTable()
		{
			DataTable dt = new DataTable();
			foreach( SpssVariable var in document.Variables )
			{
				DataColumn dc = new DataColumn(var.Name);
				if( var is SpssStringVariable )
					dc.DataType = typeof(string);
				else if( var is SpssNumericVariable )
					dc.DataType = typeof(double);
				else if( var is SpssDateVariable )
					dc.DataType = typeof(DateTime);
				else
					throw new NotSupportedException("SPSS variable type " + var.GetType().Name + " is not supported.");
				dt.Columns.Add(dc);
			}

			foreach( SpssCase spssCase in this )
			{
				DataRow row = dt.NewRow();
				foreach( SpssVariable var in document.Variables )
					row[var.Name] = spssCase.GetDBValue(var.Name);
				dt.Rows.Add(row);
			}

			return dt;
		}
		#endregion

		#region IEnumerable Members
		/// <summary>
		/// Gets the enumerator that will iterate over all cases in the data file.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			if( IsAppendOnly ) throw new InvalidOperationException("Not available in append-only mode.");

			return new SpssCasesCollectionEnumerator(this);
		}

		class SpssCasesCollectionEnumerator : IEnumerator
		{
			internal SpssCasesCollectionEnumerator(SpssCasesCollection cases)
			{
				if( cases == null ) throw new ArgumentNullException("cases");
				this.Cases = cases;
				Reset();
			}

			SpssCasesCollection Cases;
			int position;

			public SpssCase Current
			{
				get
				{
					return Cases[position];
				}
			}

			#region IEnumerator Members

			public bool MoveNext()
			{
				return ++position < Cases.Count;
			}

			public void Reset()
			{
				position = -1;
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			#endregion
		}

		#endregion

		internal void OnCaseCommitted() {
			if (this.Document.AccessMode == SpssFileAccess.Create) {
				this.caseCountInWriteMode++;
			}
		}
	}
}
