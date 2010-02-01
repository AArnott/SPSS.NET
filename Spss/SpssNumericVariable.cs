namespace Spss {
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;

	/// <summary>
	/// Represents an SPSS data variable that stores numeric information.
	/// </summary>
	/// <remarks>
	/// Both integer and floating point numbers are handled through this
	/// class.
	/// </remarks>
	public class SpssNumericVariable : SpssVariable {
		private const int DecimalPlacesDefault = 0;

		private int decimalPlaces = -1;

		private readonly SpssNumericVariableValueLabelsDictionary valueLabels;

		/// <summary>
		/// Creates an instance of the <see cref="SpssNumericVariable"/> class,
		/// for use when defining a new variable.
		/// </summary>
		public SpssNumericVariable() {
			this.valueLabels = new SpssNumericVariableValueLabelsDictionary(this);
			this.WriteFormat = this.PrintFormat = FormatTypeCode.SPSS_FMT_F;
			this.WriteDecimal = this.PrintDecimal = DecimalPlacesDefault;
			this.WriteWidth = this.PrintWidth = ColumnWidthDefault;
		}

		/// <summary>
		/// Creates an instance of the <see cref="SpssNumericVariable"/> class,
		/// for use in loading variables from an existing SPSS data file.
		/// </summary>
		/// <param name="variables">The containing collection.</param>
		/// <param name="varName">The name of the variable.</param>
		/// <param name="writeFormat">The write format.  The default is <see cref="FormatTypeCode.SPSS_FMT_F"/></param>
		/// <param name="writeDecimal">The write decimal.</param>
		/// <param name="writeWidth">Width of the write.</param>
		/// <param name="printFormat">The print format.  The default is <see cref="FormatTypeCode.SPSS_FMT_F"/></param>
		/// <param name="printDecimal">The print decimal.</param>
		/// <param name="printWidth">Width of the print.</param>
		protected internal SpssNumericVariable(SpssVariablesCollection variables, string varName, FormatTypeCode writeFormat, int writeDecimal, int writeWidth, FormatTypeCode printFormat, int printDecimal, int printWidth)
			: base(variables, varName, writeFormat, writeDecimal, writeWidth, printFormat, printDecimal, printWidth) {
			this.valueLabels = new SpssNumericVariableValueLabelsDictionary(this);
		}

		/// <summary>
		/// Gets the SPSS type for the variable.
		/// </summary>
		public override int SpssType {
			get {
				return 0; // 0 = numeric to SPSS
			}
		}

		/// <summary>
		/// The number of decimal places to reserve for the variable.
		/// </summary>
		public int DecimalPlaces {
			get {
				// If this variable was read from an existing file, and 
				// decimal places has not yet been retrieved, get it.
				if (decimalPlaces < 0 && IsCommitted) {
					FormatTypeCode code;
					int width;
					// just throw away code and width
					SpssSafeWrapper.spssGetVarWriteFormat(FileHandle, Name, out code, out decimalPlaces, out width);
				}
				return decimalPlaces >= 0 ? decimalPlaces : DecimalPlacesDefault;
			}

			set {
				decimalPlaces = value;
				Update();
			}
		}

		/// <summary>
		/// The set of value labels (response values and labels) that are defined.
		/// </summary>
		public IDictionary<double, string> ValueLabels {
			get { return this.valueLabels; }
		}

		/// <summary>
		/// Gets or sets the data value of this variable within a specific case.
		/// </summary>
		/// <remarks>
		/// Null values are translated to and from 
		/// <see cref="SpssDataDocument.SystemMissingValue"/> transparently.
		/// </remarks>
		internal new double? Value {
			get {
				double v;
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetValueNumeric(FileHandle, Handle, out v), "SpssSafeWrapper");
				if (v == SpssDataDocument.SystemMissingValue)
					return null;
				return v;
			}

			set {
				if (!value.HasValue) value = SpssDataDocument.SystemMissingValue;
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetValueNumeric(FileHandle, Handle, value.Value), "SpssSafeWrapper");
			}
		}

		/// <summary>
		/// Updates details of the variable.
		/// </summary>
		protected override void Update() {
			base.Update();

			if (!IsInCollection) {
				return; // we'll get to do this later
			}

			this.valueLabels.Update();
		}

		public override SpssVariable Clone() {
			SpssNumericVariable other = new SpssNumericVariable();
			CloneTo(other);
			return other;
		}

		protected override void CloneTo(SpssVariable spssVar) {
			base.CloneTo(spssVar);
			SpssNumericVariable other = spssVar as SpssNumericVariable;
			if (other == null)
				throw new ArgumentException("Must be of type " + GetType().Name + ".", "other");
			other.DecimalPlaces = DecimalPlaces;
			this.valueLabels.CopyTo(other.valueLabels);
		}

		protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType) {
			return formatType != FormatTypeCode.SPSS_FMT_A &&
				!SpssDateVariable.IsDateVariable(formatType);
		}
	}
}
