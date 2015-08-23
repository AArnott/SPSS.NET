namespace Spss {
	using System;
	using System.Diagnostics;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Represents an SPSS data variable that stores numeric information.
	/// </summary>
	/// <remarks>
	/// Both integer and floating point numbers are handled through this
	/// class.
	/// </remarks>
	public class SpssNumericVariable : SpssVariable {
		private const int DecimalPlacesDefault = 0;
		private readonly SpssNumericVariableValueLabelsDictionary valueLabels;
		private FormatTypeCode writeFormat;
		private FormatTypeCode printFormat;
		private int writeWidth;
		private int printWidth;
		private int writeDecimal;
		private int printDecimal;

		/// <summary>
		/// Creates an instance of the <see cref="SpssNumericVariable"/> class,
		/// for use when defining a new variable.
		/// </summary>
		public SpssNumericVariable() {
			this.valueLabels = new SpssNumericVariableValueLabelsDictionary(this);
			this.WriteFormat = this.PrintFormat = FormatTypeCode.SPSS_FMT_F;
			this.WriteDecimal = this.PrintDecimal = DecimalPlacesDefault;
			this.WriteWidth = this.PrintWidth = ColumnWidthDefault;
			this.MissingValues = new List<double>(3);
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
			: base(variables, varName) {
			this.valueLabels = new SpssNumericVariableValueLabelsDictionary(this);

			MissingValueFormatCode formatCode;
			double[] missingValues = new double[3];
			ReturnCode result = SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarNMissingValues(this.FileHandle, this.Name, out formatCode, out missingValues[0], out missingValues[1], out missingValues[2]), "spssGetVarNMissingValues");
			this.MissingValueFormat = formatCode;
			this.MissingValues = new List<double>(missingValues.Take(Math.Abs((int)formatCode)));
			this.writeDecimal = writeDecimal;
			this.writeWidth = writeWidth;
			this.writeFormat = writeFormat;
			this.printDecimal = printDecimal;
			this.printWidth = printWidth;
			this.printFormat = printFormat;
		}

		/// <summary>
		/// Gets or sets the missing values for this variable.
		/// </summary>
		/// <value>The missing values.</value>
		/// <remarks>
		/// A maximum of three maximum values may be supplied.
		/// </remarks>
		public IList<double> MissingValues { get; private set; }

		public MissingValueFormatCode MissingValueFormat { get; set; }

		/// <summary>
		/// Gets the SPSS type for the variable.
		/// </summary>
		public override int SpssType {
			get {
				return 0; // 0 = numeric to SPSS
			}
		}

		public virtual FormatTypeCode WriteFormat {
			get {
				return this.writeFormat;
			}

			set {
				if (!this.IsApplicableFormatTypeCode(value)) {
					throw new ArgumentOutOfRangeException("value", "This value does not apply to this type of SPSS variable.");
				}

				this.writeFormat = value;
			}
		}

		public virtual FormatTypeCode PrintFormat {
			get {
				return this.printFormat;
			}

			set {
				if (!this.IsApplicableFormatTypeCode(value)) {
					throw new ArgumentOutOfRangeException("value", "This value does not apply to this type of SPSS variable.");
				}

				this.printFormat = value;
			}
		}

		public int WriteWidth {
			get {
				return this.writeWidth;
			}

			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.writeWidth = value;
			}
		}

		public int PrintWidth {
			get {
				return this.printWidth;
			}

			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.printWidth = value;
			}
		}

		public int WriteDecimal {
			get {
				return this.writeDecimal;
			}

			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.writeDecimal = value;
			}
		}

		public int PrintDecimal {
			get {
				return this.printDecimal;
			}

			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException("value");
				}

				this.printDecimal = value;
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
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetValueNumericImpl(FileHandle, Handle, out v), "spssGetValueNumeric");
				if (v == SpssDataDocument.SystemMissingValue)
					return null;
				return v;
			}

			set {
				if (!value.HasValue) value = SpssDataDocument.SystemMissingValue;
				SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetValueNumeric(FileHandle, Handle, value.Value), "spssSetValueNumeric");
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
			double[] missingValues = new double[3];
			this.MissingValues.Take(missingValues.Length).ToArray().CopyTo(missingValues, 0);
			SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarNMissingValues(
				this.FileHandle,
				this.Name,
				this.MissingValueFormat,
				missingValues[0],
				missingValues[1],
				missingValues[2]), "spssSetVarNMissingValues");

			SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarPrintFormat(FileHandle, Name, this.PrintFormat, this.PrintDecimal, this.PrintWidth), "spssSetVarPrintFormat");
			SpssException.ThrowOnFailure(SpssSafeWrapper.spssSetVarWriteFormat(FileHandle, Name, this.WriteFormat, this.WriteDecimal, this.WriteWidth), "spssSetVarWriteFormat");
		}

		public override SpssVariable Clone() {
			SpssNumericVariable other = new SpssNumericVariable();
			CloneTo(other);
			return other;
		}

		protected override void CloneTo(SpssVariable spssVar) {
			base.CloneTo(spssVar);
			SpssNumericVariable other = spssVar as SpssNumericVariable;
			if (other == null) {
				throw new ArgumentException("Must be of type " + GetType().Name + ".", "other");
			}
			other.PrintDecimal = this.PrintDecimal;
			other.PrintFormat = this.PrintFormat;
			other.PrintWidth = this.PrintWidth;
			other.WriteDecimal = this.WriteDecimal;
			other.WriteFormat = this.WriteFormat;
			other.WriteWidth = this.WriteWidth;
			other.MissingValueFormat = this.MissingValueFormat;
			other.MissingValues = new List<double>(this.MissingValues);
			this.valueLabels.CopyTo(other.valueLabels);
		}

		protected override bool IsApplicableFormatTypeCode(FormatTypeCode formatType) {
			return formatType != FormatTypeCode.SPSS_FMT_A &&
				!SpssDateVariable.IsDateVariable(formatType);
		}
	}
}
