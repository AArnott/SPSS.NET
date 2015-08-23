using System;
using System.Runtime.InteropServices;

namespace Spss
{

	/// <summary>
	/// Very thin SpssSafeWrapper class for functions exposed by spssio32.dll.
	/// </summary>
	[CLSCompliant(false)]
	public class SpssSafeWrapper : SpssThinWrapper
	{
		/// <summary>
		/// Creates an instance of the <see cref="SpssSafeWrapper"/> class.
		/// </summary>
		protected SpssSafeWrapper()
		{
		}

		// These lengths were extracted from the SPSS documentation 
		// rather than any header file.
		public const int SPSS_GUID_LENGTH = 256;
		public const int SPSS_SYSTEM_STRING_LENGTH = 41;
		public const int SPSS_MAX_TEXTINFO = 255;
		public const int SPSS_DATESTAMP_LENGTH = 9;
		public const int SPSS_TIMESTAMP_LENGTH = 8;

		#region veg

		public static Int32 SPSS_STRING(Int32 size)
		{
			return size;
		}

		public const Int32 SPSS_NUMERIC = 0;

		#endregion veg

		/// <summary>
		/// Opens an SPSS file for reading.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file.
		/// </param>
		/// <param name="handle">
		/// The handle to the opened file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FITAB_FULL"/>,
		/// <see cref="ReturnCode.SPSS_FILE_OERROR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>,
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>,
		/// <see cref="ReturnCode.SPSS_NO_TYPE2"/>, or
		/// <see cref="ReturnCode.SPSS_NO_TYPE999"/>.
		/// </returns>
		/// <remarks>
		/// This function opens an SPSS data file for reading and returns a handle that should be
		/// used for subsequent operations on the file.
		/// Files opened with spssOpenRead should be closed with <see cref="SpssThinWrapper.spssCloseReadDelegate"/>.
		/// </remarks>
		public static ReturnCode spssOpenRead(string fileName, out int handle)
		{
			return SpssThinWrapper.spssOpenReadImpl(ref fileName, out handle);
		}
		/// <summary>
		/// Creates an empty SPSS file.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file.
		/// </param>
		/// <param name="handle">
		/// The handle to the new file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FITAB_FULL"/>,
		/// <see cref="ReturnCode.SPSS_FILE_OERROR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>,
		/// </returns>
		/// <remarks>
		/// This function opens a file in preparation for creating a new SPSS data file and returns a
		/// handle that should be used for subsequent operations on the file.
		/// Files opened with spssOpenWrite should be closed with <see cref="SpssThinWrapper.spssCloseWriteDelegate"/>.
		/// </remarks>
		public static ReturnCode spssOpenWrite(string fileName, out int handle)
		{
			return SpssThinWrapper.spssOpenWriteImpl(ref fileName, out handle);
		}
		/// <summary>
		/// Reports the name of the case weight variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// The name of the case weight variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_CASEWGT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_CASEWGT"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the name of the case weight variable. 
		/// </remarks>
		public static ReturnCode spssGetCaseWeightVar(int handle, out string varName)
		{
			varName = new string(' ', SPSS_MAX_VARNAME + 1);
			ReturnCode result = SpssThinWrapper.spssGetCaseWeightVarImpl(handle, ref varName);
			varName = (result == ReturnCode.SPSS_OK) ? varName.Substring(0, varName.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Retrieves the first block of SPSS Data Entry information from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="data">
		/// Data from the file.
		/// </param>
		/// <param name="maxData">
		/// Maximum bytes to return.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_DEW"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_BADTEMP"/>.
		/// </returns>
		/// <remarks>
		/// The client can retrieve DEW information (file information that is private to the SPSS
		/// Data Entry product) from a file in whatever increments are convenient. The first such
		/// increment is retrieved by calling spssGetDEWFirst, and subsequent segments are
		/// retrieved by calling spssGetDEWNext as many times as necessary. As with
		/// spssGetDEWInfo, spssGetDEWFirst will return SPSS_NO_DEW if the file was written
		/// with a byte order that is the reverse of that of the host.
		/// </remarks>
		public static ReturnCode spssGetDEWFirst(int handle, out string data, int maxData)
		{
			data = new string(' ', maxData);
			int len;
			ReturnCode result = SpssThinWrapper.spssGetDEWFirstImpl(handle, ref data, data.Length, out len);
			data = (result == ReturnCode.SPSS_OK) ? data.Substring(0, len) : null;
			return result;
		}
		/// <summary>
		/// Gets the Data Entry GUID from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="asciiGUID">
		/// The file's GUID in character form or a null string if the
		/// file contains no GUID.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// Data Entry for Windows maintains a GUID in character form as a uniqueness indicator.
		/// Two files have identical dictionaries and DEW information if they have the same
		/// GUID. Note that the spssOpenWriteCopy function will not copy the source file’s
		/// GUID. spssGetDEWGUID allows the client to read a file’s GUID, if any. 
		/// </remarks>
		public static ReturnCode spssGetDEWGUID(int handle, out string asciiGUID)
		{
			asciiGUID = new string(' ', SPSS_GUID_LENGTH + 1);
			ReturnCode result = SpssThinWrapper.spssGetDEWGUIDImpl(handle, ref asciiGUID);
			// Although we search for a null terminator, the documentation
			// tells us that the GUID is always SPSS_GUID_LENGTH, so we COULD
			// just terminate the string there to trim off the null terminator that is
			// artificially put there.
			asciiGUID = (result == ReturnCode.SPSS_OK) ? asciiGUID.Substring(0, asciiGUID.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Retrieves the next block of SPSS Data Entry information from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="data">
		/// Data from the file.
		/// </param>
		/// <param name="maxData">
		/// Maximum bytes to return.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_DEW_NOFIRST"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_BADTEMP"/>.
		/// </returns>
		/// <remarks>
		/// The client can retrieve DEW information (file information that is private to the SPSS
		/// Data Entry product) from a file in whatever increments are convenient. The first such
		/// increment is retrieved by calling <see cref="spssGetDEWFirst"/>, and subsequent segments are
		/// retrieved by calling spssGetDEWNext as many times as necessary. As with
		/// <see cref="SpssThinWrapper.spssGetDEWInfoDelegate"/>, <see cref="spssGetDEWFirst"/> will return SPSS_NO_DEW if the file was written
		/// with a byte order that is the reverse of that of the host.
		/// </remarks>
		public static ReturnCode spssGetDEWNext(int handle, out string data, int maxData)
		{
			data = new string(' ', maxData);
			int len;
			ReturnCode result = SpssThinWrapper.spssGetDEWNextImpl(handle, ref data, data.Length, out len);
			data = (result == ReturnCode.SPSS_OK) ? data.Substring(0, len) : null;
			return result;
		}
		/// <summary>
		/// Gets the file label of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="id">
		/// File label.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function retrieves the file label of the SPSS data file associated with 
		/// <paramref>handle</paramref> into the <paramref>id</paramref> parameter.
		/// </remarks>
		public static ReturnCode spssGetIdString(int handle, out string id)
		{
			id = new string(' ', SPSS_MAX_IDSTRING + 1); // leave room for null terminator
			ReturnCode result = SpssThinWrapper.spssGetIdStringImpl(handle, ref id);
			id = (result == ReturnCode.SPSS_OK) ? id.Substring(0, id.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets information on the running version of SPSS, and the hosting computer.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="relInfo">
		/// Array of <see cref="int"/> in which release- and machine-specific data will be
		/// stored.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_TYPE73"/>.
		/// </returns>
		/// <remarks>
		/// This function reports release- and machine-specific information about the file
		/// associated with handle. The information consists of an array of eight int values copied
		/// from record type 7, subtype 3 of the file, and is useful primarily for debugging. The
		/// array elements are, in order, release number (index 0), release subnumber (1), special
		/// release identifier number (2), machine code (3), floating-point representation code (4),
		/// compression scheme code (5), big/little-endian code (6), and character representation
		/// code (7).
		/// </remarks>
		public static ReturnCode spssGetReleaseInfo(int handle, out int[] relInfo)
		{
			relInfo = new int[8];
			ReturnCode result = SpssThinWrapper.spssGetReleaseInfoImpl(handle, relInfo);
			return result;
		}
		/// <summary>
		/// Gets the name of the system that created a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="sysName">
		/// The originating system name.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function returns the name of the system under which the file was created.
		/// </remarks>
		public static ReturnCode spssGetSystemString(int handle, out string sysName)
		{
			sysName = new string(' ', SPSS_SYSTEM_STRING_LENGTH + 1); // leave room for null terminator
			ReturnCode result = SpssThinWrapper.spssGetSystemStringImpl(handle, ref sysName);
			sysName = (result == ReturnCode.SPSS_OK) ? sysName.Substring(0, sysName.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets the data created by TextSmart.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="textInfo">
		/// Text data.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function retrieves the text data created by TextSmart.
		/// </remarks>
		public static ReturnCode spssGetTextInfo(int handle, out string textInfo)
		{
			textInfo = new string(' ', SPSS_MAX_TEXTINFO + 1);
			ReturnCode result = SpssThinWrapper.spssGetTextInfoImpl(handle, ref textInfo);
			textInfo = (result == ReturnCode.SPSS_OK) ? textInfo.Substring(0, textInfo.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets the creation date of a data file, as recorded in the file itself.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="fileDate">
		/// File creation date.
		/// </param>
		/// <param name="fileTime">
		/// File creation time.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function returns the creation date of the file as recorded in the file itself. The creation
		/// date is a 9-byte string in dd mmm yy format (27 Feb 96). The creation time is a 
		/// 8-byte oypcbi in hh:mm:ss format (13:12:15).
		/// </remarks>
		public static ReturnCode spssGetTimeStamp(int handle, out string fileDate, out string fileTime)
		{
			fileDate = new string(' ', SPSS_DATESTAMP_LENGTH + 1);
			fileTime = new string(' ', SPSS_TIMESTAMP_LENGTH + 1);
			ReturnCode result = SpssThinWrapper.spssGetTimeStampImpl(handle, ref fileDate, ref fileTime);
			fileDate = (result == ReturnCode.SPSS_OK) ? fileDate.Substring(0, fileDate.IndexOf('\0')) : null;
			fileTime = (result == ReturnCode.SPSS_OK) ? fileTime.Substring(0, fileTime.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets the string value of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varHandle">
		/// Handle of the variable.
		/// </param>
		/// <param name="value">
		/// Value of the string variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_CASE"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_BUFFER_SHORT"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the value of a string variable for the current case, which is the case
		/// read by the most recent call to <see cref="SpssThinWrapper.spssReadCaseRecordDelegate"/>. 
		/// </remarks>
		public static ReturnCode spssGetValueChar(int handle, double varHandle, out string value)
		{
			value = new string(' ', SPSS_MAX_LONGSTRING + 1); // leave room for null terminator
			ReturnCode result = SpssThinWrapper.spssGetValueCharImpl(handle, varHandle, ref value, value.Length);
			if (result == ReturnCode.SPSS_BUFFER_SHORT)
			{
				value = new string(' ', SPSS_MAX_VERYLONGSTRING + 1);
				result = SpssThinWrapper.spssGetValueCharImpl(handle, varHandle, ref value, value.Length);
			}

			value = (result == ReturnCode.SPSS_OK) ? value.Substring(0, value.IndexOf('\0')) : null;
			return result;
		}

		/// <summary>
		/// Gets the alignment of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="alignment">
		/// Alignment of the variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the value of the alignment attribute of a variable.
		/// </remarks>
		public static ReturnCode spssGetVarAlignment(int handle, string varName, out AlignmentCode alignment)
		{
			return SpssThinWrapper.spssGetVarAlignmentImpl(handle, ref varName, out alignment);
		}
		/// <summary>
		/// Gets the missing values of a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="missingFormat">
		/// Missing value format code.
		/// </param>
		/// <param name="missingVal1">
		/// First missing value.
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value.
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, 
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_SHORTSTR_EXP"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the missing values of a short string variable. The value of
		/// <paramref>missingFormat</paramref> will indicate 
		/// the number of missing values. The
		/// appropriate number of missing values is copied to the <paramref>missingVal1</paramref>,
		/// <paramref>missingVal2</paramref>, and <paramref>missingVal3</paramref> parameters. 
		/// </remarks>
		public static ReturnCode spssGetVarCMissingValues(int handle, string varName, out MissingValueFormatCode missingFormat, out string missingVal1, out string missingVal2, out string missingVal3)
		{
			missingVal1 = new string(' ', SPSS_MAX_SHORTSTRING + 1);
			missingVal2 = new string(' ', SPSS_MAX_SHORTSTRING + 1);
			missingVal3 = new string(' ', SPSS_MAX_SHORTSTRING + 1);
			ReturnCode result = SpssThinWrapper.spssGetVarCMissingValuesImpl(handle, ref varName, out missingFormat,
				ref missingVal1, ref missingVal2, ref missingVal3);
			if (missingFormat < MissingValueFormatCode.SPSS_THREE_MISSVAL)
				missingVal3 = null;
			else
				missingVal3 = (result == ReturnCode.SPSS_OK) ? missingVal3.Substring(0, missingVal3.IndexOf('\0')) : null;
			if (missingFormat < MissingValueFormatCode.SPSS_TWO_MISSVAL)
				missingVal2 = null;
			else
				missingVal2 = (result == ReturnCode.SPSS_OK) ? missingVal2.Substring(0, missingVal2.IndexOf('\0')) : null;
			if (missingFormat < MissingValueFormatCode.SPSS_ONE_MISSVAL)
				missingVal1 = null;
			else
				missingVal1 = (result == ReturnCode.SPSS_OK) ? missingVal1.Substring(0, missingVal1.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets the width of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="columnWidth">
		/// Column width.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the value of the column width attribute of a variable. A value of
		/// zero is special and means that the SPSS Data Editor, which is the primary user of this
		/// attribute, will set an appropriate width using its own algorithm.
		/// </remarks>
		public static ReturnCode spssGetVarColumnWidth(int handle, string varName, out int columnWidth)
		{
			return SpssThinWrapper.spssGetVarColumnWidthImpl(handle, ref varName, out columnWidth);
		}
		/// <summary>
		/// Gets the name and type of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="iVar">
		/// Zero origin variable number.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="varType">
		/// Variable type.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the name and type of one of the variables present in a data file. It
		/// serves the same purpose as <see cref="spssGetVarNames"/> but returns the information one variable
		/// at a time and, therefore, can be passed to a Visual Basic program. 
		/// The type code is an integer in the range 0–255, 0 indicating a numeric
		/// variable and a positive value indicating a string variable of that size.
		/// </remarks>
		public static ReturnCode spssGetVarInfo(int handle, int iVar, out string varName, out int varType)
		{
			varName = new string(' ', SpssSafeWrapper.SPSS_MAX_VARNAME + 1);
			ReturnCode result = SpssThinWrapper.spssGetVarInfoImpl(handle, iVar, ref varName, out varType);
			varName = (result == ReturnCode.SPSS_OK) ? varName.Substring(0, varName.IndexOf('\0')) : null;
			return result;
		}
		/// <summary>
		/// Gets the handle for a named variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="varHandle">
		/// Handle for the variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function returns a handle for a variable, which can then be used to read or write
		/// (depending on how the file was opened) values of the variable. If handle is associated
		/// with an output file, the dictionary must be written with <see cref="SpssThinWrapper.spssCommitHeaderDelegate"/> 
		/// before variable handles can be obtained via spssGetVarHandle.
		/// </remarks>
		public static ReturnCode spssGetVarHandle(int handle, string varName, out double varHandle)
		{
			return SpssThinWrapper.spssGetVarHandleImpl(handle, ref varName, out varHandle);
		}
		/// <summary>
		/// Gets the variable label for some named variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="varLabel">
		/// Variable label.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABEL"/>, 
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function copies the label of variable <paramref>varName</paramref> into 
		/// <paramref>varLabel</paramref>.  To get labels more than 120 characters long, use
		/// the spssGetVarLabelLong function.
		/// </remarks>
		public static ReturnCode spssGetVarLabel(int handle, string varName, out string varLabel)
		{
			int len;
			varLabel = new string(' ', SPSS_MAX_LONGSTRING + 1); // leave room for null terminator
			ReturnCode result = spssGetVarLabelLongImpl(handle, ref varName, ref varLabel, varLabel.Length, out len);
			varLabel = (result == ReturnCode.SPSS_OK) ? varLabel.Substring(0, len) : varLabel = null;
			return result;
		}

		/// <summary>
		/// Gets the measurement level of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="measureLevel">
		/// Measurement level.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the value of the measurement level attribute of a variable.
		/// </remarks>
		public static ReturnCode spssGetVarMeasureLevel(int handle, string varName, out MeasurementLevelCode measureLevel)
		{
			int measureLevelInt;
			ReturnCode status = SpssThinWrapper.spssGetVarMeasureLevelImpl(handle, ref varName, out measureLevelInt);
			measureLevel = (MeasurementLevelCode)measureLevelInt;
			return status;
		}
		/// <summary>
		/// Gets the missing values of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="missingFormat">
		/// Missing value format code.
		/// </param>
		/// <param name="missingVal1">
		/// First missing value.
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value.
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, 
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>, or
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the missing values of a numeric variable. The value of
		/// <paramref>missingFormat</paramref> determines the interpretation of 
		/// <paramref>missingVal1</paramref>, <paramref>missingVal2</paramref>, and
		/// <paramref>missingVal3</paramref>. If missingFormat is SPSS_MISS_RANGE, 
		/// <paramref>missingVal1</paramref> and <paramref>missingVal2</paramref>
		/// represent the upper and lower limits, respectively, of the range, and 
		/// <paramref>missingVal3</paramref> is not used. If missingFormat is 
		/// SPSS_MISS_RANGEANDVAL, <paramref>missingVal1</paramref> and 
		/// <paramref>missingVal2</paramref> represent the range and 
		/// <paramref>missingVal3</paramref> is the discrete missing value. 
		/// If missingFormat is neither of the above, it will be in the range 
		/// 0–3, indicating the number of discrete missing
		/// values present. (The macros SPSS_NO_MISSVAL, SPSS_ONE_MISSVAL,
		/// SPSS_TWO_MISSVAL, and SPSS_THREE_MISSVAL may be used as synonyms for 0–3.)
		/// </remarks>
		public static ReturnCode spssGetVarNMissingValues(int handle, string varName, out MissingValueFormatCode missingFormat, out double missingVal1, out double missingVal2, out double missingVal3)
		{
			return SpssThinWrapper.spssGetVarNMissingValuesImpl(handle, ref varName, out missingFormat, out missingVal1, out missingVal2, out missingVal3);
		}
		/// <summary>
		/// Gets the value label for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="value">
		/// Short string value for which the label is wanted.
		/// </param>
		/// <param name="label">
		/// Label for the value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABELS"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABEL"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_SHORTSTR_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_EXC_STRVALUE"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the value label for a given value of a short string variable. 
		/// </remarks>
		public static ReturnCode spssGetVarCValueLabel(int handle, string varName, string value, out string label)
		{
			int len;
			label = new string(' ', SPSS_MAX_VALLABEL + 1);
			ReturnCode result = spssGetVarCValueLabelLongImpl(handle, ref varName, ref value, ref label, label.Length, out len);
			label = (result == ReturnCode.SPSS_OK) ? label.Substring(0, len) : null;
			return result;
		}
		/// <summary>
		/// Gets the value label for a given value of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="value">
		/// Numeric value for which the label is wanted.
		/// </param>
		/// <param name="label">
		/// Label for the value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABELS"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABEL"/>, 
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, 
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>, or
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the value label for a given value of a numeric variable. 
		/// </remarks>
		public static ReturnCode spssGetVarNValueLabel(int handle, string varName, double value, out string label)
		{
			int len;
			label = new string(' ', SPSS_MAX_VALLABEL + 1); // leave room for null terminator
			ReturnCode result = spssGetVarNValueLabelLong(handle, ref varName, value, ref label, label.Length, out len);
			if (result == ReturnCode.SPSS_OK)
				label = label.Substring(0, len);
			else
				label = null;

			return result;
		}

		/// <summary>
		/// Gets print formatting information for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="printType">
		/// Print format type code (file spssdio.h defines macros of
		/// the form <see cref="FormatTypeCode">SPSS_FMT_...</see> for all valid format type codes)
		/// </param>
		/// <param name="printDec">
		/// Number of digits after the decimal.
		/// </param>
		/// <param name="printWidth">
		/// Print format width.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the print format of a variable. Format type, number of decimal
		/// places, and field width are returned as <paramref>printType</paramref>, 
		/// <paramref>printDec</paramref>, and <paramref>printWid</paramref>, respectively.
		/// </remarks>
		public static ReturnCode spssGetVarPrintFormat(int handle, string varName, out FormatTypeCode printType, out int printDec, out int printWidth)
		{
			return SpssThinWrapper.spssGetVarPrintFormatImpl(handle, ref varName, out printType, out printDec, out printWidth);
		}
		/// <summary>
		/// Gets the write format of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="writeType">
		/// Pointer to write format type code (file spssdio.h defines macros of
		/// the form <see cref="FormatTypeCode">SPSS_FMT_...</see> for all valid format type codes)
		/// </param>
		/// <param name="writeDec">
		/// Pointer to number of digits after the decimal
		/// </param>
		/// <param name="writeWidth">
		/// Pointer to write format width
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the write format of a variable. Format type, number of decimal
		/// places, and field width are returned as writeType, writeDec, and writeWid,
		/// respectively.
		/// </remarks>
		public static ReturnCode spssGetVarWriteFormat(int handle, string varName, out FormatTypeCode writeType, out int writeDec, out int writeWidth)
		{
			return SpssThinWrapper.spssGetVarWriteFormatImpl(handle, ref varName, out writeType, out writeDec, out writeWidth);
		}
		/// <summary>
		/// Opens an SPSS file for appending cases.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file.
		/// </param>
		/// <param name="handle">
		/// Handle to the opened file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FITAB_FULL"/>,
		/// <see cref="ReturnCode.SPSS_FILE_OERROR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>,
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>,
		/// <see cref="ReturnCode.SPSS_NO_TYPE2"/>,
		/// <see cref="ReturnCode.SPSS_NO_TYPE999"/>, or
		/// <see cref="ReturnCode.SPSS_INCOMPAT_APPEND"/>.
		/// </returns>
		/// <remarks>
		/// This function opens an SPSS data file for appending cases and returns a handle that
		/// should be used for subsequent operations on the file.
		/// </remarks>
		public static ReturnCode spssOpenAppend(string fileName, out int handle)
		{
			return SpssThinWrapper.spssOpenAppendImpl(ref fileName, out handle);
		}
		/// <summary>
		/// Creates a new SPSS data file, with a dictionary copied from an existing file.
		/// </summary>
		/// <param name="fileName">
		/// Name of the new file.
		/// </param>
		/// <param name="dictFileName">
		/// Name of existing file.
		/// </param>
		/// <param name="handle">
		/// Handle of the new file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FITAB_FULL"/>,
		/// <see cref="ReturnCode.SPSS_FILE_OERROR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>,
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>,
		/// <see cref="ReturnCode.SPSS_NO_TYPE2"/>, or
		/// <see cref="ReturnCode.SPSS_NO_TYPE999"/>.
		/// </returns>
		/// <remarks>
		/// This function opens a file in preparation for creating a new SPSS data file and initializes
		/// its dictionary from that of an existing SPSS data file. It is useful when you want to modify
		/// the dictionary or data of an existing file or replace all of its data. The typical sequence
		/// of operations is to call <see cref="spssOpenWriteCopy"/> (newFileName, oldFileName, ...) to open a
		/// new file initialized with a copy of the old file’s dictionary, then <see cref="spssOpenRead"/> (oldFile-
		/// Name, ...) to open the old file to access its data.
		/// </remarks>
		public static ReturnCode spssOpenWriteCopy(string fileName, string dictFileName, out int handle)
		{
			return SpssThinWrapper.spssOpenWriteCopyImpl(ref fileName, ref dictFileName, out handle);
		}
		/// <summary>
		/// Sets the case weight variable in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// The name of the case weight variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function defines variable varName as the case weight variable for the data file
		/// specified by the handle.
		/// </remarks>
		public static ReturnCode spssSetCaseWeightVar(int handle, string varName)
		{
			return SpssThinWrapper.spssSetCaseWeightVarImpl(handle, ref varName);
		}
		/// <summary>
		/// Sets the Trends date variable information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="dateInfo">
		/// Array containing date variables information.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_DATEINFO"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the Trends date variable information. The array at dateInfo is
		/// assumed to have numofElements elements that correspond to the data array portion of
		/// record 7, subtype 3. Its first six elements comprise the “fixed” information, followed
		/// by a sequence of one or more three-element groups. Since very little validity checking
		/// is done on the input array, this function should be used with caution and is
		/// recommended only for copying Trends information from one file to another.
		/// </remarks>
		unsafe public static ReturnCode spssSetDateVariables(int handle, int[] dateInfo)
		{
			return SpssThinWrapper.spssSetDateVariablesImpl(handle, dateInfo.Length, (int*)Marshal.UnsafeAddrOfPinnedArrayElement(dateInfo, 0));
		}
		/// <summary>
		/// Sets the first block of Data Entry information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="data">
		/// Data to be written.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EMPTY_DEW"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_BADTEMP"/>.
		/// </returns>
		/// <remarks>
		/// DEW information (file information which is private to the SPSS Data Entry product)
		/// can be delivered to the I/O DLL in whatever segments are convenient for the client.
		/// The spssSetDEWFirst function is called to deliver the first such segment, and
		/// subsequent segments are delivered by calling spssSetDEWNext as many times as
		/// necessary.
		/// </remarks>
		public static ReturnCode spssSetDEWFirst(int handle, string data)
		{
			return SpssThinWrapper.spssSetDEWFirstImpl(handle, ref data, data.Length);
		}
		/// <summary>
		/// Sets the unique Data Entry uniqueness GUID.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="asciiGUID">
		/// The GUID to be stored on the file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function stores the Data Entry for Windows uniqueness indicator on the data file.
		/// It should only be used by the DEW product.
		/// </remarks>
		public static ReturnCode spssSetDEWGUID(int handle, string asciiGUID)
		{
			return SpssThinWrapper.spssSetDEWGUIDImpl(handle, ref asciiGUID);
		}
		/// <summary>
		/// Sets the next block of information for Data Entry.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="data">
		/// Data to be written.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_DEW_NOFIRST"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_BADTEMP"/>.
		/// </returns>
		/// <remarks>
		/// The DEW information (file information that is private to the SPSS Data Entry product)
		/// can be delivered to the I/O DLL in whatever segments are convenient for the client.
		/// The spssSetDEWFirst function is called to deliver the first such segment, and
		/// subsequent segments are delivered by calling spssSetDEWNext as many times as
		/// necessary.
		/// </remarks>
		public static ReturnCode spssSetDEWNext(int handle, string data)
		{
			return SpssThinWrapper.spssSetDEWNextImpl(handle, ref data, data.Length);
		}
		/// <summary>
		/// Sets the file label of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="id">
		/// File label. The length of the string should not exceed 
		/// <see cref="SpssThinWrapper.SPSS_MAX_IDSTRING"/> characters.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EXC_LEN64"/>
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>, or
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the file label of the output SPSS data file associated with handle to
		/// the given string id.
		/// </remarks>
		public static ReturnCode spssSetIdString(int handle, string id)
		{
			return SpssThinWrapper.spssSetIdStringImpl(handle, ref id);
		}

		/// <summary>
		/// Sets multiple response definitions to a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="mrespDefs">
		/// ASCII string containing definitions.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EMPTY_MULTRESP"/>
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function is used to write multiple response definitions to the file. The definitions
		/// consist of a single null-terminated ASCII string which is similar to that containing the
		/// variable set definitions.
		/// </remarks>
		public static ReturnCode spssSetMultRespDefs(int handle, string mrespDefs)
		{
			return SpssThinWrapper.spssSetMultRespDefsImpl(handle, ref mrespDefs);
		}

		/// <summary>
		/// Sets the text data from the string in textInfo.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="textInfo">
		/// Text data
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the text data from the null-terminated string in textInfo. If the string is
		/// longer than 255 characters, only the first 255 are (quietly) used. If textInfo contains the
		/// empty string, existing text data, if any, is deleted.
		/// </remarks>
		public static ReturnCode spssSetTextInfo(int handle, string textInfo)
		{
			return SpssThinWrapper.spssSetTextInfoImpl(handle, ref textInfo);
		}

		/// <summary>
		/// Sets the value of a string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varHandle">
		/// Handle to the variable.
		/// </param>
		/// <param name="value">
		/// Value of the variable. The length of the
		/// string (ignoring trailing blanks, if any) should be less than or equal
		/// to the length of the variable.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_EXC_STRVALUE"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the value of a string variable for the current case. The current case is
		/// not written out to the data file until <see cref="SpssThinWrapper.spssCommitCaseRecordDelegate"/> is called.
		/// </remarks>
		public static ReturnCode spssSetValueChar(int handle, double varHandle, string value)
		{
			return SpssThinWrapper.spssSetValueCharImpl(handle, varHandle, ref value);
		}
		/// <summary>
		/// Sets the alignment of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="alignment">
		/// Alignment.  If not a legal
		/// value, alignment is set to a type-appropriate default.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the value of the alignment attribute of a variable.
		/// </remarks>
		public static ReturnCode spssSetVarAlignment(int handle, string varName, AlignmentCode alignment)
		{
			return SpssThinWrapper.spssSetVarAlignmentImpl(handle, ref varName, alignment);
		}
		/// <summary>
		/// Sets missing values for a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="missingFormat">
		/// The number of missing values (0-3) actually supplied.
		/// </param>
		/// <param name="missingVal1">
		/// First missing value.
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value.
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_SHORTSTR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_MISSFOR"/>,
		/// <see cref="ReturnCode.SPSS_EXC_STRVALUE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets missing values for a short string variable. The argument
		/// missingFormat must be set to a value in the range 0–3 to indicate the number of missing
		/// values supplied. When fewer than three missing values are to be defined, the redundant
		/// arguments must still be present, although their values are not inspected. For example,
		/// if missingFormat is 2, missingVal3 is unused. The supplied missing values must be nullterminated
		/// and not longer than the length of the variable unless the excess length is
		/// made up of blanks, which are ignored. If the missing value is shorter than the length of
		/// the variable, trailing blanks are assumed.
		/// </remarks>
		public static ReturnCode spssSetVarCMissingValues(int handle, string varName, MissingValueFormatCode missingFormat, string missingVal1, string missingVal2, string missingVal3)
		{
			return SpssThinWrapper.spssSetVarCMissingValuesImpl(handle, ref varName, missingFormat,
				ref missingVal1, ref missingVal2, ref missingVal3);
		}
		/// <summary>
		/// Sets the column width of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="columnWidth">
		/// Column width. If negative, a value of zero is (quietly) used instead.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the value of the column width attribute of a variable. A value of zero
		/// is special and means that the SPSS Data Editor, which is the primary user of this
		/// attribute, is to set an appropriate width using its own algorithm.
		/// </remarks>
		public static ReturnCode spssSetVarColumnWidth(int handle, string varName, int columnWidth)
		{
			return SpssThinWrapper.spssSetVarColumnWidthImpl(handle, ref varName, columnWidth);
		}
		/// <summary>
		/// Sets the label of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="varLabel">
		/// Variable label. The length of the string should not exceed 
		/// <see cref="SpssThinWrapper.SPSS_MAX_VARLABEL"/> characters.
		/// If varLabel is the empty string, the existing label, if any, is deleted.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EXC_LEN120"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the label of a variable.
		/// </remarks>
		public static ReturnCode spssSetVarLabel(int handle, string varName, string varLabel)
		{
			return SpssThinWrapper.spssSetVarLabelImpl(handle, ref varName, ref varLabel);
		}

		/// <summary>
		/// Creates a new variable with a given name.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="varType">
		/// Type and size of the variable.
		/// </param>
		/// <returns>
		/// <list type="table">
		/// <listheader>
		/// <term>Error code</term>
		/// <description>Description</description>
		/// </listheader>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_OK"/></term>
		///		<description>No error</description>
		/// </item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_INVALID_HANDLE"/></term>
		///		<description>The file handle is not valid</description>
		/// </item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_OPEN_RDMODE"/></term>
		///		<description>File is open for reading, not writing</description>
		///	</item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_DICT_COMMIT"/></term>
		///		<description>Dictionary has already been written with <see cref="SpssThinWrapper.spssCommitHeaderDelegate"/></description>
		///	</item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_INVALID_VARTYPE"/></term>
		///		<description>Invalid length code ( varLength is negative or exceeds 255)</description>
		///	</item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_INVALID_VARNAME"/></term>
		///		<description>Variable name is invalid</description>
		///	</item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_DUP_VAR"/></term>
		///		<description>There is already a variable with the same name</description>
		///	</item>
		/// <item>
		///		<term><see cref="ReturnCode.SPSS_NO_MEMORY"/></term>
		///		<description>Insufficient memory</description>
		///	</item>
		/// </list>
		/// </returns>
		/// <remarks>
		/// This function creates a new variable named varName, which will be either numeric or
		/// string based on varLength. If the latter is zero, a numeric variable with a default format
		/// of F8.2 will be created; if it is greater than 0 and less than or equal to 255, a string variable
		/// with length varLength will be created; any other value will be rejected as invalid.
		/// For better readability, macros SPSS_NUMERIC and SPSS_STRING( length) may be
		/// used as values for varLength.
		/// </remarks>
		public static ReturnCode spssSetVarName(int handle, string varName, int varType)
		{
			return SpssThinWrapper.spssSetVarNameImpl(handle, ref varName, varType);
		}

		/// <summary>
		/// Changes or adds a value label for a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="value">
		/// Value to be labeled.
		/// </param>
		/// <param name="label">
		/// Label.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EXC_LEN60"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_SHORTSTR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_EXC_STRVALUE"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_INTERNAL_VLABS"/>.
		/// </returns>
		/// <remarks>
		/// This function changes or adds a value label for the specified value of a short string
		/// variable. The label should not exceed 60 characters in length.
		/// </remarks>
		public static ReturnCode spssSetVarCValueLabel(int handle, string varName, string value, string label)
		{
			return SpssThinWrapper.spssSetVarCValueLabelImpl(handle, ref varName, ref value, ref label);
		}
		/// <summary>
		/// Sets the variable sets information in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varSets">
		/// Variable sets information.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EMPTY_VARSETS"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the variable sets information in the data file. The information must
		/// be provided in the form of a string. No validity checks are performed
		/// on the supplied string beyond ensuring that its length is not 0. Any existing variable
		/// sets information is discarded.
		/// </remarks>
		public static ReturnCode spssSetVariableSets(int handle, string varSets)
		{
			return SpssThinWrapper.spssSetVariableSetsImpl(handle, ref varSets);
		}
		/// <summary>
		/// Sets the measurement level of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="measureLevel">
		/// Measurement level.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_MEASURELEVEL"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the value of the measurement level attribute of a variable.
		/// </remarks>
		public static ReturnCode spssSetVarMeasureLevel(int handle, string varName, MeasurementLevelCode measureLevel)
		{
			return SpssThinWrapper.spssSetVarMeasureLevelImpl(handle, ref varName, (int)measureLevel);
		}
		/// <summary>
		/// Sets the missing values for a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="missingFormat">
		/// Missing values format code.
		/// </param>
		/// <param name="missingVal1">
		/// First missing value.
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value.
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_MISSFOR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets missing values for a numeric variable. The interpretation of the
		/// arguments missingVal1, missingVal2, and missingVal3 depends on the value of
		/// missingFormat. If missingFormat is set to SPSS_MISS_RANGE, missingVal1 and
		/// missingVal2 are taken as the upper and lower limits, respectively, of the range, and
		/// missingVal3 is ignored. If missingFormat is SPSS_MISS_RANGEANDVAL, missingval1
		/// and missingVal2 are taken as limits of the range and missingVal3 is taken as the discrete
		/// missing value. If missingFormat is neither of the above, it must be in the range 0–3,
		/// indicating the number of discrete missing values present. For example, if
		/// missingFormat is 2, missingVal1 and missingVal2 are taken as two discrete missing
		/// values and missingVal3 is ignored. (The macros SPSS_NO_MISSVAL,
		/// SPSS_ONE_MISSVAL, SPSS_TWO_MISSVAL, and SPSS_THREE_MISSVAL may be
		/// used as synonyms for 0–3.)
		/// </remarks>
		public static ReturnCode spssSetVarNMissingValues(int handle, string varName, MissingValueFormatCode missingFormat, double missingVal1, double missingVal2, double missingVal3)
		{
			return SpssThinWrapper.spssSetVarNMissingValuesImpl(handle, ref varName, missingFormat,
				missingVal1, missingVal2, missingVal3);
		}
		/// <summary>
		/// Changes or adds a value label to a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="value">
		/// Value to be labeled.
		/// </param>
		/// <param name="label">
		/// Label.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_EXC_LEN60"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_INTERNAL_VLABS"/>.
		/// </returns>
		/// <remarks>
		/// This function changes or adds a value label for the specified value of a numeric
		/// variable. The label should not exceed 60 characters in length.
		/// </remarks>
		public static ReturnCode spssSetVarNValueLabel(int handle, string varName, double value, string label)
		{
			return SpssThinWrapper.spssSetVarNValueLabelImpl(handle, ref varName, value, ref label);
		}

		/// <summary>
		/// Sets the print format of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="printType">
		/// Print format type code (file spssdio.h defines macros of the form
		/// <see cref="FormatTypeCode">SPSS_FMT_...</see> for all 
		/// valid format type codes).
		/// </param>
		/// <param name="printDec">
		/// Number of digits after the decimal.
		/// </param>
		/// <param name="printWidth">
		/// Print format width.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_PRFOR"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the print format of a variable.
		/// </remarks>
		public static ReturnCode spssSetVarPrintFormat(int handle, string varName, FormatTypeCode printType, int printDec, int printWidth)
		{
			return SpssThinWrapper.spssSetVarPrintFormatImpl(handle, ref varName, printType, printDec, printWidth);
		}

		/// <summary>
		/// Sets the write format of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="writeType">
		/// Write format type code (file spssdio.h defines macros of the form
		/// <see cref="FormatTypeCode">SPSS_FMT_...</see> for all valid format type codes).
		/// </param>
		/// <param name="writeDec">
		/// Number of digits after the decimal.
		/// </param>
		/// <param name="writeWidth">
		/// Write format width.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_WRFOR"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the write format of a variable.
		/// </remarks>
		public static ReturnCode spssSetVarWriteFormat(int handle, string varName, FormatTypeCode writeType, int writeDec, int writeWidth)
		{
			return SpssThinWrapper.spssSetVarWriteFormatImpl(handle, ref varName, writeType, writeDec, writeWidth);
		}
		/// <summary>
		/// Reads in the raw data for an entire case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="caseRec">
		/// Buffer to contain the case.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FILE_END"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function reads a case from a data file into a case buffer provided by the user. The
		/// required size of the buffer may be obtained by calling <see cref="SpssThinWrapper.spssGetCaseSizeDelegate"/>. This is a fairly
		/// low-level function whose use should not be mixed with calls to <see cref="SpssThinWrapper.spssReadCaseRecordDelegate"/>
		/// using the same file handle because both procedures read a new case from the data file.
		/// </remarks>
		public static ReturnCode spssWholeCaseIn(int handle, out string caseRec)
		{
			// Fetch the size of each case to set an appropriately sized buffer
			int caseSize;
			ReturnCode result = spssGetCaseSizeImpl(handle, out caseSize);
			if (result != ReturnCode.SPSS_OK)
			{
				caseRec = null;
				return result;
			}

			caseRec = new string(' ', caseSize);
			return SpssThinWrapper.spssWholeCaseInImpl(handle, ref caseRec);
		}
		/// <summary>
		/// Writes out raw data as a case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="caseRec">
		/// Case record to be written to the data file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_WERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function writes a case assembled by the caller to a data file. The case is assumed
		/// to have been constructed correctly in the buffer caseRec, and its validity is not
		/// checked. This is a fairly low-level function whose use should not be mixed with calls
		/// to <see cref="SpssThinWrapper.spssCommitCaseRecordDelegate"/> using the same file handle 
		/// because both procedures write a new case to the data file.
		/// </remarks>
		public static ReturnCode spssWholeCaseOut(int handle, string caseRec)
		{
			return SpssThinWrapper.spssWholeCaseOutImpl(handle, ref caseRec);
		}

		/// <summary>
		/// Gets variable sets information in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varSets">
		/// Variable sets string.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_VARSETS"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the variable sets information in the data file. 
		/// </remarks>
		unsafe public static ReturnCode spssGetVariableSets(int handle, out string varSets)
		{
			char* cVarSets;
			ReturnCode result = SpssThinWrapper.spssGetVariableSetsImpl(handle, out cVarSets);
			try
			{
				if (result == ReturnCode.SPSS_OK)
					varSets = Marshal.PtrToStringAnsi(new IntPtr(cVarSets));
				else
					varSets = null;

				return result;
			}
			finally
			{
				if (cVarSets != null)
					spssFreeVariableSetsImpl(cVarSets);
			}
		}
		/// <summary>
		/// Reports TRENDS date variable information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="dateInfo">
		/// Pointer to first element of the allocated array.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_DATEINFO"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the TRENDS date variable information, if any, in an SPSS data file.
		/// It places the information in a dynamically allocated long array, sets *numofElements to
		/// the number of elements in the array, and sets *dateInfo to point to the array. The caller is
		/// expected to free the array by calling <see cref="SpssThinWrapper.spssFreeDateVariablesDelegate"/> when it is no longer needed.
		/// The variable information is copied directly from record 7, subtype 3. Its first six elements
		/// comprise the "fixed" information, followed by a sequence of one or more three-element
		/// groups.
		/// </remarks>
		unsafe public static ReturnCode spssGetDateVariables(int handle, int[] dateInfo)
		{
			int* cDateInfo;
			int numofElements;
			ReturnCode result = SpssThinWrapper.spssGetDateVariablesImpl(handle, out numofElements, out cDateInfo);
			try
			{
				if (result == ReturnCode.SPSS_OK)
				{
					dateInfo = new int[numofElements];
					for (int i = 0; i < numofElements; i++)
						dateInfo[i] = cDateInfo[i];
				}

				return result;
			}
			finally
			{
				if (cDateInfo != null)
					spssFreeDateVariablesImpl(cDateInfo);
			}
		}
		/// <summary>
		/// Retrieves the definitions from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="mrespDefs">
		/// Returned string.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_MULTRESP"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function retrieves the definitions from an SPSS data file. The definitions are stored
		/// as a null-terminated ASCII string which is very similar to that containing the variable
		/// set definitions. The memory allocated by this function to contain the string must be freed
		/// by calling <see cref="SpssThinWrapper.spssFreeMultRespDefsDelegate"/>. If the file contains no multiple response definitions,
		/// *mrespDefs is set to NULL, and the function returns the warning code
		/// <see cref="ReturnCode.SPSS_NO_MULTRESP"/>.
		/// </remarks>
		unsafe public static ReturnCode spssGetMultRespDefs(int handle, out string mrespDefs)
		{
			char* cMrespDefs;
			ReturnCode result = SpssThinWrapper.spssGetMultRespDefsImpl(handle, out cMrespDefs);
			try
			{
				if (result == ReturnCode.SPSS_OK)
					mrespDefs = Marshal.PtrToStringAnsi(new IntPtr(cMrespDefs));
				else
					mrespDefs = null;

				return result;
			}
			finally
			{
				if (cMrespDefs != null)
					spssFreeMultRespDefsImpl(cMrespDefs);
			}
		}
		/// <summary>
		/// Gets a list of all response values and labels for a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="values">
		/// Array of values.
		/// </param>
		/// <param name="labels">
		/// Array of labels.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABELS"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, 
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the set of labeled values and associated labels for a numeric variable.
		/// The number of values is returned as *numLabels. Values are stored into an array of
		/// *numLabels double elements, and *values is set to point to the first element of the array.
		/// The corresponding labels are structured as an array of *numLabels pointers, each pointing
		/// to a char string containing a null-terminated label, and *labels is set to point to the
		/// first element of the array.
		/// The two arrays and the label strings are allocated on the heap. When they are no longer
		/// needed, <see cref="SpssThinWrapper.spssFreeVarNValueLabelsDelegate"/> should be called to free the memory.
		/// </remarks>
		unsafe public static ReturnCode spssGetVarNValueLabels(int handle, string varName, out double[] values, out string[] labels)
		{
			double* cValues;
			char** cLabels;
			int numLabels;
			ReturnCode result = SpssException.ThrowOnFailure(SpssThinWrapper.spssGetVarNValueLabelsImpl(handle, ref varName, out cValues, out cLabels, out numLabels), "spssGetVarNValueLabels", ReturnCode.SPSS_NO_LABELS);
			if (result == ReturnCode.SPSS_NO_LABELS)
			{
				values = new double[0];
				labels = new string[0];
			}
			else
			{ // ReturnCode.SPSS_OK
				values = new double[numLabels];
				labels = new string[numLabels];
				for (int i = 0; i < numLabels; i++)
				{
					values[i] = cValues[i];
					labels[i] = Marshal.PtrToStringAnsi(new IntPtr(cLabels[i]));
				}
				spssFreeVarNValueLabels(cValues, cLabels, numLabels);
			}

			return result;
		}

		/// <summary>
		/// Gets the list of response values and labels for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="varName">
		/// Variable name.
		/// </param>
		/// <param name="values">
		/// Array of values.
		/// </param>
		/// <param name="labels">
		/// Array of labels.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABELS"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>,
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>,
		/// <see cref="ReturnCode.SPSS_STR_EXP"/>,
		/// <see cref="ReturnCode.SPSS_SHORTSTR_EXP"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the set of labeled values and associated labels for a short string variable.
		/// The number of values is returned as *numLabels. Values are stored into an array
		/// of *numLabels pointers, each pointing to a char string containing a null-terminated
		/// value, and *values is set to point to the first element of the array. Each value string is as
		/// long as the variable. The corresponding labels are structured as an array of *numLabels
		/// pointers, each pointing to a char string containing a null-terminated label, and *labels is
		/// set to point to the first element of the array.
		/// The two arrays and the value and label strings are allocated on the heap. When they
		/// are no longer needed, <see cref="SpssThinWrapper.spssFreeVarCValueLabelsDelegate"/> should be called to free the memory.
		/// </remarks>
		unsafe public static ReturnCode spssGetVarCValueLabels(int handle, string varName, out string[] values, out string[] labels)
		{
			char** cValues;
			char** cLabels;
			int numLabels;

			ReturnCode result = SpssException.ThrowOnFailure(SpssThinWrapper.spssGetVarCValueLabelsImpl(handle, ref varName, out cValues, out cLabels, out numLabels), "spssGetVarCValueLabels", ReturnCode.SPSS_NO_LABELS);
			if (result == ReturnCode.SPSS_NO_LABELS)
			{
				values = new string[0];
				labels = new string[0];
			}
			else
			{ // ReturnCode.SPSS_OK
				values = new string[numLabels];
				labels = new string[numLabels];
				for (int i = 0; i < numLabels; i++)
				{
					values[i] = Marshal.PtrToStringAnsi(new IntPtr(cValues[i]));
					labels[i] = Marshal.PtrToStringAnsi(new IntPtr(cLabels[i]));
				}
				SpssThinWrapper.spssFreeVarCValueLabelsImpl(cValues, cLabels, numLabels);
			}

			return result;
		}

		/// <summary>
		/// Reports on all the variables in a data file, including names and types.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varNames">
		/// The array to fill with variable names
		/// </param>
		/// <param name="varTypes">
		/// The array to fill with variable types
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the names and types of all the variables present in a data file. 
		/// The type code is an integer in the range 0-255, 0 indicating a numeric
		/// variable and a positive value indicating a string variable of that size.
		/// </remarks>
		unsafe public static ReturnCode spssGetVarNames(int handle, out string[] varNames, out int[] varTypes)
		{
			int numVars;
			char** cVarNames;
			int* cVarTypes;
			ReturnCode result = SpssThinWrapper.spssGetVarNamesImpl(handle, out numVars, out cVarNames, out cVarTypes);
			if (result != ReturnCode.SPSS_OK)
			{
				varNames = null;
				varTypes = null;
			}
			else // all is well
			{
				varNames = new string[numVars];
				varTypes = new int[numVars];
				for (int i = 0; i < numVars; i++)
				{
					varNames[i] = Marshal.PtrToStringAnsi(new IntPtr(cVarNames[i]));
					varTypes[i] = cVarTypes[i];
				}
				spssFreeVarNamesImpl(cVarNames, cVarTypes, numVars);
			}
			return result;
		}


		#region Obsolete, exception throwing wrappers for SPSS functions
		[Obsolete("Use spssSetVarWriteFormat instead.")]
		public static void SetVarWriteFormat(int handle, string varName, FormatTypeCode writeType, int writeDec, int writeWidth)
		{
			SpssException.ThrowOnFailure(spssSetVarWriteFormat(handle, varName, writeType, writeDec, writeWidth), "spssSetVarWriteFormat");
		}
		[Obsolete("Use spssSetVarPrintFormat instead.")]
		public static void SetVarPrintFormat(int handle, string varName, FormatTypeCode printType, int printDec, int printWidth)
		{
			SpssException.ThrowOnFailure(spssSetVarPrintFormat(handle, varName, printType, printDec, printWidth), "spssSetVarPrintFormat");
		}
		[Obsolete("Use spssSetVarName instead.")]
		public static void SetVarName(int handle, string varName, int varType)
		{
			SpssException.ThrowOnFailure(spssSetVarName(handle, varName, varType), "spssSetVarName");
		}
		[Obsolete("Use spssSetVarLabel instead.")]
		public static void SetVarLabel(int handle, string varName, string varLabel)
		{
			SpssException.ThrowOnFailure(spssSetVarLabel(handle, varName, varLabel), "spssSetVarLabel");
		}
		[Obsolete("Use spssSetVarNValueLabel instead.")]
		public static void SetVarNValueLabel(int handle, string varName, double value, string label)
		{
			SpssException.ThrowOnFailure(spssSetVarNValueLabel(handle, varName, value, label), "spssSetVarNValueLabel");
		}

		[Obsolete("Use spssGetVarLabel instead.")]
		public static string GetVarLabel(int handle, string varName)
		{
			string varLabel;
			SpssException.ThrowOnFailure(spssGetVarLabel(handle, varName, out varLabel), "spssGetVarLabel");
			return varLabel;
		}

		[Obsolete("Use spssGetVarNValueLabel instead.")]
		public static string GetVarNValueLabel(int handle, string varName, double value)
		{
			string label;
			SpssException.ThrowOnFailure(spssGetVarNValueLabel(handle, varName, value, out label), "spssGetVarNValueLabel");
			return label;
		}

		[Obsolete("Use spssGetValueChar instead.")]
		public static string GetValueChar(int handle, double varHandle)
		{
			string value;
			SpssException.ThrowOnFailure(spssGetValueChar(handle, varHandle, out value), "spssGetValueChar");
			return value;
		}
		#endregion
	}
}
