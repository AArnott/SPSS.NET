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
        public SpssException() { }
        /// <summary>
        /// Creates an instance of the <see cref="SpssException"/> class,
        /// for deserialization.
        /// </summary>
        protected SpssException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
        /// <summary>
        /// Creates an instance of the <see cref="SpssException"/> class,
        /// for leaving a custom message.
        /// </summary>
        public SpssException(string message, Exception innerException)
            : base(message, innerException)
        { }
        /// <summary>
        /// Creates an instance of the <see cref="SpssException"/> class,
        /// for leaving a custom message.
        /// </summary>
        public SpssException(string message)
            : base(message)
        { }
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

        /// <summary>
        /// Throws an <see cref="SpssException"/> if a prior call into SPSS failed.
        /// </summary>
        /// <param name="returnCode">The return code actually received from the SPSS function.</param>
        /// <param name="spssFunctionName">Name of the SPSS function invoked.</param>
        /// <returns>The value of <paramref name="returnCode"/>.</returns>
        /// <remarks>
        /// This method overload is here to avoid the CLR (or C# compiler) automatically instantiating
        /// an empty params array to callers who don't pass extra returnCode.
        /// </remarks>
        internal static ReturnCode ThrowOnFailure(ReturnCode returnCode, string spssFunctionName)
        {
            return ThrowOnFailure(returnCode, spssFunctionName, null);
        }

        /// <summary>
        /// Throws an <see cref="SpssException"/> if a prior call into SPSS failed.
        /// </summary>
        /// <param name="returnCode">The return code actually received from the SPSS function.</param>
        /// <param name="spssFunctionName">Name of the SPSS function invoked.</param>
        /// <param name="acceptableReturnCodes">The acceptable return codes that should not result in a thrown exception (SPSS_OK is always ok).</param>
        /// <returns>The value of <paramref name="returnCode"/>.</returns>
        internal static ReturnCode ThrowOnFailure(ReturnCode returnCode, string spssFunctionName, params ReturnCode[] acceptableReturnCodes)
        {
            if (returnCode == ReturnCode.SPSS_OK)
            {
                return returnCode;
            }

            if (acceptableReturnCodes != null)
            {
                if (Array.IndexOf(acceptableReturnCodes, returnCode) >= 0)
                {
                    return returnCode;
                }
            }

            throw new SpssException(returnCode, spssFunctionName);
        }

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
