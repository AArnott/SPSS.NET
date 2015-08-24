//-----------------------------------------------------------------------
// <copyright file="SpssDataDocument.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Brigham Young University
// </copyright>
//-----------------------------------------------------------------------

namespace Spss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="SpssDataDocument"/> class
        /// and opens an existing SPSS file, or creates a new one.
        /// </summary>
        /// <param name="filename">The path of the file to open/create.</param>
        /// <param name="access">The desired file access.</param>
        protected SpssDataDocument(string filename, SpssFileAccess access)
        {
            this.Filename = filename;
            this.AccessMode = access;
            int handle;
            switch (access)
            {
                case SpssFileAccess.Read:
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenRead(filename, out handle), "spssOpenRead");
                    break;
                case SpssFileAccess.Append:
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenAppend(filename, out handle), "spssOpenAppend");
                    break;
                case SpssFileAccess.Create:
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssOpenWrite(filename, out handle), "spssOpenWrite");
                    break;
                default:
                    throw new ApplicationException("Unrecognized access level: " + access);
            }

            this.Handle = new SpssSafeHandle(handle, access);
            if (access == SpssFileAccess.Create)
            {
                this.IsCompressed = true;
            }
            this.IsAuthoringDictionary = true;
            this.Variables = new SpssVariablesCollection(this);
            this.Cases = new SpssCasesCollection(this);
            this.IsAuthoringDictionary = access == SpssFileAccess.Create;
        }

        /// <summary>
        /// An event raised when the dictionary has just been committed.
        /// </summary>
        protected internal event EventHandler DictionaryCommitted;

        /// <summary>
        /// Gets a value indicating whether the data file is in a dictionary writing state.
        /// </summary>
        /// <remarks>
        /// Dictionary writing is the first step in authoring an SPSS data file,
        /// and must be completed before any data rows is written to the file.
        /// </remarks>
        public bool IsAuthoringDictionary { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this document is open for read or write access.
        /// </summary>
        public SpssFileAccess AccessMode { get; private set; }

        /// <summary>
        /// Gets the filename of the open document.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this document has been closed.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return this.Handle.IsClosed;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the SPSS file is compressed on disk.
        /// </summary>
        public bool IsCompressed
        {
            get
            {
                int compressed;
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetCompressionImpl(this.Handle, out compressed), "spssGetCompression");
                return compressed != 0;
            }

            set
            {
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetCompression(this.Handle, value ? 1 : 0), "spssSetCompression");
            }
        }

        /// <summary>
        /// Gets the set of variables defined in the SPSS data file.
        /// </summary>
        public SpssVariablesCollection Variables { get; private set; }

        /// <summary>
        /// Gets the set of all cases saved in the data file.
        /// </summary>
        /// <value>
        /// In SPSS, a case is a row of data.
        /// </value>
        public SpssCasesCollection Cases { get; private set; }

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
                return SpssThinWrapper.spssSysmisValImpl();
            }
        }

        /// <summary>
        /// Gets the SPSS file handle for the open document.
        /// </summary>
        protected internal SpssSafeHandle Handle { get; private set; }

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
        public static SpssDataDocument Open(string filename, SpssFileAccess access)
        {
            if (access == SpssFileAccess.Create)
            {
                throw new ArgumentOutOfRangeException("access", access, "Use Create method to create a new file.");
            }
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
        public static SpssDataDocument Create(string filename)
        {
            if (File.Exists(filename))
            {
                throw new InvalidOperationException("File to create already exists.");
            }
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
        public static SpssDataDocument Create(string filename, string copyDictionaryFromFileName)
        {
            if (File.Exists(filename))
            {
                throw new InvalidOperationException("File to create already exists.");
            }
            if (!File.Exists(copyDictionaryFromFileName))
            {
                throw new FileNotFoundException("File to copy does not exist.", copyDictionaryFromFileName);
            }
            using (SpssDataDocument read = SpssDataDocument.Open(copyDictionaryFromFileName, SpssFileAccess.Read))
            {
                SpssDataDocument toReturn = new SpssDataDocument(filename, SpssFileAccess.Create);

                foreach (SpssVariable var in read.Variables)
                {
                    toReturn.Variables.Add(var.Clone());
                }

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
            this.Handle.Close();
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
            this.EnsureAuthoringDictionary();
            this.Variables.Commit();

            SpssException.ThrowOnFailure(SpssSafeWrapper.spssCommitHeaderImpl(this.Handle), "spssCommitHeader");

            this.IsAuthoringDictionary = false;

            this.OnDictionaryCommitted();
        }

        /// <summary>
        /// Imports data (and optionally metadata) into the document.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="data">The data, which may just be <paramref name="table"/>.Rows.</param>
        public void ImportData(DataTable table, IEnumerable<DataRow> data)
        {
            if (this.IsAuthoringDictionary)
            {
                // First import schema
                this.Variables.ImportSchema(table);
                this.CommitDictionary();
            }

            System.Collections.IEnumerable dataRows = data;
            if (dataRows == null)
            {
                dataRows = table.Rows;
            }
            foreach (DataRow row in dataRows)
            {
                SpssCase caseRow = this.Cases.New();
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    caseRow.SetDBValue(this.Variables.GenerateColumnName(table.Columns[col].ColumnName), row[col]);
                }
                caseRow.Commit();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws an InvalidOperationException if the document has been closed.
        /// </summary>
        internal void EnsureNotClosed()
        {
            if (this.IsClosed)
            {
                throw new InvalidOperationException("Cannot perform this operation after the document has been closed.");
            }
        }

        /// <summary>
        /// Throws an InvalidOperationException if we're not in authoring dictionary mode.
        /// </summary>
        internal void EnsureAuthoringDictionary()
        {
            this.EnsureNotClosed();
            if (!this.IsAuthoringDictionary)
            {
                throw new InvalidOperationException("Cannot perform this operation unless the file has just been created.");
            }
        }

        /// <summary>
        /// Raises the <see cref="DictionaryCommitted"/> event.
        /// </summary>
        protected void OnDictionaryCommitted()
        {
            EventHandler dictionaryCommitted = this.DictionaryCommitted;
            if (dictionaryCommitted != null)
            {
                dictionaryCommitted(this, null);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Close();
            }

            // We don't clean up our SPSS handle if !disposing because it's a SafeHandle, which will take care of itself.
            // And SafeHandle is a class, and we should never touch references during finalization.
        }
    }
}
