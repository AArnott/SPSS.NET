using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Spss {
	/// <summary>
	/// Summary description for VarMetaData.
	/// </summary>
	public class VarMetaDataBase {
		/// <summary>
		/// Creates an instance of the <see cref="VarMetaData"/> class.
		/// </summary>
		/// <param name="varName">
		/// The name of the variable being represented.
		/// </param>
		/// <param name="varHasValueLabels">
		/// Whether the variable has value labels defined.
		/// </param>
		public VarMetaDataBase(string varName, bool varHasValueLabels) {
			if (String.IsNullOrEmpty(varName)) {
				throw new ArgumentNullException("varName");
			}

			this.Name = varName;
			this.HasValueLabels = varHasValueLabels;
			this.ValueLabels = new NameValueCollection();
		}

		/// <summary>
		/// Gets the variable name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the variable has value labels defined.
		/// </summary>
		public bool HasValueLabels { get; private set; }

		/// <summary>
		/// Gets/sets the variable label.
		/// </summary>
		public virtual string Label { get; set; }

		/// <summary>
		/// Gets/sets the value labels for this variable.
		/// </summary>
		public string this[int responseValue] {
			get {
				return this.ValueLabels.Get(Convert.ToString(responseValue, CultureInfo.InvariantCulture));
			}
			set {
				if (!this.HasValueLabels)
					throw new InvalidOperationException("This variable does not have value labels to set.");

				this.ValueLabels.Set(Convert.ToString(responseValue, CultureInfo.InvariantCulture), value);
			}
		}

		/// <summary>
		/// The value labels on the variable.
		/// </summary>
		public NameValueCollection ValueLabels { get; private set; }
	}
}
