using System;

namespace Spss
{
	/// <summary>
	/// Represents an SPSS data variable that stores date information.
	/// </summary>
	public class SpssDateVariable : SpssVariable
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssDateVariable"/> class,
		/// for use when defining a new variable.
		/// </summary>
		public SpssDateVariable() {
			this.WriteFormat = this.PrintFormat = FormatTypeCode.SPSS_FMT_DATE_TIME;
			this.WriteDecimal = this.PrintDecimal = 4;
			this.WriteWidth = this.PrintWidth = 28;
		}

		/// <summary>
		/// Creates an instance of the <see cref="SpssDateVariable"/> class,
		/// for use in loading variables from an existing SPSS data file.
		/// </summary>
		/// <param name="variables">The containing collection.</param>
		/// <param name="varName">The name of the variable.</param>
		/// <param name="writeFormat">The write format.</param>
		/// <param name="writeDecimal">The write decimal.</param>
		/// <param name="writeWidth">Width of the write.</param>
		/// <param name="printFormat">The print format.</param>
		/// <param name="printDecimal">The print decimal.</param>
		/// <param name="printWidth">Width of the print.</param>
		protected internal SpssDateVariable(SpssVariablesCollection variables, string varName, FormatTypeCode writeFormat, int writeDecimal, int writeWidth, FormatTypeCode printFormat, int printDecimal, int printWidth)
			: base(variables, varName, writeFormat, writeDecimal, writeWidth, printFormat, printDecimal, printWidth) {
		}
		#endregion

		#region Attributes
		/// <summary>
		/// Gets the SPSS type for the variable.
		/// </summary>
		public override int SpssType
		{
			get
			{
				return 0; // date variables are numerics
			}
		}
		/// <summary>
		/// Gets/sets the data value of this variable within a specific case.
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
				ReturnCode result = SpssSafeWrapper.spssGetValueNumeric( FileHandle, Handle, out v );
				if( result != ReturnCode.SPSS_OK )
					throw new SpssException(result, "spssGetValueNumeric");

				if( v == SpssDataDocument.SystemMissingValue ) return null;

				int sD, sM, sY, sd, sh, sm, ss, sms;
				double smsDbl;
				SpssSafeWrapper.spssConvertSPSSDate(out sD, out sM, out sY, v);
				SpssSafeWrapper.spssConvertSPSSTime(out sd, out sh, out sm, out smsDbl, v);
				ss = (int)smsDbl;
				sms = (int)((smsDbl % 1.0) * 1000);
				return new DateTime(sY, sM, sD, sh, sm, ss, sms);
			}
			set
			{
				double d, t = 0;
				if( !value.HasValue )
					d = SpssDataDocument.SystemMissingValue;
				else
				{
					SpssSafeWrapper.spssConvertDate(value.Value.Day, value.Value.Month, value.Value.Year, out d);
					double seconds = (double)(value.Value.Second) + (value.Value.Millisecond / 1000.0);
					SpssSafeWrapper.spssConvertTime(0, value.Value.Hour, value.Value.Minute, seconds, out t);
				}

				SpssSafeWrapper.spssSetValueNumeric(FileHandle, Handle, d + t);
			}
		}
		#endregion

		/// <summary>
		/// Updates the changed attributes of the variable within SPSS.
		/// </summary>
		protected override void Update()
		{
			base.Update ();

			if( !IsInCollection ) return; // we'll get to do this later
		}

		public override SpssVariable Clone()
		{
			SpssDateVariable other = new SpssDateVariable();
			CloneTo(other);
			return other;
		}

		protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType) {
			return IsDateVariable(formatType);
		}

		protected internal static bool IsDateVariable(FormatTypeCode writeType) {
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
	}
}
