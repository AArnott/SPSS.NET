using System;
using System.Diagnostics;
using System.Collections;

namespace Spss
{
	/// <summary>
	/// A collection of value labels for a <see cref="SpssNumericVariable"/>.
	/// </summary>
	public class SpssNumericVariableValueLabelsDictionary : SpssVariableValueLabelsDictionary<double>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpssNumericVariableValueLabelsDictionary"/> class.
		/// </summary>
		/// <param name="variable">The variable containing this collection.</param>
		public SpssNumericVariableValueLabelsDictionary(SpssVariable variable) : base(variable, null)
		{
		}

		#region Operations

		/// <summary>
		/// Updates the SPSS data file with changes made to the collection.
		/// </summary>
		protected internal override void Update() {
			foreach (var pair in this) {
				SpssSafeWrapper.spssSetVarNValueLabel(FileHandle, Variable.Name, pair.Key, pair.Value);
			}
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
