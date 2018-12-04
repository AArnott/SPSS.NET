using System;
using System.Diagnostics;
using System.Collections;

namespace Spss
{
    /// <summary>
    /// A collection of value labels for a <see cref="SpssStringVariable"/>.
    /// </summary>
    public class SpssStringVariableValueLabelsDictionary : SpssVariableValueLabelsDictionary<string>
    {
        /// <summary>
        /// Creates an instance of the <see cref="SpssStringVariableValueLabelsDictionary"/> class.
        /// </summary>
        public SpssStringVariableValueLabelsDictionary(SpssVariable variable)
            : base(variable, StringComparer.Ordinal)
        {
        }

        #region Attributes
        /// <summary>
        /// The variable hosting this collection.
        /// </summary>
        protected new SpssStringVariable Variable => (SpssStringVariable)base.Variable;

        /// <summary>
        /// Gets a value indicating whether this string variable can have value labels.
        /// </summary>
        /// <remarks>
        /// SPSS only supports value labels on strings within
        /// <see cref="SpssThinWrapper.SPSS_MAX_SHORTSTRING"/> characters
        /// in length..
        /// </remarks>
        public bool Applies => Variable.Length <= SpssSafeWrapper.SPSS_MAX_SHORTSTRING;
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
        public override void Add(string value, string label)
        {
            if (!Applies) throw new InvalidOperationException("Cannot add value labels to a long string variable.");
            base.Add(value, label);
        }

        /// <summary>
        /// Updates the SPSS data file with changes made to the collection.
        /// </summary>
        protected internal override void Update()
        {
            foreach (var pair in this)
                SpssSafeWrapper.spssSetVarCValueLabel(FileHandle, Variable.Name, pair.Key, pair.Value);
        }

        /// <summary>
        /// Initializes the value labels dictionary from the SPSS data file.
        /// </summary>
        protected override void LoadFromSpssFile()
        {
            if (!Applies) return;

            string[] values;
            string[] labels;
            ReturnCode result = SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarCValueLabels(FileHandle, Variable.Name, out values, out labels), "spssGetVarCValueLabels", ReturnCode.SPSS_NO_LABELS);
            if (result == ReturnCode.SPSS_OK)
            { // ReturnCode.SPSS_NO_LABELS is nothing special -- just no labels to add
                Debug.Assert(values.Length == labels.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    Add(values[i], labels[i]);
                }
            }
        }

        #endregion
    }
}
