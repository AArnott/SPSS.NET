using System;
using System.Diagnostics;
using System.Collections;

namespace Spss
{
	/// <summary>
	/// A collection of value labels for a <see cref="SpssStringVariable"/>.
	/// </summary>
	public class SpssStringVariableValueLabelsCollection : SpssVariableValueLabelsCollection
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssStringVariableValueLabelsCollection"/> class.
		/// </summary>
		public SpssStringVariableValueLabelsCollection(SpssVariable variable) : base(variable)
		{
		}
		#endregion

		#region Attributes
		/// <summary>
		/// The variable hosting this collection.
		/// </summary>
		protected new SpssStringVariable Variable { get { return (SpssStringVariable) base.Variable; } }
		/// <summary>
		/// Gets/sets the response label for some response value.
		/// </summary>
		public string this [string Value]
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
		/// <summary>
		/// Gets whether this string variable can have value labels.
		/// </summary>
		/// <remarks>
		/// SPSS only supports value labels on strings within
		/// <see cref="SpssThinWrapper.SPSS_MAX_SHORTSTRING"/> characters
		/// in length..
		/// </remarks>
		public bool Applies
		{
			get
			{
				return Variable.Length <= SpssSafeWrapper.SPSS_MAX_SHORTSTRING;
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
		public void Add(string value, string label)
		{
			if( !Applies ) throw new InvalidOperationException("Cannot add value labels to a long string variable.");
			base.Add(value, label);
		}
		/// <summary>
		/// Removes a value label.
		/// </summary>
		/// <param name="value">
		/// The response value to remove.
		/// </param>
		public void Remove(string value)
		{
			base.Remove(value);
		}
		/// <summary>
		/// Updates the SPSS data file with changes made to the collection.
		/// </summary>
		protected internal override void Update()
		{
			foreach( DictionaryEntry de in this )
				SpssSafeWrapper.spssSetVarCValueLabel(FileHandle, Variable.Name, (string)de.Key, (string)de.Value);
		}
		/// <summary>
		/// Initializes the value labels dictionary from the SPSS data file.
		/// </summary>
		protected override void LoadFromSpssFile()
		{
			if( !Applies ) return; 

			string[] values;
			string[] labels;
			ReturnCode result = SpssSafeWrapper.spssGetVarCValueLabels(FileHandle, Variable.Name, out values, out labels);
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
					throw new SpssException(result, "spssGetVarCValueLabels");
			}
		}
		#endregion
	}
}
