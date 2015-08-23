namespace Spss
{
    using System;
    using System.Collections;
    using System.Data;

    /// <summary>
    /// Manages the cases in an SPSS data file.  
    /// Supports reading, writing, and appending data.
    /// </summary>
    public sealed class SpssCasesCollection : IEnumerable
    {
        private int position = -1;

        private int caseCountInWriteMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssCasesCollection"/> class.
        /// </summary>
        /// <param name="document">The hosting document.</param>
        internal SpssCasesCollection(SpssDataDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            this.Document = document;
        }

        /// <summary>
        /// Gets a value indicating whether the set of cases is read only.  
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
        /// Gets a value indicating whether the set of cases can only be appended to.
        /// If no cases can be removed or changed, IsAppendOnly is true.
        /// <seealso cref="IsReadOnly"/>
        /// </summary>
        public bool IsAppendOnly
        {
            get
            {
                return this.Document.AccessMode != SpssFileAccess.Read; // Create or Append
            }
        }

        /// <summary>
        /// Gets the SPSS data document whose cases are being managed.
        /// </summary>
        public SpssDataDocument Document { get; private set; }

        /// <summary>
        /// Gets the file handle of the SPSS data document whose cases are being managed.
        /// </summary>
        internal Int32 FileHandle
        {
            get { return this.Document.Handle; }
        }

        /// <summary>
        /// The number of cases in the document.
        /// </summary>
        public int Count
        {
            get
            {
                Int32 casecount = 0;

                // New documents aren't allowed to query cases count, and appended docs
                // report the # of cases there were when the file was first opened.
                if (this.Document.AccessMode != SpssFileAccess.Create)
                {
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetNumberofCases(this.FileHandle, out casecount), "spssGetNumberofCases");
                }

                return casecount + this.caseCountInWriteMode;
            }
        }

        /// <summary>
        /// Gets or sets the case that SPSS is pointing at to get/set variable data on.
        /// </summary>
        internal int Position
        {
            get
            {
                return this.position;
            }

            set
            {
                if (IsAppendOnly)
                {
                    throw new InvalidOperationException("Not available while in Append mode.");
                }
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Position", value, "Must be a non-negative integer.");
                }
                if (value >= Count)
                {
                    throw new ArgumentOutOfRangeException("Position", value, "Must be less than the number of cases in the file.");
                }
                if (value == position)
                {
                    return; // nothing to do!
                }
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssSeekNextCase(this.FileHandle, value), "spssSeekNextCase");
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssReadCaseRecord(this.FileHandle), "spssReadCaseRecord");
                this.position = value;
            }
        }

        /// <summary>
        /// Gets the case record at a specific 0-based index.
        /// </summary>
        public SpssCase this[int index]
        {
            get { return new SpssCase(this, index); }
        }

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
            if (!IsAppendOnly)
            {
                throw new InvalidOperationException("Only available in append mode.");
            }

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
            foreach (SpssVariable var in this.Document.Variables)
            {
                DataColumn dc = new DataColumn(var.Name);
                if (var is SpssStringVariable)
                {
                    dc.DataType = typeof(string);
                }
                else if (var is SpssNumericVariable)
                {
                    dc.DataType = typeof(double);
                }
                else if (var is SpssDateVariable)
                {
                    dc.DataType = typeof(DateTime);
                }
                else
                {
                    throw new NotSupportedException("SPSS variable type " + var.GetType().Name + " is not supported.");
                }
                dt.Columns.Add(dc);
            }

            foreach (SpssCase spssCase in this)
            {
                DataRow row = dt.NewRow();
                foreach (SpssVariable var in this.Document.Variables)
                {
                    row[var.Name] = spssCase.GetDBValue(var.Name);
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Gets the enumerator that will iterate over all cases in the data file.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (IsAppendOnly)
            {
                throw new InvalidOperationException("Not available in append-only mode.");
            }

            return new SpssCasesCollectionEnumerator(this);
        }

        private class SpssCasesCollectionEnumerator : IEnumerator
        {
            private SpssCasesCollection Cases;

            private int position;

            /// <summary>
            /// Initializes a new instance of the <see cref="SpssCasesCollectionEnumerator"/> class.
            /// </summary>
            /// <param name="cases">The cases.</param>
            internal SpssCasesCollectionEnumerator(SpssCasesCollection cases)
            {
                if (cases == null)
                {
                    throw new ArgumentNullException("cases");
                }
                this.Cases = cases;
                this.Reset();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The enumerator is positioned before the first element of the collection or after the last element.
            /// </exception>
            public SpssCase Current
            {
                get
                {
                    return this.Cases[position];
                }
            }

            #region IEnumerator Members

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                return ++position < Cases.Count;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                position = -1;
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The enumerator is positioned before the first element of the collection or after the last element.
            /// </exception>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            #endregion
        }

        internal void OnCaseCommitted()
        {
            this.caseCountInWriteMode++;
        }
    }
}
