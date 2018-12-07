// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an SPSS data variable that stores character string information.
    /// </summary>
    public class SpssStringVariable : SpssVariable
    {
        private readonly SpssStringVariableValueLabelsDictionary valueLabels;

        private int length = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssStringVariable"/> class
        /// for use when defining a new variable.
        /// </summary>
        public SpssStringVariable()
        {
            this.valueLabels = new SpssStringVariableValueLabelsDictionary(this);
            this.MissingValues = new List<string>(3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssStringVariable"/> class
        /// for use in loading variables from an existing SPSS data file.
        /// </summary>
        /// <param name="variables">The containing collection.</param>
        /// <param name="varName">The name of the variable being loaded.</param>
        /// <param name="length">The length of the string variable.  This is the same as SpssType</param>
        protected internal SpssStringVariable(SpssVariablesCollection variables, string varName, int length)
            : base(variables, varName)
        {
            this.valueLabels = new SpssStringVariableValueLabelsDictionary(this);
            this.length = length;

            MissingValueFormatCode formatCode;
            string[] missingValues = new string[3];
            ReturnCode result = SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarCMissingValues(this.FileHandle, this.Name, out formatCode, out missingValues[0], out missingValues[1], out missingValues[2]), "spssGetVarCMissingValues", ReturnCode.SPSS_SHORTSTR_EXP);
            if (result == ReturnCode.SPSS_OK)
            {
                this.MissingValues = new List<string>(missingValues.Take((int)formatCode));
            }
            else
            {
                this.MissingValues = new List<string>(0);
            }
        }

        /// <summary>
        /// Gets or sets the maximum length a string in this variable can be.
        /// </summary>
        public int Length
        {
            get
            {
                return this.length >= 0 ? this.length : this.ColumnWidth;
            }

            set
            {
                this.VerifyNotCommittedVariable();
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Length", value, "Must be a non-negative number.");
                }

                this.length = value;
            }
        }

        /// <summary>
        /// Gets the missing values for this variable.
        /// </summary>
        /// <value>The missing values.</value>
        /// <remarks>
        /// A maximum of three maximum values may be supplied.
        /// </remarks>
        public IList<string> MissingValues { get; private set; }

        /// <summary>
        /// Gets the SPSS type for the variable.
        /// </summary>
        public override int SpssType => this.Length;

        /// <summary>
        /// Gets or sets the data value of this variable within a specific case.
        /// </summary>
        internal new string Value
        {
            get
            {
                string v;
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetValueChar(this.FileHandle, this.Handle, out v), "spssGetValueChar");
                return v;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                if (value.Length > this.Length)
                {
                    throw new ArgumentOutOfRangeException("Value", value, "String too long for variable " + this.Name + ".  Maximum length is: " + this.Length);
                }

                SpssSafeWrapper.spssSetValueChar(this.FileHandle, this.Handle, value);
            }
        }

        /// <summary>
        /// Gets the set of value labels (response values and labels) that are defined.
        /// </summary>
        public IDictionary<string, string> ValueLabels => this.valueLabels;

        /// <summary>
        /// Updates the changed attributes of the variable within SPSS.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (!this.IsInCollection)
            {
                return; // we'll get to do this later
            }

            this.valueLabels.Update();
            string[] missingValues = new string[3];
            this.MissingValues.Take(missingValues.Length).ToArray().CopyTo(missingValues, 0);

            // We allow failure due to long string var types only if we're not actually setting any missing values.
            ReturnCode[] allowedReturnCodes = this.MissingValues.Count > 0 ? new ReturnCode[0] : new ReturnCode[] { ReturnCode.SPSS_SHORTSTR_EXP };
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarCMissingValues(
                this.FileHandle,
                this.Name,
                (MissingValueFormatCode)Math.Min(this.MissingValues.Count, missingValues.Length),
                missingValues[0],
                missingValues[1],
                missingValues[2]), "spssSetVarCMissingValues", allowedReturnCodes);
        }

        public override SpssVariable Clone()
        {
            SpssStringVariable other = new SpssStringVariable();
            this.CloneTo(other);
            return other;
        }

        protected override void CloneTo(SpssVariable spssVar)
        {
            base.CloneTo(spssVar);
            SpssStringVariable other = spssVar as SpssStringVariable;
            if (other == null)
            {
                throw new ArgumentException("Must be of type " + this.GetType().Name + ".", "other");
            }

            other.Length = this.Length;
            this.valueLabels.CopyTo(other.valueLabels);
        }

        protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType)
        {
            return formatType == FormatTypeCode.SPSS_FMT_A;
        }
    }
}
