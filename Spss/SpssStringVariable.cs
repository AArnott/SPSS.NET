//-----------------------------------------------------------------------
// <copyright file="SpssStringVariable.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Brigham Young University
// </copyright>
//-----------------------------------------------------------------------

namespace Spss
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents an SPSS data variable that stores character string information.
	/// </summary>
	public class SpssStringVariable : SpssVariable
	{
		/// <summary>
		/// Creates an instance of the <see cref="SpssStringVariable"/> class,
		/// for use when defining a new variable.
		/// </summary>
		public SpssStringVariable()
		{
			this.valueLabels = new SpssStringVariableValueLabelsDictionary(this);
		}

		/// <summary>
		/// Creates an instance of the <see cref="SpssStringVariable"/> class,
		/// for use in loading variables from an existing SPSS data file.
		/// </summary>
		/// <param name="variables">The containing collection.</param>
		/// <param name="varName">The name of the variable being loaded.</param>
		/// <param name="length">The length of the string variable.  This is the same as SpssType</param>
		/// <param name="writeFormat">The write format.</param>
		/// <param name="writeDecimal">The write decimal.</param>
		/// <param name="writeWidth">Width of the write.</param>
		/// <param name="printFormat">The print format.</param>
		/// <param name="printDecimal">The print decimal.</param>
		/// <param name="printWidth">Width of the print.</param>
		protected internal SpssStringVariable(SpssVariablesCollection variables, string varName, int length, FormatTypeCode writeFormat, int writeDecimal, int writeWidth, FormatTypeCode printFormat, int printDecimal, int printWidth)
			: base(variables, varName, writeFormat, writeDecimal, writeWidth, printFormat, printDecimal, printWidth) {
			this.valueLabels = new SpssStringVariableValueLabelsDictionary(this);
			this.length = length;
		}

		#region Attributes
		private int length = -1;
		/// <summary>
		/// Gets the maximum length a string in this variable can be.
		/// </summary>
		public int Length
		{
			get
			{
				return length >= 0 ? length : ColumnWidth;
			}
			set
			{
				VerifyNotCommittedVariable();
				if( value < 0 ) throw new ArgumentOutOfRangeException("Length", value, "Must be a non-negative number.");

				length = value;
			}
		}
		/// <summary>
		/// Gets the SPSS type for the variable.
		/// </summary>
		public override int SpssType
		{
			get
			{
				return Length;
			}
		}
		/// <summary>
		/// Gets or sets the data value of this variable within a specific case.
		/// </summary>
		internal new string Value
		{
			get
			{
				string v;
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetValueChar(FileHandle, Handle, out v), "SpssSafeWrapper");
				return v;
			}
			set
			{
				if( value == null ) value = string.Empty;
				if( value.Length > Length )
					throw new ArgumentOutOfRangeException("Value", value, "String too long for variable " + Name + ".  Maximum length is: " + Length);
				SpssSafeWrapper.spssSetValueChar( FileHandle, Handle, value );
			}
		}

		private readonly SpssStringVariableValueLabelsDictionary valueLabels;

		/// <summary>
		/// The set of value labels (response values and labels) that are defined.
		/// </summary>
		public IDictionary<string, string> ValueLabels {
			get { return this.valueLabels; }
		}

		#endregion

		/// <summary>
		/// Updates the changed attributes of the variable within SPSS.
		/// </summary>
		protected override void Update()
		{
			base.Update ();

			if( !IsInCollection ) return; // we'll get to do this later

			this.valueLabels.Update();
		}

		public override SpssVariable Clone()
		{
			SpssStringVariable other = new SpssStringVariable();
			CloneTo(other);
			return other;
		}

		protected override void CloneTo(SpssVariable spssVar)
		{
			base.CloneTo(spssVar);
			SpssStringVariable other = spssVar as SpssStringVariable;
			if (other == null)
				throw new ArgumentException("Must be of type " + GetType().Name + ".", "other");
			other.Length = Length;
			this.valueLabels.CopyTo(other.valueLabels);
		}

		protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType) {
			return formatType == FormatTypeCode.SPSS_FMT_A;
		}
	}
}
