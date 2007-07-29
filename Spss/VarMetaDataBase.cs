using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Spss
{
	/// <summary>
	/// Summary description for VarMetaData.
	/// </summary>
	public class VarMetaDataBase
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="VarMetaData"/> class.
		/// </summary>
		/// <param name="varName">
		/// The name of the variable being represented.
		/// </param>
		/// <param name="varHasValueLabels">
		/// Whether the variable has value labels defined.
		/// </param>
		public VarMetaDataBase( string varName, bool varHasValueLabels )
		{
			if( varName == null || varName.Length == 0 ) throw new ArgumentNullException("varName");

			name = varName;
			hasValueLabels = varHasValueLabels;
		}
		#endregion
		
		#region Attributes
		private string name;
		/// <summary>
		/// Gets the variable name.
		/// </summary>
		public virtual string Name
		{
			get
			{
				return name;
			}
		}
		private bool hasValueLabels;
		/// <summary>
		/// Gets whether the variable has value labels defined.
		/// </summary>
		public bool HasValueLabels
		{
			get
			{
				return hasValueLabels;
			}
		}
		private string label;
		/// <summary>
		/// Gets/sets the variable label.
		/// </summary>
		public virtual string Label
		{
			get
			{
				return label;
			}
			set
			{
				label = value;
			}
		}

		/// <summary>
		/// Gets/sets the value labels for this variable.
		/// </summary>
		public string this [int responseValue]
		{
			get
			{
				return valueLabels.Get( Convert.ToString( responseValue, CultureInfo.InvariantCulture ) );
			}
			set
			{
				if( !hasValueLabels ) 
					throw new InvalidOperationException("This variable does not have value labels to set.");

				valueLabels.Set(Convert.ToString( responseValue, CultureInfo.InvariantCulture ), value);
			}
		}
		private NameValueCollection valueLabels = new NameValueCollection();
		/// <summary>
		/// The value labels on the variable.
		/// </summary>
		protected NameValueCollection ValueLabels
		{
			get
			{
				return valueLabels;
			}
		}

		#endregion
	}
}
