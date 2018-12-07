// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// A collection of value labels for a <see cref="SpssNumericVariable"/>.
    /// </summary>
    public class SpssNumericVariableValueLabelsDictionary : SpssVariableValueLabelsDictionary<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpssNumericVariableValueLabelsDictionary"/> class.
        /// </summary>
        /// <param name="variable">The variable containing this collection.</param>
        public SpssNumericVariableValueLabelsDictionary(SpssVariable variable)
            : base(variable, null)
        {
        }

        /// <summary>
        /// Updates the SPSS data file with changes made to the collection.
        /// </summary>
        protected internal override void Update()
        {
            foreach (var pair in this)
            {
                SpssSafeWrapper.spssSetVarNValueLabel(this.FileHandle, this.Variable.Name, pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Initializes the value labels dictionary from the SPSS data file.
        /// </summary>
        protected override void LoadFromSpssFile()
        {
            double[] values;
            string[] labels;
            ReturnCode result = SpssException.ThrowOnFailure(SpssSafeWrapper.spssGetVarNValueLabels(this.FileHandle, this.Variable.Name, out values, out labels), "spssGetVarNValueLabels", ReturnCode.SPSS_NO_LABELS);
            if (result == ReturnCode.SPSS_OK)
            {
                Debug.Assert(values.Length == labels.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    this.Add(values[i], labels[i]);
                }
            }

            // SPSS_NO_LABELs is nothing special -- just no labels to add
        }
    }
}
