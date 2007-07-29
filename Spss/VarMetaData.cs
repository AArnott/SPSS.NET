using System;
using System.Collections;

namespace Spss
{
	/// <summary>
	/// A class that assists in transferring meta data from an
	/// import operation into an SPSS data file.
	/// </summary>
	public class VarMetaData : VarMetaDataBase
	{
		internal SpssVariable var;

		internal VarMetaData( SpssVariable var, string origName )
			: base( origName, true /* assume yes */ )
		{
			this.var = var;
		}


		#region Attributes
		/// <summary>
		/// The variable label.
		/// </summary>
		public override string Label
		{
			get
			{
				return base.Label;
			}
			set
			{
				if( value != null && value.Length > SpssSafeWrapper.SPSS_MAX_VARLABEL )
				{
					Console.Error.WriteLine("WARNING: {0} label truncated (SPSS max label length {1})",
						var.Name, SpssSafeWrapper.SPSS_MAX_VARLABEL.ToString());
					value = value.Substring(0, SpssSafeWrapper.SPSS_MAX_VARLABEL-3) + "...";
				}
				base.Label = value;
			}
		}
		#endregion

		#region Operations
		internal void ApplyToSpssVar()
		{
			var.Label = Label == null ? "" : Label;
			foreach( string key in ValueLabels )
				if( var is SpssNumericVariable )
					((SpssNumericVariable)var).ValueLabels.Add(Convert.ToDouble(key), ValueLabels[key]);
				else if( var is SpssStringVariable )
					((SpssStringVariable)var).ValueLabels.Add(key, ValueLabels[key]);
		}
		#endregion
	}
}
