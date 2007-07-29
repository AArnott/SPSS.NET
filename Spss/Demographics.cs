using System;
using System.Collections.Specialized;

namespace MetaData
{
	/// <summary>
	/// A collection of commonly useful demographical attributes.
	/// </summary>
	public static class Demographics
	{
		/// <summary>
		/// Describes gender as being either Male, Female, or Unknown.
		/// </summary>
		[Serializable]
		public enum GenderType
		{
			/// <summary>
			/// Gender is unknown, generic, or inconsequential.
			/// </summary>
			Unknown,
			/// <summary>
			/// Male
			/// </summary>
			Male,
			/// <summary>
			/// Female
			/// </summary>
			Female
		}
		/// <summary>
		/// Converts a character to the appropriate GenderType.
		/// </summary>
		/// <param name="ch">Can be M, m, F, f or some other character.</param>
		/// <returns>
		/// If M or m, GenderType.Male is returned.  
		/// If F or f, GenderType.Female is returned.
		/// Otherwise, GenderType.Unknown is returned.
		/// </returns>
		public static GenderType ConvertCharToGenderType(char ch)
		{
			switch( ch )
			{
				case 'M':
				case 'm':
					return GenderType.Male;
				case 'F':
				case 'f':
					return GenderType.Female;
				default:
					return GenderType.Unknown;
			}
		}
		/// <summary>
		/// Converts a GenderType to a character.
		/// </summary>
		/// <returns>
		/// The characters M or F for male and female, 
		/// or the space character otherwise.
		/// </returns>
		public static char ConvertGenderTypeToChar(GenderType genderType)
		{
			switch( genderType )
			{
				case GenderType.Male:
					return 'M';
				case GenderType.Female:
					return 'F';
				default:
					return ' ';
			}
		}
	}
}
