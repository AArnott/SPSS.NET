using System;

namespace Spss
{
	/// <summary>
	/// Represents a single case in an SPSS data file.
	/// Allows for reading/writing variable data on a per-case basis.
	/// </summary>
	/// <remarks>
	/// In SPSS a case is analogous to a <see cref="System.Data.DataRow"/> 
	/// in ADO.NET.
	/// </remarks>
	public class SpssCase
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssCase"/> class.
		/// </summary>
		protected internal SpssCase(SpssCasesCollection cases, int position)
		{
			this.cases = cases;
			this.position = position;
			// Clear out all variables explicitly to prevent SPSS from assuming
			// garbage characters in those data areas.
			if( cases.IsAppendOnly ) ClearData();
		}
		#endregion

		#region Attributes
		private readonly SpssCasesCollection cases;
		/// <summary>
		/// Gets the containing <see cref="SpssCasesCollection"/>.
		/// </summary>
		protected SpssCasesCollection Cases { get { return cases; } }
		/// <summary>
		/// Gets the variables in the document.
		/// </summary>
		protected SpssVariablesCollection Variables
		{
			get
			{
				return Cases.Document.Variables;
			}
		}
		private readonly int position;
		/// <summary>
		/// The index of the row within the data file.
		/// </summary>
		public int Position { get { return position; } }
		/// <summary>
		/// Gets/sets the value of some variable on this row.
		/// </summary>
		public object this [string varName]
		{
			get
			{
				EnsureActiveCase();
				
				return Variables[varName].Value;
			}
			set
			{
				EnsureActiveCase();
				
				Variables[varName].Value = value;
			}
		}
		#endregion

		#region Operations
		/// <summary>
		/// Ensures that the SPSS data file is currently pointing at this case's data.
		/// </summary>
		protected void EnsureActiveCase()
		{
			if( Position != Cases.Position )
			{
				if( Cases.IsAppendOnly )
					throw new InvalidOperationException("This case is no longer the one being appended.");

				Cases.Position = Position;
			}
		}
		/// <summary>
		/// Writes a newly added row to the data file.
		/// </summary>
		/// <remarks>
		/// This method should be called after a new row has had all its values set.
		/// After calling this method, variable values may not be changed.
		/// Without calling this method, the row is never actually written to the 
		/// data file.
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
		public void Commit() 
		{
			Cases.Document.EnsureNotClosed();
			if( Cases.IsReadOnly ) throw new InvalidOperationException("Not available when in read-only mode.");
			SpssSafeWrapper.spssCommitCaseRecord(Cases.FileHandle);
		}
		/// <summary>
		/// Clears every variable of data for this row.
		/// </summary>
		/// <remarks>
		/// Remarkably, this operation is necessary for every new row, in order to 
		/// prevent garbage from being placed into SPSS for the values that are never
		/// explicitly set otherwise.
		/// </remarks>
		public void ClearData()
		{
			foreach( SpssVariable var in Cases.Document.Variables )
				this[var.Name] = null;
		}
		public object GetDBValue(string varName)
		{
			return (this[varName] != null) ? this[varName] : DBNull.Value;
		}
		public void SetDBValue(string varName, object value)
		{
			this[varName] = (value != DBNull.Value) ? value : null;
		}
		#endregion
	}
}
