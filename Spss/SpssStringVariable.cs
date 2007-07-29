using System;

namespace Spss
{
	/// <summary>
	/// Represents an SPSS data variable that stores character string information.
	/// </summary>
	public class SpssStringVariable : SpssVariable, ISpssVariableWithValueLabels
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssStringVariable"/> class,
		/// for use when defining a new variable.
		/// </summary>
		public SpssStringVariable()
		{
			valueLabels = new SpssStringVariableValueLabelsCollection(this);
		}
		/// <summary>
		/// Creates an instance of the <see cref="SpssStringVariable"/> class, 
		/// for use in loading variables from an existing SPSS data file.
		/// </summary>
		/// <param name="variables">
		/// The containing collection.
		/// </param>
		/// <param name="varName">
		/// The name of the variable being loaded.
		/// </param>
		/// <param name="length">
		/// The length of the string variable.  This is the same as SpssType
		/// </param>
		protected internal SpssStringVariable(SpssVariablesCollection variables, string varName, int length)
			: base( variables, varName )
		{
			valueLabels = new SpssStringVariableValueLabelsCollection(this);
			this.length = length;
		}
		#endregion

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
		protected override int SpssType
		{
			get
			{
				return Length;
			}
		}
		/// <summary>
		/// Gets/sets the data value of this variable within a specific case.
		/// </summary>
		internal new string Value
		{
			get
			{
				string v;
				ReturnCode result = SpssSafeWrapper.spssGetValueChar( FileHandle, Handle, out v );
				if( result != ReturnCode.SPSS_OK )
					throw new SpssException(result, "spssGetValueChar");
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

		private SpssStringVariableValueLabelsCollection valueLabels;
		/// <summary>
		/// The set of value labels (response values and labels) that are defined.
		/// </summary>
		public SpssStringVariableValueLabelsCollection ValueLabels { get { return valueLabels; } }
		#endregion

		#region Operations
		/// <summary>
		/// Updates the changed attributes of the variable within SPSS.
		/// </summary>
		protected override void Update()
		{
			base.Update ();

			if( !IsInCollection ) return; // we'll get to do this later

			ValueLabels.Update();
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
			ValueLabels.CopyTo(other.ValueLabels);
		}

		#endregion

		#region ISpssVariableWithValueLabels Members

		SpssVariableValueLabelsCollection Spss.ISpssVariableWithValueLabels.ValueLabels
		{
			get
			{
				return this.ValueLabels;
			}
		}

		#endregion
	}
}
