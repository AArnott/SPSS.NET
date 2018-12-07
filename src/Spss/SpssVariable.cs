// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents an SPSS data variable.
    /// </summary>
    public abstract class SpssVariable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariable"/> class.
        /// </summary>
        protected SpssVariable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssVariable"/> class.
        /// </summary>
        /// <param name="variables">The containing collection.</param>
        /// <param name="varName">The name of the variable.</param>
        protected SpssVariable(SpssVariablesCollection variables, string varName)
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            if (varName == null || varName.Length == 0)
            {
                throw new ArgumentNullException("varName");
            }

            this.variables = variables;
            this.AssumeIdentity(varName);
        }

        private void AssumeIdentity(string varName)
        {
            if (varName == null || varName.Length == 0)
            {
                throw new ArgumentNullException("varName");
            }

            ReturnCode result = SpssSafeWrapper.spssGetVarHandle(this.FileHandle, varName, out this.variableHandle);

            switch (result)
            {
                case ReturnCode.SPSS_OK:
                    break;
                case ReturnCode.SPSS_DICT_NOTCOMMIT:
                    // Header not yet saved, but that's ok.
                    // Just remember the name of the variable
                    break;
                case ReturnCode.SPSS_VAR_NOTFOUND:
                    throw new ArgumentOutOfRangeException("varName", varName, "SPSS returned: " + result);
                default:
                    throw new SpssException(result, "spssGetVarHandle");
            }

            this.name = varName;
        }

        internal static SpssVariable LoadVariable(SpssVariablesCollection parent, string varName, int varType)
        {
            FormatTypeCode writeFormat, printFormat;
            int writeDecimal, writeWidth, printDecimal, printWidth;
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarWriteFormat(parent.Document.Handle, varName, out writeFormat, out writeDecimal, out writeWidth), "spssGetVarWriteFormat");
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarPrintFormat(parent.Document.Handle, varName, out printFormat, out printDecimal, out printWidth), "spssGetVarPrintFormat");

            SpssVariable variable;
            switch (varType)
            {
                case 0:
                    // This may be a date or a numeric
                    if (SpssDateVariable.IsDateVariable(writeFormat))
                    {
                        variable = new SpssDateVariable(parent, varName, writeFormat, writeWidth, printFormat, printWidth);
                    }
                    else
                    {
                        variable = new SpssNumericVariable(parent, varName, writeFormat, writeDecimal, writeWidth, printFormat, printDecimal, printWidth);
                    }

                    break;
                default:
                    Debug.Assert(varType == printWidth);
                    variable = new SpssStringVariable(parent, varName, varType);
                    break;
            }

            return variable;
        }

        #region Attributes
        private bool committedThisSession = false;

        internal bool CommittedThisSession => this.committedThisSession;

        /// <summary>
        /// Gets a value indicating whether this variable has been added to a collection yet.
        /// </summary>
        protected internal bool IsInCollection => this.Variables != null;

        /// <summary>
        /// Gets a value indicating whether this variable has been committed to the SPSS data file.
        /// </summary>
        protected internal bool IsCommitted => this.Handle >= 0;

        private SpssVariablesCollection variables;

        /// <summary>
        /// Gets the collection of variables to which this one belongs.
        /// </summary>
        public SpssVariablesCollection Variables => this.variables;

        /// <summary>
        /// Gets the file handle of the SPSS data document whose variables are being managed.
        /// </summary>
        protected int FileHandle
        {
            get
            {
                if (!this.IsInCollection)
                {
                    throw new InvalidOperationException("This variable is not associated with a SPSS data file.");
                }

                return this.Variables.Document.Handle;
            }
        }

        private double variableHandle = -1;

        /// <summary>
        /// Gets the variable handle assigned by SPSS for this variable.
        /// </summary>
        protected double Handle => this.variableHandle;

        private string name;

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value == null || value.Length == 0)
                {
                    throw new ArgumentNullException("Name");
                }

                if (value.Length > SpssSafeWrapper.SPSS_MAX_VARNAME)
                {
                    throw new ArgumentOutOfRangeException("Name", value, "Too long.  Maximum variable name is " + SpssSafeWrapper.SPSS_MAX_VARNAME + " characters.");
                }

                this.VerifyNotCommittedVariable();

                // Ensure that this new name will not conflict with another variable.
                if (this.IsInCollection && this.Variables.Contains(value))
                {
                    throw new SpssVariableNameConflictException(value, this.name);
                }

                // Ensures that the look up table in SpssVariablesCollection are renamed as well.
                string oldName = this.name;
                this.name = value;
                if (this.Variables != null)
                {
                    this.Variables.ColumnNameUpdated(this, oldName);
                }
            }
        }

        private string label = null;

        /// <summary>
        /// Gets or sets the variable label.
        /// </summary>
        public string Label
        {
            get
            {
                // If this variable was read from an existing data file, the label
                // may not have been loaded yet.
                if (this.label == null && this.IsCommitted)
                {
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarLabel(this.FileHandle, this.Name, out this.label), "spssGetVarLabel", ReturnCode.SPSS_NO_LABEL);
                }

                return this.label ?? string.Empty;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                // Check to make sure the label is not too long
                if (value.Length > SpssSafeWrapper.SPSS_MAX_VARLABEL)
                {
                    throw new ArgumentOutOfRangeException("Label", value, "Label length maximum is " + SpssSafeWrapper.SPSS_MAX_VARLABEL);
                }

                this.label = value;
            }
        }

        /// <summary>
        /// Gets or sets the data value of this variable within a specific case.
        /// </summary>
        internal object Value
        {
            get
            {
                if (this is SpssNumericVariable)
                {
                    return ((SpssNumericVariable)this).Value;
                }
                else if (this is SpssStringVariable)
                {
                    return ((SpssStringVariable)this).Value;
                }
                else if (this is SpssDateVariable)
                {
                    return ((SpssDateVariable)this).Value;
                }
                else
                {
                    throw new NotSupportedException("Specific type of SpssVariable could not be determined and is not supported.");
                }
            }

            set
            {
                if (this is SpssNumericVariable)
                {
                    ((SpssNumericVariable)this).Value = (value == null) ? (double?)null : Convert.ToDouble(value);
                }
                else if (this is SpssStringVariable)
                {
                    ((SpssStringVariable)this).Value = (string)value;
                }
                else if (this is SpssDateVariable)
                {
                    ((SpssDateVariable)this).Value = (value == null) ? (DateTime?)null : (DateTime)value;
                }
                else
                {
                    throw new NotSupportedException("Specific type of SpssVariable could not be determined and is not supported.");
                }
            }
        }

        /// <summary>
        /// Gets the SPSS type for the variable.
        /// </summary>
        /// <value>For numeric/date types, this is 0.  For strings, this is the length of the column.</value>
        public abstract int SpssType { get; }

        protected const int ColumnWidthDefault = 8;
        private int columnWidth = -1;

        /// <summary>
        /// Gets or sets the width to reserve for this variable when printed.
        /// </summary>
        public int ColumnWidth
        {
            get
            {
                // If this variable was read from an existing file, and
                // width has not yet been retrieved, get it.
                if (this.columnWidth < 0 && this.Handle >= 0)
                {
                    SpssSafeWrapper.spssGetVarColumnWidth(this.FileHandle, this.Name, out this.columnWidth);
                }

                return this.columnWidth >= 0 ? this.columnWidth : ColumnWidthDefault;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("ColumnWidth", value, "Must be a positive integer.");
                }

                this.columnWidth = value;
            }
        }

        private MeasurementLevelCode measurementLevel = MeasurementLevelCode.SPSS_MLVL_UNK;

        /// <summary>
        /// Gets or sets the measurement level.
        /// </summary>
        /// <value>The measurement level.</value>
        public MeasurementLevelCode MeasurementLevel
        {
            get
            {
                // If this variable was read from an existing file, and
                // width has not yet been retrieved, get it.
                if (this.measurementLevel == MeasurementLevelCode.SPSS_MLVL_UNK && this.Handle >= 0)
                {
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarMeasureLevel(this.FileHandle, this.Name, out this.measurementLevel), "spssGetVarMeasureLevel");
                }

                return this.measurementLevel;
            }

            set
            {
                this.measurementLevel = value;
            }
        }

        private AlignmentCode alignment = AlignmentCode.SPSS_ALIGN_LEFT;

        /// <summary>
        /// Gets or sets the alignment of the variable.
        /// </summary>
        /// <value>The alignment.</value>
        public AlignmentCode Alignment
        {
            get
            {
                // If this variable was read from an existing file, get it.
                if (this.Handle >= 0)
                {
                    SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarAlignment(this.FileHandle, this.Name, out this.alignment), "spssGetVarAlignment");
                }

                return this.alignment;
            }

            set
            {
                this.alignment = value;
            }
        }

        #endregion

        #region Operations

        protected abstract bool IsApplicableFormatTypeCode(FormatTypeCode formatType);

        public IEnumerable<KeyValuePair<string, string>> GetValueLabels()
        {
            var numeric = this as SpssNumericVariable;
            if (numeric != null)
            {
                return numeric.ValueLabels.Select(pair => new KeyValuePair<string, string>(pair.Key.ToString(CultureInfo.InvariantCulture), pair.Value));
            }

            var str = this as SpssStringVariable;
            if (str != null)
            {
                return str.ValueLabels;
            }

            return Enumerable.Empty<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Clones the variable for use in another <see cref="SpssDataDocument"/>.
        /// </summary>
        public abstract SpssVariable Clone();

        /// <summary>
        /// Copies the fields from this variable into another previously created
        /// <see cref="SpssVariable"/>.
        /// </summary>
        protected virtual void CloneTo(SpssVariable other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            other.Name = this.Name;
            other.Label = this.Label;
            other.ColumnWidth = this.ColumnWidth;
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> when called
        /// after the variable has been committed to the dictionary.
        /// </summary>
        protected void VerifyNotCommittedVariable()
        {
            if (this.IsCommitted)
            {
                throw new InvalidOperationException("Cannot perform this operation after the variable has been committed.");
            }
        }

        /// <summary>
        /// Writes the variable's metadata out to the dictionary of the SPSS data file.
        /// </summary>
        protected internal void CommitToDictionary()
        {
            if (this.Handle >= 0)
            {
                throw new InvalidOperationException("Already committed.");
            }

            // Create the variable.
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarName(this.FileHandle, this.Name, this.SpssType), "spssSetVarName");

            // Call the descending class to finish the details.
            this.Update();
            this.committedThisSession = true;
        }

        /// <summary>
        /// Updates the changed attributes of the variable within SPSS.
        /// </summary>
        protected virtual void Update()
        {
            if (!this.IsInCollection)
            {
                return; // we'll get to do this later
            }

            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarLabel(this.FileHandle, this.Name, this.Label), "spssSetVarLabel");
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarColumnWidth(this.FileHandle, this.Name, this.ColumnWidth), "spssSetVarColumnWidth");
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarMeasureLevel(this.FileHandle, this.Name, this.MeasurementLevel), "spssSetVarMeasureLevel");
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarAlignment(this.FileHandle, this.Name, this.Alignment), "spssSetVarAlignment");
        }

        /// <summary>
        /// Informs this variable that it is being added to a <see cref="SpssVariablesCollection"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when this variable already belongs to a different collection.
        /// </exception>
        internal void AddToCollection(SpssVariablesCollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            if (this.Variables != null && this.Variables != variables)
            {
                throw new InvalidOperationException("Already belongs to a different collection.");
            }

            if (this.Name == null || this.Name.Length == 0)
            {
                throw new InvalidOperationException("SpssVariable.Name must be set first.");
            }

            // Make sure that a variable with this same name has not already been added to the collection.
            if (variables.Contains(this.Name) && !variables.Contains(this)) // and not this one
            {
                throw new SpssVariableNameConflictException(this.Name);
            }

            this.variables = variables;
            this.Variables.Document.DictionaryCommitted += new EventHandler(this.Document_DictionaryCommitted);
        }

        /// <summary>
        /// Informs this variable that it is being removed from a <see cref="SpssVariablesCollection"/>.
        /// </summary>
        internal void RemoveFromCollection(SpssVariablesCollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException("variables");
            }

            if (variables != this.Variables)
            {
                throw new ArgumentException("The variables collection being removed from does not match the collection this variable belongs to.");
            }

            this.Variables.Document.DictionaryCommitted -= new EventHandler(this.Document_DictionaryCommitted);
            this.variables = null; // remove reference to owning collection
        }
        #endregion

        #region Events
        private void Document_DictionaryCommitted(object sender, EventArgs e)
        {
            // Set the variable handle
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarHandle(this.FileHandle, this.Name, out this.variableHandle), "spssGetVarHandle");
        }
        #endregion
    }
}
