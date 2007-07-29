using System;
using System.Diagnostics;
using System.Collections;

namespace Spss
{
	/// <summary>
	/// A collection of value labels for a <see cref="SpssNumericVariable"/>.
	/// </summary>
	public class SpssNumericVariableValueLabelsCollection : SpssVariableValueLabelsCollection
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssNumericVariableValueLabelsCollection"/> class.
		/// </summary>
		public SpssNumericVariableValueLabelsCollection(SpssVariable variable) : base(variable)
		{
		}
		#endregion

		#region Attributes
		/// <summary>
		/// Gets/sets the response label for some response value.
		/// </summary>
		public string this [double Value]
		{
			get
			{
				return base[Value];
			}
			set
			{
				base[Value] = value;
			}
		}
		#endregion

		#region Operations
		/// <summary>
		/// Adds a value label.
		/// </summary>
		/// <param name="value">
		/// The response value to associate with the new response label.
		/// </param>
		/// <param name="label">
		/// The new response label.
		/// </param>
		public void Add(double value, string label)
		{
			base.Add(value, label);
		}
		/// <summary>
		/// Removes a value label.
		/// </summary>
		/// <param name="value">
		/// The response value to remove.
		/// </param>
		public void Remove(double value)
		{
			base.Remove(value);
		}
		/// <summary>
		/// Updates the SPSS data file with changes made to the collection.
		/// </summary>
		protected internal override void Update()
		{
			foreach( DictionaryEntry de in this )
				SpssSafeWrapper.spssSetVarNValueLabel(FileHandle, Variable.Name, (double) de.Key, (string) de.Value);
		}
		/// <summary>
		/// Initializes the value labels dictionary from the SPSS data file.
		/// </summary>
		protected override void LoadFromSpssFile()
		{
			double[] values;
			string[] labels;
			ReturnCode result = SpssSafeWrapper.spssGetVarNValueLabels(FileHandle, Variable.Name, out values, out labels);
			switch( result )
			{
				case ReturnCode.SPSS_OK:
					Debug.Assert( values.Length == labels.Length );
					for( int i = 0; i < values.Length; i++ )
						Add(values[i], labels[i]);
					break;
				case ReturnCode.SPSS_NO_LABELS:
					break; // nothing special -- just no labels to add
				default:
					throw new SpssException(result, "spssGetVarNValueLabels");
			}
		}

		#endregion
	}
}
