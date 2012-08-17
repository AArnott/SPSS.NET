//-----------------------------------------------------------------------
// <copyright file="SpssCase.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Brigham Young University
// </copyright>
//-----------------------------------------------------------------------

namespace Spss {
	using System;

	/// <summary>
	/// Represents a single case in an SPSS data file.
	/// Allows for reading/writing variable data on a per-case basis.
	/// </summary>
	/// <remarks>
	/// In SPSS a case is analogous to a <see cref="System.Data.DataRow"/> 
	/// in ADO.NET.
	/// </remarks>
	public class SpssCase {
		/// <summary>
		/// Creates an instance of the <see cref="SpssCase"/> class.
		/// </summary>
		protected internal SpssCase(SpssCasesCollection cases, int position) {
			this.Cases = cases;
			this.Position = position;

			// Clear out all variables explicitly to prevent SPSS from assuming
			// garbage characters in those data areas.
			if (cases.IsAppendOnly) {
				this.ClearData();
			}
		}

		/// <summary>
		/// The index of the row within the data file.
		/// </summary>
		public int Position { get; private set; }

		/// <summary>
		/// Gets the containing <see cref="SpssCasesCollection"/>.
		/// </summary>
		protected SpssCasesCollection Cases { get; private set; }

		/// <summary>
		/// Gets the variables in the document.
		/// </summary>
		protected SpssVariablesCollection Variables {
			get {
				return Cases.Document.Variables;
			}
		}

		/// <summary>
		/// Gets or sets the value of some variable on this row.
		/// </summary>
		public object this[string varName] {
			get {
				this.EnsureActiveCase();
				return this.Variables[varName].Value;
			}

			set {
				this.EnsureActiveCase();
				this.Variables[varName].Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the value of some variable on this row.
		/// </summary>
		public object this[int columnIndex] {
			get {
				this.EnsureActiveCase();
				return this.Variables[columnIndex].Value;
			}

			set {
				this.EnsureActiveCase();
				this.Variables[columnIndex].Value = value;
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
		public void Commit() {
			this.Cases.Document.EnsureNotClosed();
			if (this.Cases.IsReadOnly) {
				throw new InvalidOperationException("Not available when in read-only mode.");
			}
			SpssSafeWrapper.spssCommitCaseRecordImpl(this.Cases.FileHandle);
			this.Cases.OnCaseCommitted();
		}

		/// <summary>
		/// Clears every variable of data for this row.
		/// </summary>
		/// <remarks>
		/// Remarkably, this operation is necessary for every new row, in order to 
		/// prevent garbage from being placed into SPSS for the values that are never
		/// explicitly set otherwise.
		/// </remarks>
		public void ClearData() {
			foreach (SpssVariable var in Cases.Document.Variables)
				this[var.Name] = null;
		}

		public object GetDBValue(string varName) {
			return (this[varName] != null) ? this[varName] : DBNull.Value;
		}

		public void SetDBValue(string varName, object value) {
			this[varName] = (value != DBNull.Value) ? value : null;
		}

		/// <summary>
		/// Ensures that the SPSS data file is currently pointing at this case's data.
		/// </summary>
		protected void EnsureActiveCase() {
			if (this.Position != this.Cases.Position) {
				if (this.Cases.IsAppendOnly) {
					throw new InvalidOperationException("This case is no longer the one being appended.");
				}

				this.Cases.Position = this.Position;
			}
		}
	}
}
