//-----------------------------------------------------------------------
// <copyright file="SpssDateVariable.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Brigham Young University
// </copyright>
//-----------------------------------------------------------------------

namespace Spss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an SPSS data variable that stores date information.
    /// </summary>
    public class SpssDateVariable : SpssVariable
    {
        private FormatTypeCode writeFormat;
        private FormatTypeCode printFormat;
        private int writeWidth;
        private int printWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssDateVariable"/> class
        /// for use when defining a new variable.
        /// </summary>
        public SpssDateVariable()
        {
            this.WriteFormat = this.PrintFormat = FormatTypeCode.SPSS_FMT_DATE_TIME;
            this.WriteWidth = this.PrintWidth = 28;
            this.MissingValues = new List<DateTime>(3);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpssDateVariable"/> class.
        /// for use in loading variables from an existing SPSS data file.
        /// </summary>
        /// <param name="variables">The containing collection.</param>
        /// <param name="varName">The name of the variable.</param>
        /// <param name="writeFormat">The write format.</param>
        /// <param name="writeWidth">Width of the write.</param>
        /// <param name="printFormat">The print format.</param>
        /// <param name="printWidth">Width of the print.</param>
        protected internal SpssDateVariable(SpssVariablesCollection variables, string varName, FormatTypeCode writeFormat, int writeWidth, FormatTypeCode printFormat, int printWidth)
            : base(variables, varName)
        {
            MissingValueFormatCode formatCode;
            double[] missingValues = new double[3];
            ReturnCode result = SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarNMissingValues(this.FileHandle, this.Name, out formatCode, out missingValues[0], out missingValues[1], out missingValues[2]), "spssGetVarNMissingValues");
            this.MissingValueFormat = formatCode;
            this.MissingValues = new List<DateTime>(missingValues.Take(Math.Abs((int)formatCode)).Select(v => ConvertDoubleToDateTime(v)));
            this.writeFormat = writeFormat;
            this.writeWidth = writeWidth;
            this.printFormat = printFormat;
            this.printWidth = printWidth;
        }

        /// <summary>
        /// Gets or sets the missing values for this variable.
        /// </summary>
        /// <value>The missing values.</value>
        /// <remarks>
        /// A maximum of three maximum values may be supplied.
        /// </remarks>
        public IList<DateTime> MissingValues { get; private set; }

        public MissingValueFormatCode MissingValueFormat { get; set; }

        /// <summary>
        /// Gets the SPSS type for the variable.
        /// </summary>
        public override int SpssType => 0; // date variables are numerics

        public virtual FormatTypeCode WriteFormat
        {
            get
            {
                return this.writeFormat;
            }

            set
            {
                if (!this.IsApplicableFormatTypeCode(value))
                {
                    throw new ArgumentOutOfRangeException("value", "This value does not apply to this type of SPSS variable.");
                }

                this.writeFormat = value;
            }
        }

        public virtual FormatTypeCode PrintFormat
        {
            get
            {
                return this.printFormat;
            }

            set
            {
                if (!this.IsApplicableFormatTypeCode(value))
                {
                    throw new ArgumentOutOfRangeException("value", "This value does not apply to this type of SPSS variable.");
                }

                this.printFormat = value;
            }
        }

        public int WriteWidth
        {
            get
            {
                return this.writeWidth;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.writeWidth = value;
            }
        }

        public int PrintWidth
        {
            get
            {
                return this.printWidth;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                this.printWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the data value of this variable within a specific case.
        /// </summary>
        /// <remarks>
        /// Null values are translated to and from 
        /// <see cref="SpssDataDocument.SystemMissingValue"/> transparently.
        /// </remarks>
        internal new DateTime? Value
        {
            get
            {
                double v;
                SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetValueNumericImpl(FileHandle, Handle, out v), "spssGetValueNumeric");
                if (v == SpssDataDocument.SystemMissingValue)
                {
                    return null;
                }

                return ConvertDoubleToDateTime(v);
            }

            set
            {
                double v;
                if (value.HasValue)
                {
                    v = ConvertDateTimeToDouble(value.Value);
                }
                else
                {
                    v = SpssDataDocument.SystemMissingValue;
                }
                SpssSafeWrapper.spssSetValueNumeric(FileHandle, Handle, v);
            }
        }

        public override SpssVariable Clone()
        {
            SpssDateVariable other = new SpssDateVariable();
            CloneTo(other);
            return other;
        }

        protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType)
        {
            return IsDateVariable(formatType);
        }

        protected override void Update()
        {
            base.Update();

            double[] missingValues = new double[3];
            this.MissingValues.Select(v => ConvertDateTimeToDouble(v)).Take(missingValues.Length).ToArray().CopyTo(missingValues, 0);
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarNMissingValues(
                this.FileHandle,
                this.Name,
                this.MissingValueFormat,
                missingValues[0],
                missingValues[1],
                missingValues[2]), "spssSetVarNMissingValues");

            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarPrintFormat(FileHandle, Name, this.PrintFormat, 4, this.PrintWidth), "spssSetVarPrintFormat");
            SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarWriteFormat(FileHandle, Name, this.WriteFormat, 4, this.WriteWidth), "spssSetVarWriteFormat");
        }

        protected internal static bool IsDateVariable(FormatTypeCode writeType)
        {
            return writeType == FormatTypeCode.SPSS_FMT_ADATE ||
                writeType == FormatTypeCode.SPSS_FMT_DATE ||
                writeType == FormatTypeCode.SPSS_FMT_DATE_TIME ||
                writeType == FormatTypeCode.SPSS_FMT_DTIME ||
                writeType == FormatTypeCode.SPSS_FMT_EDATE ||
                writeType == FormatTypeCode.SPSS_FMT_JDATE ||
                writeType == FormatTypeCode.SPSS_FMT_MOYR ||
                writeType == FormatTypeCode.SPSS_FMT_QYR ||
                writeType == FormatTypeCode.SPSS_FMT_SDATE ||
                writeType == FormatTypeCode.SPSS_FMT_TIME ||
                writeType == FormatTypeCode.SPSS_FMT_WKYR;
        }

        private static double ConvertDateTimeToDouble(DateTime value)
        {
            double d, t = 0;
            SpssSafeWrapper.spssConvertDate(value.Day, value.Month, value.Year, out d);
            double seconds = (double)(value.Second) + (value.Millisecond / 1000.0);
            SpssSafeWrapper.spssConvertTimeImpl(0, value.Hour, value.Minute, seconds, out t);

            double total = d + t;
            return total;
        }

        private static DateTime ConvertDoubleToDateTime(double v)
        {
            int sD, sM, sY, sd, sh, sm, ss, sms;
            double smsDbl;
            SpssSafeWrapper.spssConvertSPSSDateImpl(out sD, out sM, out sY, v);
            SpssSafeWrapper.spssConvertSPSSTimeImpl(out sd, out sh, out sm, out smsDbl, v);
            ss = (int)smsDbl;
            sms = (int)((smsDbl % 1.0) * 1000);
            return new DateTime(sY, sM, sD, sh, sm, ss, sms);
        }
    }
}