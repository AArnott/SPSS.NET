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
		public SpssDateVariable()
		{
		}
		/// <summary>
		/// Creates an instance of the <see cref="SpssDateVariable"/> class, 
		/// for use in loading variables from an existing SPSS data file.
		/// </summary>
		/// <param name="variables">
		/// The containing collection.
		/// </param>
		/// <param name="varName">
		/// The name of the variable.
		/// </param>
		protected internal SpssDateVariable(SpssVariablesCollection variables, string varName)
			: base( variables, varName )
		{
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

		#region Operations
		/// <summary>
		/// Updates the changed attributes of the variable within SPSS.
		/// </summary>
		protected override void Update()
		{
			base.Update ();

			if( !IsInCollection ) return; // we'll get to do this later

			SpssSafeWrapper.spssSetVarPrintFormat(FileHandle, Name, FormatTypeCode.SPSS_FMT_DATE_TIME, 4, 28);
			SpssSafeWrapper.spssSetVarWriteFormat(FileHandle, Name, FormatTypeCode.SPSS_FMT_DATE_TIME, 4, 28);
		}

		public override SpssVariable Clone()
		{
			SpssDateVariable other = new SpssDateVariable();
			CloneTo(other);
			return other;
		}

		#endregion
	}
}
