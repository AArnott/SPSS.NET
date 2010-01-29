namespace Spss
{
	using System;
	using System.IO;
	using System.Data;
	using System.Collections;
	using System.Diagnostics;
	using System.Globalization;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	/// <summary>
	/// Class to manage the metadata of variables in an <see cref="SpssDataDocument">SPSS data document</see>.
	/// </summary>
	public sealed class SpssVariablesCollection : IList<SpssVariable>
	{
		/// <summary>
		/// The list of variables in the file, or that will shortly be committed to the file.
		/// </summary>
		private List<SpssVariable> variables;
	
		private KeyedCollection<string, SpssVariable> variablesLookup;

		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssVariablesCollection"/> class.
		/// </summary>
		/// <param name="document">
		/// The hosting SPSS data document whose variables will be managed by
		/// this instance.
		/// </param>
		internal SpssVariablesCollection(SpssDataDocument document)
		{
			if( document == null ) throw new ArgumentNullException("document");
			this.document = document;
			
			InitializeVariablesList();
		}
		private void InitializeVariablesList() {
			Debug.Assert(FileHandle >= 0, "Must be working with an open file.");
			int initialSize;
			ReturnCode result = SpssSafeWrapper.spssGetNumberofVariables(FileHandle, out initialSize);
			if (result != ReturnCode.SPSS_OK) {
				throw new SpssException(result, "spssGetNumberofVariables");
			}
			variables = new List<SpssVariable>(initialSize);
			variablesLookup = new SpssVariableKeyedCollection();

			string[] varNames;
			int[] varTypes;
			result = SpssSafeWrapper.spssGetVarNames(FileHandle, out varNames, out varTypes);
			if (result == ReturnCode.SPSS_INVALID_FILE) {
				// brand new file
				return;
			} else if (result != ReturnCode.SPSS_OK) {
				throw new SpssException(result, "spssGetVarNames");
			}
			Debug.Assert(varNames.Length == varTypes.Length);
			for (int i = 0; i < varNames.Length; i++)
				Add(SpssVariable.LoadVariable(this, varNames[i], varTypes[i]));
		}
		#endregion

		#region Indexers

		/// <summary>
		/// Gets the variable with a given name.
		/// </summary>
		public SpssVariable this [string varName]
		{
			get { return variablesLookup[varName]; }
		}

		#endregion

		#region Attributes
		private readonly SpssDataDocument document;
		/// <summary>
		/// The SPSS data document whose variables are being managed.
		/// </summary>
		public SpssDataDocument Document { get { return document; } }
		/// <summary>
		/// The file handle of the SPSS data document whose variables are being managed.
		/// </summary>
		private Int32 FileHandle
		{
			get
			{
				return Document.Handle;
			}
		}
		/// <summary>
		/// Gets whether a variable has been added to this document.
		/// </summary>
		public bool Contains(SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");

			return variables.Contains( variable );
		}
		/// <summary>
		/// Gets whether a variable exists in this document.
		/// </summary>
		/// <param name="varName">
		/// The name of the variable in question.
		/// </param>
		/// <returns>
		/// True if the variable already exists in the document.  False otherwise.
		/// </returns>
		public bool Contains(string varName)
		{
			if( varName == null || varName.Length == 0 ) 
				throw new ArgumentNullException("varName");

			return variablesLookup.Contains(varName);
		}
		/// <summary>
		/// Gets the position of a variable.
		/// </summary>
		/// <returns>
		/// The index of the variable, or -1 if not found.
		/// </returns>
		public int IndexOf(SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");
			return variables.IndexOf(variable);
		}
		/// <summary>
		/// Gets the position of the variable with a given name.
		/// </summary>
		/// <returns>
		/// The index of the variable, or -1 if not found.
		/// </returns>
		public int IndexOf(string varName)
		{
			if( varName == null || varName.Length == 0 ) throw new ArgumentNullException("varName");
			return IndexOf(variablesLookup[varName]);
		}
		#endregion

		#region Operations
		/// <summary>
		/// Adds a variable to the document at a specific index.
		/// </summary>
		public void Insert(int index, SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");
			EnsureAuthoringDictionary();
			variable.AddToCollection(this);
			variablesLookup.Add(variable);
			variables.Insert(index, variable);
		}
		/// <summary>
		/// Adds a variable to the document.
		/// </summary>
		/// <returns>
		/// The index of the newly added variable.
		/// </returns>
		public void Add(SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");
			EnsureAuthoringDictionary();
			variable.AddToCollection(this);
			variablesLookup.Add(variable);
			variables.Add(variable);
		}
		/// <summary>
		/// Removes a variable from the document.
		/// </summary>
		public bool Remove(SpssVariable variable)
		{
			if( variable == null ) throw new ArgumentNullException("variable");
			EnsureAuthoringDictionary();
			try {
				variable.RemoveFromCollection(this);
			} catch (ArgumentException) {
				return false;
			}
			variables.Remove( variable );
			variablesLookup.Remove(variable.Name);
			return true;
		}

		/// <summary>
		/// Writes the variables to the dictionary.
		/// </summary>
		internal void Commit()
		{
			EnsureAuthoringDictionary();

			// Write the variables we have been caching into the data file.
			foreach( SpssVariable var in this )
				var.CommitToDictionary();
		}

		private void EnsureAuthoringDictionary()
		{
			Document.EnsureAuthoringDictionary();
		}
		/// <summary>
		/// Copies the definition of variables from this file to another.
		/// </summary>
		public void CopyTo(SpssVariablesCollection other, int index)
		{
			if( other == null ) throw new ArgumentNullException("other");
			if( other == this ) throw new ArgumentException("Must be a different variables collection.", "other");
			
			throw new NotImplementedException();
		}

		/// <summary>
		/// Defines the variables in the SPSS data file so that they mirror
		/// those defined in a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="table">
		/// The DataTable whose list of columns are the ones we want to copy.
		/// </param>
		/// <param name="fillInMetadataCallback">
		/// The callback method to use to retrieve the additional metadata 
		/// to put into the SPSS data document, that is not included in a DataTable.
		/// Optional.
		/// </param>
		public void ImportSchema(DataTable table, MetadataProviderCallback fillInMetadataCallback) 
		{
			foreach( DataColumn column in table.Columns ) 
			{
				try 
				{
					SpssVariable var;
					if( column.DataType == typeof(string) ) 
					{
						var = new SpssStringVariable();
						((SpssStringVariable)var).Length = (column.MaxLength < 0 || column.MaxLength > SpssSafeWrapper.SPSS_MAX_LONGSTRING) ? SpssSafeWrapper.SPSS_MAX_LONGSTRING : column.MaxLength;
					}
					else if( column.DataType == typeof(DateTime) ) 
						var = new SpssDateVariable();
					else 
					{
						var = new SpssNumericVariable();
						if( column.DataType == typeof(float) || column.DataType == typeof(double) )
							((SpssNumericVariable)var).DecimalPlaces = 2;
					}

					var.Name = GenerateColumnName(column.ColumnName);
					Add(var);

					// Provide opportunity for callback function to fill in variable-specific metadata
					if( fillInMetadataCallback != null && var != null ) 
						try 
						{
							VarMetaData varMetaData = new VarMetaData( var, column.ColumnName );
							fillInMetadataCallback( varMetaData );
							varMetaData.ApplyToSpssVar();
						}
						catch( Exception ex ) 
						{
							throw new ApplicationException("Exception in metadata filler callback function on column " + column.ColumnName + ".",ex );
						}
				}
				catch( Exception ex ) 
				{
					throw new ApplicationException("Error adding column " + column.ColumnName + " schema information to the SPSS .SAV data file.", ex);
				}
			}
		}
		/// <summary>
		/// Defines the variables in the SPSS data file so that they mirror
		/// those defined in a <see cref="DataTable"/>.
		/// </summary>
		/// <param name="table">
		/// The DataTable whose list of columns are the ones we want to copy.
		/// </param>
		public void ImportSchema(DataTable table) 
		{
			ImportSchema( table, null );
		}

		/// <summary>
		/// Comes up with a variable name guaranteed to be short enough 
		/// to fit within the limits of SPSS.
		/// </summary>
		/// <param name="colName">
		/// The initially suggested name for a variable.
		/// </param>
		/// <returns>
		/// The original variable name, if it was within SPSS limits.
		/// Otherwise, it applies a string shortening algorithm and returns
		/// the shorter variable name.
		/// </returns>
		/// <remarks>
		/// The shortening algorithm takes the first and last several characters of the 
		/// variable name and concatenates them together such that the resulting
		/// string is exactly the allowed length for a variable name.
		/// The process is not guaranteed to produce a unique variable name.
		/// </remarks>
		internal string GenerateColumnName(string colName)  
		{
			if( colName.Length > SpssThinWrapper.SPSS_MAX_VARNAME )
				colName = colName.Substring(0, SpssThinWrapper.SPSS_MAX_VARNAME / 2) + colName.Substring(colName.Length - SpssThinWrapper.SPSS_MAX_VARNAME / 2);
			return colName;
		}

		/// <summary>
		/// Called when a <see cref="SpssVariable.Name"/> changes so that the lookup
		/// table can be updated.
		/// </summary>
		internal void ColumnNameUpdated(SpssVariable variable, string oldName)
		{
			variablesLookup.Remove(oldName);
			variablesLookup.Add(variable);
		}
		#endregion

		#region IList<SpssVariable> Members

		/// <summary>
		/// Gets the variable at some 0-based index.
		/// </summary>
		public SpssVariable this[int index] {
			get { return variables[index]; }
			set { variables[index] = value; }
		}

		#endregion

		#region ICollection<SpssVariable> Members

		public void CopyTo(SpssVariable[] array, int arrayIndex) {
			throw new NotImplementedException();
		}

		#endregion

		#region IEnumerable<SpssVariable> Members

		IEnumerator<SpssVariable> IEnumerable<SpssVariable>.GetEnumerator() {
			return this.variables.GetEnumerator();
		}

		#endregion

		#region IList<SpssVariable> Members


		public void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		#endregion

		#region ICollection<SpssVariable> Members

		public void Clear() {
			EnsureAuthoringDictionary();
			while (this.variables.Count > 0) {
				this.Remove(this.variables[0]);
			}
		}

		public int Count {
			get { return this.variables.Count; }
		}

		public bool IsReadOnly {
			get { return !this.Document.IsAuthoringDictionary; }
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator() {
			return this.variables.GetEnumerator();
		}

		#endregion
	}
}
