using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Diagnostics;

namespace Spss
{
	/// <summary>
	/// The three levels of file access supported by SPSS.
	/// </summary>
	public enum SpssFileAccess
	{
		/// <summary>
		/// A new file is being written.
		/// </summary>
		Create,
		/// <summary>
		/// Read-only access to metadata and data.
		/// </summary>
		Read,
		/// <summary>
		/// Data is being added to an existing file.
		/// </summary>
		Append,
	}

	/// <summary>
	/// Manages reading from and writing to SPSS data files.
	/// </summary>
	public class SpssDataDocument : IDisposable
	{		
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssDataDocument"/> class, 
		/// and opens an existing SPSS file, or creates a new one.
		/// </summary>
		/// <param name="filename">
		/// The path of the file to open/create.
		/// </param>
		/// <param name="access">
		/// The desired file access.
		/// </param>
		protected SpssDataDocument(string filename, SpssFileAccess access)
		{
			this.filename = filename;
			this.accessMode = access;

			switch( access )
			{
				case SpssFileAccess.Read:
					SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenRead(filename, out handle), "spssOpenRead");
					break;
				case SpssFileAccess.Append:
					SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenAppend(filename, out handle), "spssOpenAppend");
					break;
				case SpssFileAccess.Create:
					SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenWrite(filename, out handle), "spssOpenWrite");
					IsCompressed = true;
					break;
				default:
					throw new ApplicationException("Unrecognized access level: " + access);
			}

			isAuthoringDictionary = true;
			variables = new SpssVariablesCollection(this);
			cases = new SpssCasesCollection(this);
			isAuthoringDictionary = access == SpssFileAccess.Create;
		}
		#endregion

		#region Attributes
		private bool isAuthoringDictionary = false;
		/// <summary>
		/// Gets whether the data file is in a dictionary writing state.
		/// </summary>
		/// <remarks>
		/// Dictionary writing is the first step in authoring an SPSS data file,
		/// and must be completed before any data rows is written to the file.
		/// </remarks>
		public bool IsAuthoringDictionary
		{
			get
			{
				return isAuthoringDictionary;
			}
		}

		private Int32 handle = -1;
		/// <summary>
		/// Gets the SPSS file handle for the open document.
		/// </summary>
		protected internal Int32 Handle
		{
			get
			{
				return handle;
			}
		}
		private SpssFileAccess accessMode;
		/// <summary>
		/// Gets whether this document is open for read or write access.
		/// </summary>
		public SpssFileAccess AccessMode
		{
			get
			{
				return accessMode;
			}
		}
		private string filename;
		/// <summary>
		/// Gets the filename of the open document.
		/// </summary>
		public string Filename
		{
			get
			{
				return filename;
			}
		}
		/// <summary>
		/// Gets whether this document has been closed.
		/// </summary>
		public bool IsClosed
		{
			get
			{
				return Handle < 0;
			}
		}
		/// <summary>
		/// Gets/sets whether the SPSS file is compressed on disk.
		/// </summary>
		public bool IsCompressed
		{
			get
			{
				int compressed;
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetCompression(Handle, out compressed), "SpssSafeWrapper");
				return compressed != 0;
			}
			set
			{
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetCompression(Handle, value ? 1 : 0), "SpssSafeWrapper");
			}
		}
		/// <summary>
		/// Gets the SPSS-defined system missing value.
		/// </summary>
		/// <remarks>
		/// Setting a numeric variable to this value is equivalent in purpose
		/// to setting DBNull.Value in a database.
		/// </remarks>
		protected internal static double SystemMissingValue
		{
			get
			{
				return SpssThinWrapper.spssSysmisVal();
			}
		}
		#endregion

		#region Collections
		private readonly SpssVariablesCollection variables;
		/// <summary>
		/// The set of variables defined in the SPSS data file.
		/// </summary>
		public SpssVariablesCollection Variables
		{
			get
			{
				return variables;
			}
		}
		private readonly SpssCasesCollection cases;
		/// <summary>
		/// The set of all cases saved in the data file.
		/// </summary>
		/// <value>
		/// In SPSS, a case is a row of data.
		/// </value>
		public SpssCasesCollection Cases
		{
			get
			{
				return cases;
			}
		}
		#endregion

		#region Operations
		/// <summary>
		/// Opens an SPSS data document for reading or appending.
		/// </summary>
		/// <param name="filename">
		/// The filename of the existing document to open.
		/// </param>
		/// <param name="access">
		/// <see cref="FileAccess.Read"/> for read only access, or 
		/// <see cref="FileAccess.Write"/> for append access.
		/// </param>
		/// <returns>
		/// The newly opened <see cref="SpssDataDocument">SPSS data document</see>.
		/// </returns>
		/// <remarks>
		/// This method is only for opening existing data documents.
		/// To create a new document, use the <see cref="Create(string)"/> method.
		/// </remarks>
		public static SpssDataDocument Open( string filename, SpssFileAccess access ) 
		{
			if( access == SpssFileAccess.Create )
				throw new ArgumentOutOfRangeException("access", access, "Use Create method to create a new file.");

			return new SpssDataDocument(filename, access);
		}
		/// <summary>
		/// Creates a new SPSS data document.
		/// </summary>
		/// <param name="filename">
		/// The filename of the new document to create.
		/// </param>
		/// <returns>
		/// The newly created <see cref="SpssDataDocument">SPSS data document</see>.
		/// </returns>
		public static SpssDataDocument Create( string filename )
		{
			if( File.Exists( filename ) )
				throw new InvalidOperationException("File to create already exists.");

			return new SpssDataDocument(filename, SpssFileAccess.Create);
		}

		/// <summary>
		/// Creates a new SPSS data document, initializing its dictionary 
		/// by copying the dictionary from an existing SPSS data file.
		/// </summary>
		/// <param name="filename">
		/// The filename of the new document to create.
		/// </param>
		/// <param name="copyDictionaryFromFileName">
		/// The filename of the existing SPSS data file to copy the dictionary from.
		/// </param>
		/// <returns>
		/// The newly created <see cref="SpssDataDocument">SPSS data document</see>.
		/// </returns>
		public static SpssDataDocument Create( string filename, string copyDictionaryFromFileName )
		{
			if( File.Exists( filename ) )
				throw new InvalidOperationException("File to create already exists.");
			if( !File.Exists( copyDictionaryFromFileName ) )
				throw new FileNotFoundException("File to copy does not exist.", copyDictionaryFromFileName);
			using (SpssDataDocument read = SpssDataDocument.Open(copyDictionaryFromFileName, SpssFileAccess.Read))
			{
				SpssDataDocument toReturn = new SpssDataDocument(filename, SpssFileAccess.Create);

				foreach (SpssVariable var in read.variables)
					toReturn.Variables.Add(var.Clone());

				toReturn.CommitDictionary();
				return toReturn;
			}
		}
		/// <summary>
		/// Closes the SAV that is open for reading or writing.  
		/// If no file is open, close() simply returns.
		/// </summary>
		public void Close()
		{
			if( IsClosed ) return; // already closed

			lock( this )
			{
				switch( AccessMode ) 
				{
					case SpssFileAccess.Read:
						SpssException.ThrowOnFailure(SpssSafeWrapper.spssCloseRead(handle), "spssCloseRead");
						break;
					case SpssFileAccess.Append:
						SpssException.ThrowOnFailure(SpssSafeWrapper.spssCloseAppend(handle), "spssCloseAppend");
						break;
					case SpssFileAccess.Create:
						SpssException.ThrowOnFailure(SpssSafeWrapper.spssCloseWrite(handle), "spssCloseWrite", ReturnCode.SPSS_DICT_NOTCOMMIT);
						break;
					default:
						throw new ApplicationException("Unrecognized access level: " + AccessMode );
				}
				handle = -1; // mark that we don't have any handle
			
				// We don't need to have Dispose called later.
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>
		/// Commits the dictionary of a newly created SPSS file.
		/// </summary>
		/// <remarks>
		/// This method should be called after all variables
		/// have been added to the data file.
		/// </remarks>
		public void CommitDictionary() 
		{
			EnsureAuthoringDictionary();
			Variables.Commit();

			SpssException.ThrowOnFailure(SpssSafeWrapper.spssCommitHeader(handle), "SpssSafeWrapper");

			isAuthoringDictionary = false;

			OnDictionaryCommitted();
		}

		/// <summary>
		/// Imports data (and optionally metadata) into the document.
		/// </summary>
		public void ImportData(DataTable table, IEnumerable<DataRow> data) 
		{
			if( IsAuthoringDictionary ) 
			{
				// First import schema
				Variables.ImportSchema( table );
				CommitDictionary();
			}

			System.Collections.IEnumerable dataRows = data;
			if (dataRows == null) dataRows = table.Rows;
			foreach( DataRow row in dataRows )
			{
				SpssCase Case = Cases.New();
				for( int col = 0; col < table.Columns.Count; col++ ) 
					Case.SetDBValue(Variables.GenerateColumnName( table.Columns[col].ColumnName ), row[col]);
				Case.Commit();
			}
		}

		internal void EnsureNotClosed()
		{
			if( IsClosed )
				throw new InvalidOperationException("Cannot perform this operation after the document has been closed.");
		}
		internal void EnsureAuthoringDictionary()
		{
			EnsureNotClosed();
			if( !IsAuthoringDictionary )
				throw new InvalidOperationException("Cannot perform this operation unless the file has just been created.");			
		}
		
		/// <summary>
		/// Raises the <see cref="DictionaryCommitted"/> event.
		/// </summary>
		protected void OnDictionaryCommitted()
		{
			EventHandler dictionaryCommitted = DictionaryCommitted;
			if( dictionaryCommitted != null )
				dictionaryCommitted(this, null);
		}
		#endregion

		#region Events
		/// <summary>
		/// An event raised when the dictionary has just been committed.
		/// </summary>
		protected internal event EventHandler DictionaryCommitted;
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Disposes of all unmanaged resources held by the <see cref="SpssDataDocument"/>.
		/// </summary>
		~SpssDataDocument() 
		{
			Dispose();
		}
		/// <summary>
		/// Disposes of all unmanaged resources held by the <see cref="SpssDataDocument"/>.
		/// </summary>
		public void Dispose()
		{
			Close();
		}

		#endregion
	}
}
