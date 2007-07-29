using System;
using System.Runtime.Serialization;

namespace Spss
{
	/// <summary>
	/// Thrown when any spssio32.dll function returns a status code other than SPSS_OK.
	/// The error number of the exception represents the result code.
	/// </summary>
	[Serializable]
	public class SpssException : Exception
	{
		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssException"/> class,
		/// for deserialization.
		/// </summary>
		public SpssException() {}
		/// <summary>
		/// Creates an instance of the <see cref="SpssException"/> class,
		/// for deserialization.
		/// </summary>
		protected SpssException(SerializationInfo info, StreamingContext context)
			: base( info, context ) {}
		/// <summary>
		/// Creates an instance of the <see cref="SpssException"/> class,
		/// for leaving a custom message.
		/// </summary>
		public SpssException(string message, Exception innerException)
			: base( message, innerException ) {}
		/// <summary>
		/// Creates an instance of the <see cref="SpssException"/> class,
		/// for leaving a custom message.
		/// </summary>
		public SpssException(string message)
			: base( message ) {}
		/// <summary>
		/// Creates an instance of the <see cref="SpssException"/> class.
		/// </summary>
		/// <param name="spssResultCode">
		/// The SPSS error code that caused the exception.
		/// </param>
		/// <param name="spssFunction">
		/// The name of the SPSS function that returned the error code.
		/// </param>
		public SpssException(ReturnCode spssResultCode, string spssFunction) 
			: base("SPSS function " + spssFunction + " returned error code " + spssResultCode)
		{
			this.spssResultCode = spssResultCode;
		}
		#endregion

		#region Attributes
		private ReturnCode spssResultCode;
		/// <summary>
		/// Gets the original <see cref="ReturnCode">SPSS error code</see> returned that caused this exception.
		/// </summary>
		public ReturnCode SpssResultCode 
		{
			get
			{
				return spssResultCode;
			}
		}
		#endregion
	}
}
