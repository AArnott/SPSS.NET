using System;
using System.Runtime.InteropServices;

namespace Spss
{
	#region Enumerables
	/// <summary>
	/// Error/warning codes that calls to SPSS methods can return.
	/// </summary>
	public enum ReturnCode : int 
	{
		/// <summary>
		/// No error
		/// </summary>
		SPSS_OK = 0,

		#region Error codes that calls to SPSS methods can return.
		/// <summary>
		/// Error opening file
		/// </summary>
		SPSS_FILE_OERROR = 1,
		/// <summary>
		/// File write error
		/// </summary>
		SPSS_FILE_WERROR = 2,
		/// <summary>
		/// Error reading the file
		/// </summary>
		SPSS_FILE_RERROR = 3,
		/// <summary>
		/// File table full (too many open SPSS data files)
		/// </summary>
		SPSS_FITAB_FULL = 4,
		/// <summary>
		/// The file handle is not valid
		/// </summary>
		SPSS_INVALID_HANDLE = 5,
		/// <summary>
		/// Data file contains no variables
		/// </summary>
		SPSS_INVALID_FILE = 6,
		/// <summary>
		/// Insufficient memory
		/// </summary>
		SPSS_NO_MEMORY = 7,

		/// <summary>
		/// File is open for reading, not writing
		/// </summary>
		SPSS_OPEN_RDMODE = 8,
		/// <summary>
		/// The file is open for writing, not reading
		/// </summary>
		SPSS_OPEN_WRMODE = 9,

		/// <summary>
		/// The variable name is not valid
		/// </summary>
		SPSS_INVALID_VARNAME = 10,
		/// <summary>
		/// No variables defined in the dictionary
		/// </summary>
		SPSS_DICT_EMPTY = 11,
		/// <summary>
		/// A variable with the given name does not exist
		/// </summary>
		SPSS_VAR_NOTFOUND = 12,
		/// <summary>
		/// There is already a variable with the same name
		/// </summary>
		SPSS_DUP_VAR = 13,
		/// <summary>
		/// The variable is not numeric
		/// OR
		/// At least one variable in the list is not numeric
		/// OR
		/// The specified variable has string values
		/// </summary>
		SPSS_NUME_EXP = 14,
		/// <summary>
		/// The variable is numeric
		/// </summary>
		SPSS_STR_EXP = 15,
		/// <summary>
		/// The variable is a long string (length > 8)
		/// </summary>
		SPSS_SHORTSTR_EXP = 16,
		/// <summary>
		/// Invalid length code (varLength is negative or
		/// exceeds 255)
		/// </summary>
		SPSS_INVALID_VARTYPE = 17,

		/// <summary>
		/// Invalid missing values specification ( missingFormat
		/// is invalid or the lower limit of range is greater than the
		/// upper limit)
		/// </summary>
		SPSS_INVALID_MISSFOR = 18,
		/// <summary>
		/// Invalid compression switch (other than 0 or 1)
		/// </summary>
		SPSS_INVALID_COMPSW = 19,
		/// <summary>
		/// The print format specification is invalid or is
		/// incompatible with the variable type
		/// </summary>
		SPSS_INVALID_PRFOR = 20,
		/// <summary>
		/// The write format specification is invalid or is
		/// incompatible with the variable type
		/// </summary>
		SPSS_INVALID_WRFOR = 21,
		/// <summary>
		/// Invalid date 
		/// OR
		/// The date value (spssDate) is negative
		/// </summary>
		SPSS_INVALID_DATE = 22,
		/// <summary>
		/// Invalid time
		/// </summary>
		SPSS_INVALID_TIME = 23,

		/// <summary>
		/// Fewer than two variables in list 
		/// OR
		/// Number of variables ( numVars) is zero or negative
		/// </summary>
		SPSS_NO_VARIABLES = 24,
		/// <summary>
		/// The list of values contains duplicates
		/// </summary>
		SPSS_DUP_VALUE = 27,

		/// <summary>
		/// The given case weight variable is invalid. This error
		/// signals an internal problem in the implementation
		/// of the DLL and should never occur.
		/// </summary>
		SPSS_INVALID_CASEWGT = 28,
		/// <summary>
		/// Dictionary has already been written with
		/// spssCommitHeader
		/// </summary>
		SPSS_DICT_COMMIT = 30,
		/// <summary>
		/// Dictionary of the output file has not yet been written
		/// with <see cref="SpssThinWrapper.spssCommitHeader"/>.
		/// </summary>
		SPSS_DICT_NOTCOMMIT = 31,

		/// <summary>
		/// File is not a valid SPSS data file (no type 2 record)
		/// </summary>
		SPSS_NO_TYPE2 = 33,
		/// <summary>
		/// There is no type7, subtype3 record present. This
		/// code should be regarded as a warning even though
		/// it is positive. Files without this record are valid.
		/// </summary>
		SPSS_NO_TYPE73 = 41,
		/// <summary>
		/// The date variable information is invalid
		/// </summary>
		SPSS_INVALID_DATEINFO = 45,
		/// <summary>
		/// File is not a valid SPSS data file (missing type 999
		/// record)
		/// </summary>
		SPSS_NO_TYPE999 = 46,
		/// <summary>
		/// The value is longer than the length of the variable
		/// </summary>
		SPSS_EXC_STRVALUE = 47,
		/// <summary>
		/// Unable to free because arguments are illegal or
		/// inconsistent (for example, negative numLabels)
		/// </summary>
		SPSS_CANNOT_FREE = 48,
		/// <summary>
		/// Buffer value is too short to hold the value
		/// </summary>
		SPSS_BUFFER_SHORT = 49,
		/// <summary>
		/// Current case is not valid. This may be because no
		/// spssReadCaseRecord calls have been made yet or
		/// because the most recent call failed with error or encountered
		/// the end of file.
		/// </summary>
		SPSS_INVALID_CASE = 50,
		/// <summary>
		/// Internal data structures of the DLL are invalid. This
		/// signals an error in the DLL.
		/// </summary>
		SPSS_INTERNAL_VLABS = 51,
		/// <summary>
		/// File created on an incompatible system.
		/// </summary>
		SPSS_INCOMPAT_APPEND = 52,
		/// <summary>
		/// Undocumented by SPSS.
		/// </summary>
		SPSS_INTERNAL_D_A = 53,
		/// <summary>
		/// Error accessing the temporary file
		/// </summary>
		SPSS_FILE_BADTEMP = 54,
		/// <summary>
		/// spssGetDEWFirst was never called
		/// </summary>
		SPSS_DEW_NOFIRST = 55,
		/// <summary>
		/// measureLevel is not in the legal range, or is
		/// SPSS_MLVL_RAT and the variable is a string variable.
		/// </summary>
		SPSS_INVALID_MEASURELEVEL = 56,
		/// <summary>
		/// Parameter subtype not between 1 and
		/// MAX7SUBTYPE
		/// </summary>
		SPSS_INVALID_7SUBTYPE = 57,

		/// <summary>
		/// Existing multiple response set definitions are invalid
		/// </summary>
		SPSS_INVALID_MRSETDEF = 70,
		/// <summary>
		/// The multiple response set name is invalid
		/// </summary>
		SPSS_INVALID_MRSETNAME = 71,
		/// <summary>
		/// The multiple response set name is a duplicate
		/// </summary>
		SPSS_DUP_MRSETNAME = 72,
		/// <summary>
		/// Undocumented by SPSS.
		/// </summary>
		SPSS_BAD_EXTENSION = 73,
		#endregion

		#region Warning codes returned by functions

		/// <summary>
		/// Label length exceeds 64, truncated and used (warning)
		/// </summary>
		SPSS_EXC_LEN64 = -1,

		/// <summary>
		/// Variable label’s length exceeds 120, truncated and
		/// used (warning)
		/// </summary>
		SPSS_EXC_LEN120 = -2,

		/// <summary>
		/// ... (warning)
		/// </summary>
		SPSS_EXC_VARLABEL = -2,

		/// <summary>
		/// Label length exceeds 60, truncated and used (warning)
		/// </summary>
		SPSS_EXC_LEN60 = -4,

		/// <summary>
		/// ... (warning)
		/// </summary>
		SPSS_EXC_VALLABEL = -4,

		/// <summary>
		/// End of the file reached, no more cases (warning)
		/// </summary>
		SPSS_FILE_END = -5,

		/// <summary>
		/// There is no variable sets information in the file (warning)
		/// </summary>
		SPSS_NO_VARSETS = -6,

		/// <summary>
		/// The variable sets information is empty (warning)
		/// </summary>
		SPSS_EMPTY_VARSETS = -7,

		/// <summary>
		/// The variable has no labels (warning) (warning)
		/// </summary>
		SPSS_NO_LABELS = -8,

		/// <summary>
		/// There is no label for the given value (warning)
		/// </summary>
		SPSS_NO_LABEL = -9,

		/// <summary>
		/// A case weight variable has not been defined for this file (warning)
		/// </summary>
		SPSS_NO_CASEWGT = -10,

		/// <summary>
		/// There is no TRENDS date variable information in the file (warning)
		/// </summary>
		SPSS_NO_DATEINFO = -11,

		/// <summary>
		/// No definitions on the file (warning)
		/// </summary>
		SPSS_NO_MULTRESP = -12,

		/// <summary>
		/// The string contains no definitions (warning)
		/// </summary>
		SPSS_EMPTY_MULTRESP = -13,

		/// <summary>
		/// File contains no DEW information (warning)
		/// </summary>
		SPSS_NO_DEW = -14,

		/// <summary>
		/// Zero bytes to be written (warning)
		/// </summary>
		SPSS_EMPTY_DEW = -15,
		#endregion
	}

	/// <summary>
	/// Missing Value Type codes
	/// </summary>
	public enum MissingValueFormatCode : int 
	{
		/// <summary>
		/// Indicates that no discrete missing values will be defined.
		/// </summary>
		SPSS_NO_MISSVAL = 0,

		/// <summary>
		/// Indicates that 1 discrete missing values will be defined.
		/// </summary>
		SPSS_ONE_MISSVAL = 1,

		/// <summary>
		/// Indicates that 2 discrete missing values will be defined.
		/// </summary>
		SPSS_TWO_MISSVAL = 2,

		/// <summary>
		/// Indicates that 3 discrete missing values will be defined.
		/// </summary>
		SPSS_THREE_MISSVAL = 3,

		/// <summary>
		/// missingVal1 and missingVal2 are taken as the upper and lower limits, 
		/// respectively, of the range, and missingVal3 is ignored
		/// </summary>
		SPSS_MISS_RANGE = -2,

		/// <summary>
		/// missingval1 and missingVal2 are taken as limits of the range and missingVal3 is taken
		/// as the discrete missing value.
		/// </summary>
		SPSS_MISS_RANGEANDVAL = -3,
	}

	/// <summary>
	/// Format Type codes
	/// </summary>
	public enum FormatTypeCode : int 
	{
		/// <summary>
		/// Alphanumeric
		/// </summary>
		SPSS_FMT_A = 1,

		/// <summary>
		/// Alphanumeric hexadecimal
		/// </summary>
		SPSS_FMT_AHEX = 2,

		/// <summary>
		/// F Format with commas
		/// </summary>
		SPSS_FMT_COMMA = 3,

		/// <summary>
		/// Commas and floating dollar sign
		/// </summary>
		SPSS_FMT_DOLLAR = 4,

		/// <summary>
		/// Default Numeric Format
		/// </summary>
		SPSS_FMT_F = 5,

		/// <summary>
		/// Int16 binary
		/// </summary>
		SPSS_FMT_IB = 6,

		/// <summary>
		/// Positive Int16 binary - hex
		/// </summary>
		SPSS_FMT_PIBHEX = 7,

		/// <summary>
		/// Packed decimal
		/// </summary>
		SPSS_FMT_P = 8,

		/// <summary>
		/// Positive Int16 binary unsigned
		/// </summary>
		SPSS_FMT_PIB = 9,

		/// <summary>
		/// Positive Int16 binary unsigned
		/// </summary>
		SPSS_FMT_PK = 10,

		/// <summary>
		/// Floating poInt32 binary
		/// </summary>
		SPSS_FMT_RB = 11,

		/// <summary>
		/// Floating poInt32 binary hex
		/// </summary>
		SPSS_FMT_RBHEX = 12,

		/// <summary>
		/// Zoned decimal
		/// </summary>
		SPSS_FMT_Z = 15,

		/// <summary>
		/// N Format- unsigned with leading 0s
		/// </summary>
		SPSS_FMT_N = 16,

		/// <summary>
		/// E Format- with explicit power of 10
		/// </summary>
		SPSS_FMT_E = 17,

		/// <summary>
		/// Date format dd-mmm-yyyy
		/// </summary>
		SPSS_FMT_DATE = 20,

		/// <summary>
		/// Time format hh:mm:ss.s
		/// </summary>
		SPSS_FMT_TIME = 21,

		/// <summary>
		/// Date and Time
		/// </summary>
		SPSS_FMT_DATE_TIME = 22,

		/// <summary>
		/// Date format dd-mmm-yyyy
		/// </summary>
		SPSS_FMT_ADATE = 23,

		/// <summary>
		/// Julian date - yyyyddd
		/// </summary>
		SPSS_FMT_JDATE = 24,

		/// <summary>
		/// Date-time dd hh:mm:ss.s
		/// </summary>
		SPSS_FMT_DTIME = 25,

		/// <summary>
		/// Day of the week
		/// </summary>
		SPSS_FMT_WKDAY = 26,

		/// <summary>
		/// Month
		/// </summary>
		SPSS_FMT_MONTH = 27,

		/// <summary>
		/// mmm yyyy
		/// </summary>
		SPSS_FMT_MOYR = 28,

		/// <summary>
		/// q Q yyyy
		/// </summary>
		SPSS_FMT_QYR = 29,

		/// <summary>
		/// ww WK yyyy
		/// </summary>
		SPSS_FMT_WKYR = 30,

		/// <summary>
		/// Percent - F followed by %
		/// </summary>
		SPSS_FMT_PCT = 31,

		/// <summary>
		/// Like COMMA, switching dot for comma
		/// </summary>
		SPSS_FMT_DOT = 32,

		/// <summary>
		/// User Programmable currency format
		/// </summary>
		SPSS_FMT_CCA = 33,

		/// <summary>
		/// User Programmable currency format
		/// </summary>
		SPSS_FMT_CCB = 34,

		/// <summary>
		/// User Programmable currency format
		/// </summary>
		SPSS_FMT_CCC = 35,

		/// <summary>
		/// User Programmable currency format
		/// </summary>
		SPSS_FMT_CCD = 36,

		/// <summary>
		/// User Programmable currency format
		/// </summary>
		SPSS_FMT_CCE = 37,

		/// <summary>
		/// Date in dd/mm/yyyy style
		/// </summary>
		SPSS_FMT_EDATE = 38,

		/// <summary>
		/// Date in yyyy/mm/dd style
		/// </summary>
		SPSS_FMT_SDATE = 39,
	}

	/// <summary>
	/// Measurement Level codes
	/// </summary>
	public enum MeasurementLevelCode : int
	{
		/// <summary>
		/// Unknown
		/// </summary>
		SPSS_MLVL_UNK = 0,

		/// <summary>
		/// Nominal 
		/// </summary>
		SPSS_MLVL_NOM = 1,

		/// <summary>
		/// Ordinal
		/// </summary>
		SPSS_MLVL_ORD = 2,

		/// <summary>
		/// Scale (Ratio)
		/// </summary>
		SPSS_MLVL_RAT = 3,
	}
	/// <summary>
	/// Alignment codes
	/// </summary>
	public enum AlignmentCode : int
	{
		/// <summary>
		/// Left aligned
		/// </summary>
		SPSS_ALIGN_LEFT = 0,

		/// <summary>
		/// Right aligned
		/// </summary>
		SPSS_ALIGN_RIGHT = 1,

		/// <summary>
		/// Centered
		/// </summary>
		SPSS_ALIGN_CENTER = 2,
	}
	/// <summary>
	/// Diagnostics regarding var names
	/// </summary>
	public enum VarNameDiagnostic : int
	{
		/// <summary>
		/// Valid standard name
		/// </summary>
		SPSS_NAME_OK = 0,

		/// <summary>
		/// Valid scratch var name
		/// </summary>
		SPSS_NAME_SCRATCH = 1,

		/// <summary>
		/// Valid system var name
		/// </summary>
		SPSS_NAME_SYSTEM = 2,

		/// <summary>
		/// Empty or longer than SPSS_MAX_VARNAME
		/// </summary>
		SPSS_NAME_BADLTH = 3,

		/// <summary>
		/// Invalid character or imbedded blank
		/// </summary>
		SPSS_NAME_BADCHAR = 4,

		/// <summary>
		/// Name is a reserved word
		/// </summary>
		SPSS_NAME_RESERVED = 5,

		/// <summary>
		/// Invalid initial character
		/// </summary>
		SPSS_NAME_BADFIRST = 6,
	}
	/// <summary>
	/// Definitions of "type 7" records
	/// </summary>
	public enum Type7Record : int 
	{
		/// <summary>
		/// Documents (actually type 6
		/// </summary>
		SPSS_T7_DOCUMENTS = 0,

		/// <summary>
		/// VAX Data Entry - dictionary version
		/// </summary>
		SPSS_T7_VAXDE_DICT = 1,

		/// <summary>
		/// VAX Data Entry - data
		/// </summary>
		SPSS_T7_VAXDE_DATA = 2,

		/// <summary>
		/// Source system characteristics
		/// </summary>
		SPSS_T7_SOURCE = 3,

		/// <summary>
		/// Source system floating pt constants
		/// </summary>
		SPSS_T7_HARDCONST = 4,

		/// <summary>
		/// Variable sets
		/// </summary>
		SPSS_T7_VARSETS = 5,

		/// <summary>
		/// Trends date information
		/// </summary>
		SPSS_T7_TRENDS = 6,

		/// <summary>
		/// Multiple response groups
		/// </summary>
		SPSS_T7_MULTRESP = 7,

		/// <summary>
		/// Windows Data Entry data
		/// </summary>
		SPSS_T7_DEW_DATA = 8,

		/// <summary>
		/// TextSmart data
		/// </summary>
		SPSS_T7_TEXTSMART = 10,

		/// <summary>
		/// Msmt level, col width, &amp; alignment
		/// </summary>
		SPSS_T7_MSMTLEVEL = 11,

		/// <summary>
		/// Windows Data Entry GUID
		/// </summary>
		SPSS_T7_DEW_GUID = 12,

		/// <summary>
		/// Extended variable names
		/// </summary>
		SPSS_T7_XVARNAMES = 13,
	}

	#endregion

	/// <summary>
	/// A very thin SpssSafeWrapper that provides access to all the functions 
	/// exposed from the SpssIo32.dll library provided with SPSS 12.
	/// </summary>
	// <remarks>
	// <para>Each method in SPSSIO32.DLL is represented exactly once by this class.</para>
	// <para>Those methods that require the use of pointers for memory management 
	// reasons are marked <c>unsafe</c>, and have <c>protected</c> access for 
	// proper handling by a descending class.  
	// Those methods that require string marshaling are marked <c>protected</c>,
	// because they require special string buffers to be created and trimmed for
	// correct access.  
	// Both types of protected methods should be wrapped and appropriately 
	// handled by a <see cref="SpssSafeWrapper"/>descending class</see>.</para>
	// </remarks>
	public class SpssThinWrapper
	{
		#region Maximum lengths of SPSS data file objects

		/// <summary>
		/// Maximum length of a variable name
		/// </summary>
		public const int SPSS_MAX_VARNAME = 64;  

		/// <summary>
		/// Short (compatibility) variable name.
		/// </summary>
		public const int SPSS_MAX_SHORTVARNAME = 8;

		/// <summary>
		/// Maximum length of a short string variable
		/// </summary>
		public const int SPSS_MAX_SHORTSTRING = 8;  

		/// <summary>
		/// Maximum length of a file label string
		/// </summary>
		public const int SPSS_MAX_IDSTRING = 64 ; 

		/// <summary>
		/// Maximum length of a long string variable
		/// </summary>
		public const int SPSS_MAX_LONGSTRING = 32767;

		/// <summary>
		/// Maximum length of a value label
		/// </summary>
		public const int SPSS_MAX_VALLABEL = 60;  

		/// <summary>
		/// Maximum length of a variable label
		/// </summary>
		public const int SPSS_MAX_VARLABEL = 256;   

		/// <summary>
		/// Maximum record 7 subtype
		/// </summary>
		public const int SPSS_MAX_7SUBTYPE = 32;
		#endregion

		/// <summary>
		/// Closes an SPSS file that was opened using <see cref="spssOpenAppend"/>.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>, 
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_WERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function closes the data file associated with handle, which must have been opened
		/// for appending cases using <see cref="spssOpenAppend"/>. The file handle handle becomes invalid
		/// and no further operations can be performed using it.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCloseAppend@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCloseAppend(int handle);
 
		/// <summary>
		/// Closes an SPSS file that was opened using <see cref="spssOpenRead"/>.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>.
		/// </returns>
		/// <remarks>
		/// This function closes the data file associated with handle, which must have been opened
		/// for reading using <see cref="spssOpenRead"/>. The file handle handle becomes 
		/// invalid and no further operations can be performed using it.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCloseRead@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCloseRead(int handle);
 
		/// <summary>
		/// Closes an SPSS file that was opened using <see cref="spssOpenWrite"/>.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_WERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function closes the data file associated with handle, which must have been opened
		/// for writing using <see cref="spssOpenWrite"/>. The file handle handle becomes invalid
		/// and no further operations can be performed using it.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCloseWrite@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCloseWrite(int handle);
 
		/// <summary>
		/// Writes a case to an SPSS data file after its values have been set.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_WERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function writes a case to the data file specified by the handle. It must be called after
		/// setting the values of variables through <see cref="spssSetValueNumeric"/> and 
		/// <see cref="spssSetValueChar"/>.
		/// Any variables left unset will get the system-missing value if they are numeric and all
		/// blanks if they are strings. Unless spssCommitCaseRecord is called, the case will not be
		/// written out.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCommitCaseRecord@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCommitCaseRecord(int handle);
 
		/// <summary>
		/// Writes the data dictionary to the data file.  To be used after dictionary defining 
		/// functions are called, and before cases are written.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>,
		/// <see cref="ReturnCode.SPSS_DICT_EMPTY"/>,
		/// <see cref="ReturnCode.SPSS_FILE_WERROR"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>, or
		/// <see cref="ReturnCode.SPSS_INTERNAL_VLABS"/>.
		/// </returns>
		/// <remarks>
		/// This function writes the data dictionary to the data file associated with handle. Before
		/// any case data can be written, the dictionary must be committed; once the dictionary has
		/// been committed, no further changes can be made to it.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCommitHeader@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCommitHeader(int handle);
 
		/// <summary>
		/// Converts a day/month/year into SPSS format.
		/// </summary>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_DATE"/>.
		/// </returns>
		/// <remarks>
		/// This function converts a Gregorian date expressed as day-month-year to the internal
		/// SPSS date format. The time portion of the date variable is set to 0:00. To set the time
		/// portion of the date variable to another value, use spssConvertTime and add the resulting
		/// value to *spssDate. Dates before October 15, 1582, are considered invalid.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssConvertDate@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssConvertDate(int day, int month, int year, out double spssDate);
 
		/// <summary>
		/// Extracts the date from an SPSS date field.
		/// </summary>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_DATE"/>.
		/// </returns>
		/// <remarks>
		/// This function converts the date (as distinct from time) portion of a value in internal SPSS
		/// date format to Gregorian style.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssConvertSPSSDate@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssConvertSPSSDate(out int day, out int month, out int year, double spssDate);
 
		/// <summary>
		/// Extracts the time of day from an SPSS date field.
		/// </summary>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_TIME"/>.
		/// </returns>
		/// <remarks>
		/// This function breaks a value in internal SPSS date format into a day number (since October
		/// 14, 1582) plus the hour, minute, and second values. Note that the seconds value is
		/// stored in a double since it may have a fractional part.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssConvertSPSSTime@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssConvertSPSSTime(out int day, out int hourh, out int minute, out double second, double spssDate);
 
		/// <summary>
		/// Converts a time of day to the SPSS time format.
		/// </summary>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_TIME"/>.
		/// </returns>
		/// <remarks>
		/// This function converts a time given as day, hours, minutes, and seconds to the internal
		/// SPSS format. The day value is the number of days since October 14, 1582, and is typically
		/// zero, especially when this function is used in conjunction with spssConvertDate.
		/// Note that the seconds value is stored in a double since it may have a fractional part.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssConvertTime@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssConvertTime(int day, int hour, int minute, double second, out double spssTime);
 
		/// <summary>
		/// Copies stored documents from one file to another.
		/// </summary>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>, or
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>.
		/// </returns>
		/// <remarks>
		/// This function copies stored documents, if any, from the file associated with fromHandle
		/// to that associated with toHandle. The latter must be open for output. If the target file already
		/// has documents, they are discarded. If the source file has no documents, the target
		/// will be set to have none, too.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssCopyDocuments@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssCopyDocuments(int fromHandle, int toHandle);

		/// <summary>
		/// Frees memory allocated by <see cref="spssGetDateVariables"/>.
		/// </summary>
		/// <param name="dateInfo">
		/// Vector of date variable indexes.
		/// </param>
		/// <returns>
		/// Always returns <see cref="ReturnCode.SPSS_OK"/> indicating success.
		/// </returns>
		/// <remarks>
		/// This function is called to return the memory allocated by <see cref="spssGetDateVariables"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeDateVariables@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeDateVariables(int* dateInfo);
 
		/// <summary>
		/// Frees memory allocated by <see cref="spssGetMultRespDefs"/>.
		/// </summary>
		/// <param name="mrespDefs">
		/// ASCII string containing the definitions.
		/// </param>
		/// <returns>
		/// The function always succeeds and always returns <see cref="ReturnCode.SPSS_OK"/>.
		/// </returns>
		/// <remarks>
		/// This function releases the memory which was acquired by <see cref="spssGetMultRespDefs"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeMultRespDefs@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeMultRespDefs(char* mrespDefs);
	
		/// <summary>
		/// Frees memory allocated by <see cref="spssGetVarCValueLabels"/>.
		/// </summary>
		/// <param name="values">
		/// Array of pointers to values returned by <see cref="spssGetVarCValueLabels"/>
		/// </param>
		/// <param name="labels">
		/// Array of pointers to labels returned by <see cref="spssGetVarCValueLabels"/>
		/// </param>
		/// <param name="numLabels">
		/// Number of values or labels returned by <see cref="spssGetVarCValueLabels"/>
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_CANNOT_FREE"/>
		/// </returns>
		/// <remarks>
		/// This function frees the two arrays and the value and label strings allocated on the heap
		/// by <see cref="spssGetVarCValueLabels"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeVarCValueLabels", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeVarCValueLabels(char** values, char** labels, int numLabels);

		/// <summary>
		/// Frees memory allocated by <see cref="spssGetVariableSets"/>.
		/// </summary>
		/// <param name="varSets">
		/// The string defining the variable sets
		/// </param>
		/// <returns>
		/// Always returns <see cref="ReturnCode.SPSS_OK"/> indicating success.
		/// </returns>
		/// <remarks>
		/// This function is called to return the memory allocated by <see cref="spssGetVariableSets"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeVariableSets@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeVariableSets(char* varSets);

		/// <summary>
		/// Frees memory allocated by <see cref="spssGetVarNames"/>.
		/// </summary>
		/// <param name="varNames">
		/// Array of pointers to names returned by <see cref="spssGetVarNames"/>
		/// </param>
		/// <param name="varTypes">
		/// Array of variable types returned by <see cref="spssGetVarNames"/>
		/// </param>
		/// <param name="numVars">
		/// Number of variables returned by <see cref="spssGetVarNames"/>
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_CANNOT_FREE"/>.
		/// </returns>
		/// <remarks>
		/// This function frees the two arrays and the name strings allocated on the heap by
		/// <see cref="spssGetVarNames"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeVarNames", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeVarNames(char** varNames, int* varTypes, int numVars);

		/// <summary>
		/// Frees memory allocated by <see cref="spssGetVarNValueLabels"/>.
		/// </summary>
		/// <param name="values">
		/// Array of values returned by <see cref="spssGetVarNValueLabels"/>
		/// </param>
		/// <param name="labels">
		/// Array of pointers to labels returned by <see cref="spssGetVarNValueLabels"/>
		/// </param>
		/// <param name="numLabels">
		/// Number of values or labels returned by <see cref="spssGetVarNValueLabels"/>
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_CANNOT_FREE"/>
		/// </returns>
		/// <remarks>
		/// This function frees the two arrays and the value and label strings allocated on the heap
		/// by <see cref="spssGetVarCValueLabels"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssFreeVarNValueLabels", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssFreeVarNValueLabels(double* values, char** labels, int numLabels);

		/// <summary>
		/// Reports the number of bytes taken up by any one case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseSize">
		/// Pointer to size of case in bytes
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the size of a raw case record for the file associated with handle.
		/// The case size is reported in bytes and is meant to be used in conjunction with the lowlevel
		/// case input/output procedures <see cref="spssWholeCaseIn"/> and 
		/// <see cref="spssWholeCaseOut"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetCaseSize@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetCaseSize(int handle, out int caseSize);
 
		/// <summary>
		/// Reports the name of the case weight variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Pointer to the buffer to hold name of the case weight variable
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_CASEWGT"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_CASEWGT"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the name of the case weight variable. The name is copied to the
		/// buffer pointed to by varName as a null-terminated string. Since a variable name can be
		/// up to <see cref="SPSS_MAX_VARNAME"/> characters in length, the size of the buffer 
		/// must be at least <see cref="SPSS_MAX_VARNAME"/>+1.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetCaseWeightVar@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetCaseWeightVar(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName);

		/// <summary>
		/// Gets the compression attribute of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="compSwitch">
		/// Pointer to compression attribute. Upon return, *compSwitch is 1 if
		/// the file is compressed; 0 otherwise.
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the compression attribute of an SPSS data file.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetCompression@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetCompression(int handle, out int compSwitch);
 
		/// <summary>
		/// Reports TRENDS date variable information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="numofElements">
		/// Number of elements in allocated array
		/// </param>
		/// <param name="dateInfo">
		/// Pointer to first element of the allocated array
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
		/// expected to free the array by calling <see cref="spssFreeDateVariables"/> when it is no longer needed.
		/// The variable information is copied directly from record 7, subtype 3. Its first six elements
		/// comprise the "fixed" information, followed by a sequence of one or more three-element
		/// groups.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetDateVariables@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetDateVariables(int handle, out int numofElements, out int* dateInfo);
 
		/// <summary>
		/// Retrieves the first block of SPSS Data Entry information from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="data">
		/// Returned as data from the file
		/// </param>
		/// <param name="maxData">
		/// Maximum bytes to return
		/// </param>
		/// <param name="nData">
		/// Returned as number of bytes returned
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
		[DllImport("spssio32.dll", EntryPoint="spssGetDEWFirst@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetDEWFirst(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string data, int maxData, out int nData);
 
		/// <summary>
		/// Gets the Data Entry GUID from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="asciiGUID">
		/// Returned as the file's GUID in character form or a null string if the
		/// file contains no GUID
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// Data Entry for Windows maintains a GUID in character form as a uniqueness indicator.
		/// Two files have identical dictionaries and DEW information if they have the same
		/// GUID. Note that the spssOpenWriteCopy function will not copy the source file’s
		/// GUID. spssGetDEWGUID allows the client to read a file’s GUID, if any. The client
		/// supplies a 257 byte string in which the null-terminated GUID is returned.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetDEWGUID@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetDEWGUID(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string asciiGUID);
 
		/// <summary>
		/// Gets the length and hash of the Data Entry information in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="Length">
		/// Returned as the length in bytes
		/// </param>
		/// <param name="HashTotal">
		/// Returned as the hash total
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_DEW"/>.
		/// </returns>
		/// <remarks>
		/// This function can be called before actually retrieving DEW information (file
		/// information that is private to the SPSS Data Entry product) from a file, to obtain some
		/// attributes of that information--specifically its length in bytes and its hash total. The
		/// hash total is, by convention, contained in the last four bytes to be written. Because it is
		/// not cognizant of the structure of the DEW information, the I/O DLL is unable to correct
		/// the byte order of numeric information generated on a foreign host. As a result, the
		/// DEW information is discarded if the file has a byte order that is the reverse of that of
		/// the host, and calls to spssGetDEWInfo will return SPSS_NO_DEW.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetDEWInfo@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetDEWInfo(int handle, out int Length, out int HashTotal);
 
		/// <summary>
		/// Retrieves the next block of SPSS Data Entry information from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="data">
		/// Returned as data from the file
		/// </param>
		/// <param name="maxData">
		/// Maximum bytes to return
		/// </param>
		/// <param name="nData">
		/// Returned as number of bytes returned
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
		/// <see cref="spssGetDEWInfo"/>, <see cref="spssGetDEWFirst"/> will return SPSS_NO_DEW if the file was written
		/// with a byte order that is the reverse of that of the host.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetDEWNext@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetDEWNext(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string data, int maxData, out int nData);

		/// <summary>
		/// Estimates number of cases in a data file.  
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseCount">
		/// Returned as estimated n of cases
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>.
		/// </returns>
		/// <remarks>
		/// Although not strictly required for direct access input, this function helps in reading
		/// SPSS data files from releases earlier than 6.0. Some of these data files did not contain
		/// number of cases information, and <see cref="spssGetNumberofCases"/> will return -1 cases. This
		/// function will return a precise number for uncompressed files and an estimate (based on
		/// overall file size) for compressed files. It cannot be used on files open for appending data.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetEstimatedNofCases@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetEstimatedNofCases(int handle, out int caseCount);
 
		/// <summary>
		/// Gets the number of cases in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseCount">
		/// Pointer to number of cases
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the number of cases present in a data file open for reading.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetNumberofCases@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetNumberofCases(int handle, out int caseCount);

		/// <summary>
		/// Gets the file label of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="id">
		/// File label buffer
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function copies the file label of the SPSS data file associated with <paramref>handle</paramref> into the
		/// buffer pointed to by id. The label is at most 64 characters long and null-terminated.
		/// Thus, the size of the buffer should be at least 65. If an input data file is associated with
		/// the handle, the label will be exactly 64 characters long, padded with blanks as
		/// necessary.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetIdString@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetIdString(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string id);
 
		/// <summary>
		/// Retrieves the definitions from a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="mrespDefs">
		/// Returned as a pointer to a string
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
		/// by calling <see cref="spssFreeMultRespDefs"/>. If the file contains no multiple response definitions,
		/// *mrespDefs is set to NULL, and the function returns the warning code
		/// <see cref="ReturnCode.SPSS_NO_MULTRESP"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetMultRespDefs@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetMultRespDefs(int handle, out char* mrespDefs);
 
		/// <summary>
		/// Gets the number of variables in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="numVars">
		/// Pointer to number of variables
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the number of variables present in a data file.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetNumberofVariables@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetNumberofVariables(int handle, out int numVars);
 
		/// <summary>
		/// Gets information on the running version of SPSS, and the hosting computer.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="relInfo">
		/// Array of int in which release- and machine-specific data will be
		/// stored. This array must have at least eight elements.
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
		[DllImport("spssio32.dll", EntryPoint="spssGetReleaseInfo@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetReleaseInfo(int handle, [MarshalAs(UnmanagedType.LPArray, SizeConst=8)] int[] relInfo);
 
		/// <summary>
		/// Gets the name of the system that created a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="sysName">
		/// The originating system name
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function returns the name of the system under which the file was created. It is a
		/// 40-byte blank-padded character field corresponding to the last 40 bytes of record type
		/// 1. Thus, in order to accommodate the information, the parameter 
		/// <paramref>sysName</paramref> must be at least 41 bytes in length plus the 
		/// terminating null character.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetSystemString@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetSystemString(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string sysName);
 
		/// <summary>
		/// Gets the data created by TextSmart.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="textInfo">
		/// Buffer for text data
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function places the text data created by TextSmart as a null-terminated string in
		/// the user-supplied buffer <paramref>textInfo</paramref>. The buffer is assumed to be 
		/// at least 256 characters long; the text data may be up to 255 characters long. 
		/// If text data are not present in the file, the first character in 
		/// <paramref>textInfo</paramref> is set to NULL.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetTextInfo@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetTextInfo(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string textInfo);

		/// <summary>
		/// Gets the creation date of a data file, as recorded in the file itself.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="fileDate">
		/// File creation date
		/// </param>
		/// <param name="fileTime">
		/// File creation time
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/> or
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>.
		/// </returns>
		/// <remarks>
		/// This function returns the creation date of the file as recorded in the file itself. The creation
		/// date is a null-terminated 9-byte character field in dd mmm yy format (27 Feb 96),
		/// and the receiving field must be at least 10 bytes in length. The creation time is a nullterminated
		/// 8-byte character field in hh:mm:ss format (13:12:15), and the receiving field
		/// must be at least 9 bytes in length.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetTimeStamp@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetTimeStamp(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string fileDate, [MarshalAs(UnmanagedType.VBByRefStr)] ref string fileTime);
 
		/// <summary>
		/// Gets the string value of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varHandle">
		/// Handle of the variable
		/// </param>
		/// <param name="value">
		/// Buffer for the value of the string variable
		/// </param>
		/// <param name="valueSize">
		/// Size of value
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
		/// read by the most recent call to <see cref="spssReadCaseRecord"/>. 
		/// The value is returned as a null-terminated
		/// string in the caller-provided buffer value; the length of the string is the length
		/// of the string variable. Argument valueSize is the allocated size of the buffer value,
		/// which must be at least the length of the variable plus 1.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetValueChar@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetValueChar(int handle, double varHandle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string value, int valueSize);

		/// <summary>
		/// Gets the numeric value of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varHandle">
		/// Handle of the variable
		/// </param>
		/// <param name="value">
		/// Pointer to the value of the numeric variable
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_CASE"/>, or
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the value of a numeric variable for the current case, which is the case
		/// read by the most recent call to <see cref="spssReadCaseRecord"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetValueNumeric@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssGetValueNumeric(int handle, double varHandle, out double value);
 
		/// <summary>
		/// Gets the alignment of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="alignment">
		/// Pointer to alignment. 
		/// Set to <see cref="AlignmentCode.SPSS_ALIGN_LEFT"/>,
		/// <see cref="AlignmentCode.SPSS_ALIGN_RIGHT"/>, or 
		/// <see cref="AlignmentCode.SPSS_ALIGN_CENTER"/>
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
		[DllImport("spssio32.dll", EntryPoint="spssGetVarAlignment@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarAlignment(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out AlignmentCode alignment);
 
		/// <summary>
		/// Gets the missing values of a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="missingFormat">
		/// Pointer to missing value format code
		/// </param>
		/// <param name="missingVal1">
		/// Buffer for first missing value
		/// </param>
		/// <param name="missingVal2">
		/// Buffer for second missing value
		/// </param>
		/// <param name="missingVal3">
		/// Buffer for third missing value
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
		/// *<paramref>missingFormat</paramref> will be in the range 0–3, indicating 
		/// the number of missing values. The
		/// appropriate number of missing values is copied to the buffers <paramref>missingVal1</paramref>,
		/// <paramref>missingVal2</paramref>, and <paramref>missingVal3</paramref>. 
		/// The lengths of the null-terminated missing value strings
		/// will be the length of the short string variable in question. Since the latter can be at most
		/// 8 characters long, 9-character buffers are adequate for any short string variable.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarCMissingValues@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarCMissingValues(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out MissingValueFormatCode missingFormat, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal1, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal2, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal3);
 
		/// <summary>
		/// Gets the width of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="columnWidth">
		/// Pointer to column width. Non-negative
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
		[DllImport("spssio32.dll", EntryPoint="spssGetVarColumnWidth@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarColumnWidth(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out int columnWidth);
 
		/// <summary>
		/// Gets the value label for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="value">
		/// Short string value for which the label is wanted
		/// </param>
		/// <param name="label">
		/// Label for the value
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
		/// This function gets the value label for a given value of a short string variable. The label is
		/// copied as a null-terminated string into the buffer label, whose size must be at least 61 to
		/// hold the longest possible value label (60 characters plus the null terminator). To get value
		/// labels more than 60 characters long, use the <see cref="spssGetVarCValueLabelLong"/> function.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarCValueLabel@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarCValueLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string label);
 
		/// <summary>
		/// Gets a value label of a string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Null-terminated variable name
		/// </param>
		/// <param name="value">
		/// Null-terminated value for which label is requested
		/// </param>
		/// <param name="labelBuff">
		/// Returned as null-terminated label
		/// </param>
		/// <param name="lenBuff">
		/// Overall size of labelBuff in bytes
		/// </param>
		/// <param name="lenLabel">
		/// Returned as bytes stored excluding terminator
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
		/// This function returns a null-terminated value label corresponding to one value of a
		/// specified variable whose values are short strings. The function permits the client to
		/// limit the number of bytes (including the null terminator) stored and returns the number
		/// of data bytes (excluding the null terminator) actually stored. If an error is detected, the
		/// label is returned as a null string, and the length is returned as 0.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarCValueLabelLong@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarCValueLabelLong(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string labelBuff, int lenBuff, out int lenLabel);
 
		/// <summary>
		/// Gets the list of response values and labels for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="values">
		/// Pointer to array of pointers to values
		/// </param>
		/// <param name="labels">
		/// Pointer to array of pointers to labels
		/// </param>
		/// <param name="numLabels">
		/// Pointer to number of values or labels
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
		/// are no longer needed, <see cref="spssFreeVarCValueLabels"/> should be called to free the memory.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarCValueLabels", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetVarCValueLabels(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out char** values, out char** labels, out int numLabels);
		
		/// <summary>
		/// Gets the handle for a named variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="varHandle">
		/// Pointer to handle for the variable. Note that the variable handle is
		/// a double, and not int or long.
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
		/// with an output file, the dictionary must be written with <see cref="spssCommitHeader"/> 
		/// before variable handles can be obtained via spssGetVarHandle.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarHandle@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarHandle(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out double varHandle);
 
		/// <summary>
		/// Gets variable sets information in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varSets">
		/// Pointer to pointer to variable sets string
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_VARSETS"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the variable sets information in the data file. Variable sets
		/// information is stored in a null-terminated string and a pointer to the string is returned
		/// in *<paramref>varSets</paramref>. Since the variable sets string is allocated 
		/// on the heap, the caller should free
		/// it by calling <see cref="spssFreeVariableSets"/> when it is no longer needed.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVariableSets@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetVariableSets(int handle, out char* varSets);
 
		/// <summary>
		/// Gets the name and type of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="iVar">
		/// Zero origin variable number
		/// </param>
		/// <param name="varName">
		/// Returned as the variable name
		/// </param>
		/// <param name="varType">
		/// Returned as the variable type
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
		/// at a time and, therefore, can be passed to a Visual Basic program. The storage to receive
		/// the variable name must be at least 9 bytes in length because the name is returned as a
		/// null-terminated string. The type code is an integer in the range 0–255, 0 indicating a numeric
		/// variable and a positive value indicating a string variable of that size.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarInfo@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarInfo(int handle, int iVar, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out int varType);
 
		/// <summary>
		/// Gets the variable label for some named variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="varLabel">
		/// Variable label buffer
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABEL"/>, 
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function copies the label of variable varName into the buffer pointed to by
		/// varLabel. Since the variable label is at most 120 characters long and null terminated, the
		/// size of the buffer should be at least 121. To get labels more than 120 characters long, use
		/// the spssGetVarLabelLong function.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarLabel@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] out string varLabel);
 
		/// <summary>
		/// Gets the variable label for some named variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Null-terminated variable name
		/// </param>
		/// <param name="labelBuff">
		/// Buffer to receive the null-terminated label
		/// </param>
		/// <param name="lenBuff">
		/// Overall size of labelBuff in bytes
		/// </param>
		/// <param name="lenLabel">
		/// Returned as bytes stored excluding terminator
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_NO_LABEL"/>, 
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function returns the null-terminated label associated with the specified variable but
		/// restricts the number of bytes (including the null terminator) returned to lenBuff bytes.
		/// This length can be conveniently specified as sizeof(labelBuff). The function also returns
		/// the number of data bytes (this time excluding the null terminator) stored. If an error is
		/// detected, the label is returned as a null string, and the length is returned as 0.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarLabelLong@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarLabelLong(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string labelBuff, int lenBuff, out int lenLabel);

		/// <summary>
		/// Gets the measurement level of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="measureLevel">
		/// Pointer to measurement level. Set to 
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_NOM"/>,
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_ORD"/>, or 
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_RAT"/>, 
		/// for nominal, ordinal, and scale (ratio), respectively
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
		[DllImport("spssio32.dll", EntryPoint="spssGetVarMeasureLevel@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarMeasureLevel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out int measureLevel);
 
		/// <summary>
		/// Gets the missing values of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="missingFormat">
		/// Pointer to missing value format code
		/// </param>
		/// <param name="missingVal1">
		/// Buffer for first missing value
		/// </param>
		/// <param name="missingVal2">
		/// Buffer for second missing value
		/// </param>
		/// <param name="missingVal3">
		/// Buffer for third missing value
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
		/// *<paramref>missingFormat</paramref> determines the interpretation of *<paramref>missingVal1</paramref>, *<paramref>missingVal2</paramref>, and
		/// *<paramref>missingVal3</paramref>. If *missingFormat is SPSS_MISS_RANGE, *<paramref>missingVal1</paramref> and *<paramref>missingVal2</paramref>
		/// represent the upper and lower limits, respectively, of the range, and *<paramref>missingVal3</paramref> is not
		/// used. If *missingFormat is SPSS_MISS_RANGEANDVAL, *<paramref>missingVal1</paramref> and *<paramref>missingVal2</paramref>
		/// represent the range and *<paramref>missingVal3</paramref> is the discrete missing value. If *missingFormat is
		/// neither of the above, it will be in the range 0–3, indicating the number of discrete missing
		/// values present. (The macros SPSS_NO_MISSVAL, SPSS_ONE_MISSVAL,
		/// SPSS_TWO_MISSVAL, and SPSS_THREE_MISSVAL may be used as synonyms for 0–3.)
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarNMissingValues@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarNMissingValues(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out MissingValueFormatCode missingFormat, out double missingVal1, out double missingVal2, out double missingVal3);
 
		/// <summary>
		/// Gets the value label for a given value of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="value">
		/// Numeric value for which the label is wanted
		/// </param>
		/// <param name="label">
		/// Label for the value
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
		/// This function gets the value label for a given value of a numeric variable. The label is
		/// copied as a null-terminated string into the buffer label, whose size must be at least 61 to
		/// hold the longest possible value label (60 characters) plus the terminator. To get value labels
		/// more than 60 characters long, use the <see cref="spssGetVarNValueLabelLong"/> function.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarNValueLabel@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarNValueLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, double value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string label);
 
		/// <summary>
		/// Gets the value label for a given value of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Null-terminated variable name
		/// </param>
		/// <param name="value">
		/// Value for which label is requested
		/// </param>
		/// <param name="labelBuff">
		/// Returned as null-terminated label
		/// </param>
		/// <param name="lenBuff">
		/// Overall size of labelBuff in bytes
		/// </param>
		/// <param name="lenLabel">
		/// Returned as bytes stored excluding terminator
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
		/// This function returns a null-terminated value label corresponding to one value of a specified
		/// numeric variable. It permits the client to limit the number of bytes (including the
		/// null terminator) stored and returns the number of data bytes (excluding the null terminator)
		/// actually stored. If an error is detected, the label is returned as a null string, and the
		/// length is returned as 0.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarNValueLabelLong@28", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarNValueLabelLong(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, double value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string labelBuff, int lenBuff, out int lenLabel);
 
		/// <summary>
		/// Gets a list of all response values and labels for a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="values">
		/// Pointer to array of double values
		/// </param>
		/// <param name="labels">
		/// Pointer to array of pointers to labels
		/// </param>
		/// <param name="numLabels">
		/// Pointer to number of values or labels
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
		/// needed, <see cref="spssFreeVarNValueLabels"/> should be called to free the memory.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarNValueLabels", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetVarNValueLabels(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out double* values, out char** labels, out int numLabels);

		/// <summary>
		/// Gets print formatting information for a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="printType">
		/// Pointer to print format type code (file spssdio.h defines macros of
		/// the form <see cref="FormatTypeCode">SPSS_FMT_...</see> for all valid format type codes)
		/// </param>
		/// <param name="printDec">
		/// Pointer to number of digits after the decimal
		/// </param>
		/// <param name="printWidth">
		/// Pointer to print format width
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_VARNAME"/>, or
		/// <see cref="ReturnCode.SPSS_VAR_NOTFOUND"/>.
		/// </returns>
		/// <remarks>
		/// This function reports the print format of a variable. Format type, number of decimal
		/// places, and field width are returned as *<paramref>printType</paramref>, 
		/// *<paramref>printDec</paramref>, and *<paramref>printWid</paramref>, respectively.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarPrintFormat@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarPrintFormat(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out FormatTypeCode printType, out int printDec, out int printWidth);
 
		/// <summary>
		/// Gets the names and types of all variables in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="numVars">
		/// Pointer to number of variables
		/// </param>
		/// <param name="varNames">
		/// Pointer to array of pointers to variable names
		/// </param>
		/// <param name="varTypes">
		/// Pointer to array of variable types
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>, or
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>.
		/// </returns>
		/// <remarks>
		/// This function gets the names and types of all the variables present in a data file. The
		/// number of variables is returned as *numVars. Variable names are structured as an array
		/// of *numVars pointers, each pointing to a char string containing a variable name, and
		/// *varNames is set to point to the first element of the array. Variable types are stored into
		/// a corresponding array of *numVars in elements, and *varTypes is set to point to the first
		/// element of the array. The type code is an integer in the range 0–255, 0 indicating a numeric
		/// variable and a positive value indicating a string variable of that size.
		/// The two arrays and the variable name strings are allocated on the heap. When they
		/// are no longer needed, <see cref="spssFreeVarNames"/> should be called to free the memory.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarNames", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe protected static extern ReturnCode spssGetVarNames(int handle, out int numVars, out char** varNames, out int* varTypes);
		
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
		/// places, and field width are returned as *writeType, *writeDec, and *writeWid,
		/// respectively.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssGetVarWriteFormat@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssGetVarWriteFormat(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, out FormatTypeCode writeType, out int writeDec, out int writeWidth);

		/// <summary>
		/// Gets the missing value for the system.
		/// </summary>
		/// <param name="missVal">
		/// Returned as the system missing value
		/// </param>
		/// <remarks>
		/// This function accesses the same information as <see cref="spssSysmisVal"/> but returns the
		/// information via a parameter rather than on the stack as the function result. The problem
		/// being addressed is that not all languages return doubles from functions in the same
		/// fashion.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssHostSysmisVal@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern void spssHostSysmisVal(out double missVal);
 
		/// <summary>
		/// Gets the lowest and highest values used for numeric missing values on the system.
		/// </summary>
		/// <param name="lowest">
		/// Pointer to "lowest" value
		/// </param>
		/// <param name="highest">
		/// Pointer to "highest" value
		/// </param>
		/// <remarks>
		/// This function returns the "lowest" and "highest" values used for numeric missing value
		/// ranges on the host system. It may be called at any time.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssLowHighVal@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern void spssLowHighVal(out double lowest, out double highest);
 
		/// <summary>
		/// Opens an SPSS file for appending cases.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file
		/// </param>
		/// <param name="handle">
		/// Pointer to handle to be returned
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
		[DllImport("spssio32.dll", EntryPoint="spssOpenAppend@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssOpenAppend([MarshalAs(UnmanagedType.VBByRefStr)] ref string fileName, out int handle);
 
		/// <summary>
		/// Opens an SPSS file for reading.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file
		/// </param>
		/// <param name="handle">
		/// Pointer to handle to be returned
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
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssOpenRead@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssOpenRead([MarshalAs(UnmanagedType.VBByRefStr)] ref string fileName, out int handle);
 
		/// <summary>
		/// Creates an SPSS file, and prepares for writing.
		/// </summary>
		/// <param name="fileName">
		/// Name of the file
		/// </param>
		/// <param name="handle">
		/// Pointer to handle to be returned
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
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssOpenWrite@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssOpenWrite([MarshalAs(UnmanagedType.VBByRefStr)] ref string fileName, out int handle);
 
		/// <summary>
		/// Creates a new SPSS data file, with a dictionary copied from an existing file.
		/// </summary>
		/// <param name="fileName">
		/// Name of the new file
		/// </param>
		/// <param name="dictFileName">
		/// Name of existing file
		/// </param>
		/// <param name="handle">
		/// Pointer to handle to be returned
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
		[DllImport("spssio32.dll", EntryPoint="spssOpenWriteCopy@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssOpenWriteCopy([MarshalAs(UnmanagedType.VBByRefStr)] ref string fileName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string dictFileName, out int handle);
 
		/// <summary>
		/// Gets a value indicating whether a data file contains a specific "type 7" record.
		/// </summary>
		/// <param name="fromHandle">
		/// Handle to the data file
		/// </param>
		/// <param name="subType">
		/// Specific subtype record
		/// </param>
		/// <param name="bFound">
		/// Returned set if the specified subtype was encountered
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_7SUBTYPE"/>.
		/// </returns>
		/// <remarks>
		/// This function can be used to determine whether a file opened for reading or append
		/// contains a specific "type 7" record. The following type 7 subtypes might be of interest:
		/// <list type="bullet">
		/// <item>Subtype 3. Release information</item>
		/// <item>Subtype 4. Floating point constants including the system missing value</item>
		/// <item>Subtype 5. Variable set definitions</item>
		/// <item>Subtype 6. Date variable information</item>
		/// <item>Subtype 7. Multiple response set definitions</item>
		/// <item>Subtype 8. Data Entry for Windows (DEW) information</item>
		/// <item>Subtype 10. TextSmart information</item>
		/// <item>Subtype 11. Measurement level, column width, and alignment for each variable</item>
		/// </list>
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssQueryType7@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssQueryType7(int fromHandle, int subType, out int bFound);
 
		/// <summary>
		/// Reads the next case from a data file in preparation for reading its values.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_FILE_END"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>, or
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>.
		/// </returns>
		/// <remarks>
		/// This function reads the next case from a data file into internal buffers. Values of individual
		/// variables for the case may then be obtained by calling the 
		/// <see cref="spssGetValueNumeric"/> and
		/// <see cref="spssGetValueChar"/> procedures.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssReadCaseRecord@4", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssReadCaseRecord(int handle);
 
		/// <summary>
		/// Prepares SPSS to read data values from a specific case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseNumber">
		/// Zero origin case number
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_WRMODE"/>,
		/// <see cref="ReturnCode.SPSS_NO_MEMORY"/>,
		/// <see cref="ReturnCode.SPSS_FILE_RERROR"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_FILE"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the file pointer of an input file so that the next data case read will be
		/// the one specified via the caseNumber parameter. A zero origin scheme is used. That
		/// is, the first case is number 0. The next case can be read by calling either 
		/// <see cref="spssWholeCaseIn"/> or <see cref="spssReadCaseRecord"/>. 
		/// If the specified case is greater than or equal to the
		/// number of cases in the file, the call to the input function will return 
		/// <see cref="ReturnCode.SPSS_FILE_END"/>.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSeekNextCase@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssSeekNextCase(int handle, int caseNumber);

		/// <summary>
		/// Sets the case weight variable in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// The name of the case weight variable
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
		[DllImport("spssio32.dll", EntryPoint="spssSetCaseWeightVar@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetCaseWeightVar(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName);
 
		/// <summary>
		/// Sets the compression attribute of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="compSwitch">
		/// Compression switch
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_COMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_INVALID_COMPSW"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the compression attribute of an SPSS data file. Compression is set
		/// on if compSwitch is one and off if it is zero. If this function is not called, the output file
		/// will be uncompressed by default.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetCompression@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssSetCompression(int handle, int compSwitch);
 
		/// <summary>
		/// Sets the Trends date variable information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="numofElements">
		/// Size of the array dateInfo
		/// </param>
		/// <param name="dateInfo">
		/// Array containing date variables information
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
		[DllImport("spssio32.dll", EntryPoint="spssSetDateVariables@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		[CLSCompliant(false)]
		unsafe  protected static extern ReturnCode spssSetDateVariables(int handle, int numofElements, int* dateInfo);
 
		/// <summary>
		/// Sets the first block of Data Entry information.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="data">
		/// Pointer to the data to be written
		/// </param>
		/// <param name="nBytes">
		/// Number of bytes to write
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
		[DllImport("spssio32.dll", EntryPoint="spssSetDEWFirst@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetDEWFirst(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string data, int nBytes);
 
		/// <summary>
		/// Sets the unique Data Entry uniqueness GUID.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="asciiGUID">
		/// The GUID (as a null-terminated string) to be stored on the file
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
		[DllImport("spssio32.dll", EntryPoint="spssSetDEWGUID@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetDEWGUID(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string asciiGUID);
 
		/// <summary>
		/// Sets the next block of information for Data Entry.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="data">
		/// Pointer to the data to be written
		/// </param>
		/// <param name="nBytes">
		/// Number of bytes to write
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
		[DllImport("spssio32.dll", EntryPoint="spssSetDEWNext12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetDEWNext(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string data, int nBytes);
 
		/// <summary>
		/// Sets the file label of a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="id">
		/// File label. The length of the string should not exceed 64 characters
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
		[DllImport("spssio32.dll", EntryPoint="spssSetIdString@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetIdString(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string id);
 
		/// <summary>
		/// Sets multiple response definitions to a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file.
		/// </param>
		/// <param name="mrespDefs">
		/// ascii string containing definitions.
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
		/// this function is used to write multiple response definitions to the file. the definitions
		/// consist of a single null-terminated ascii string which is similar to that containing the
		/// variable set definitions.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetMultRespDefs@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetMultRespDefs(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string mrespDefs);
 
		/// <summary>
		/// Sets the text data from the null-terminated string in textInfo.
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
		[DllImport("spssio32.dll", EntryPoint="spssSetTextInfo@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetTextInfo(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string textInfo);
 
		/// <summary>
		/// Sets the value of a string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varHandle">
		/// Handle to the variable
		/// </param>
		/// <param name="value">
		/// Value of the variable as a null-terminated string. The length of the
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
		/// not written out to the data file until <see cref="spssCommitCaseRecord"/> is called.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetValueChar@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetValueChar(int handle, double varHandle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string value);
 
		/// <summary>
		/// Sets the value of a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varHandle">
		/// Handle to the variable
		/// </param>
		/// <param name="value">
		/// Value of the variable
		/// </param>
		/// <returns>
		/// <see cref="ReturnCode.SPSS_OK"/>,
		/// <see cref="ReturnCode.SPSS_INVALID_HANDLE"/>,
		/// <see cref="ReturnCode.SPSS_OPEN_RDMODE"/>,
		/// <see cref="ReturnCode.SPSS_DICT_NOTCOMMIT"/>, or
		/// <see cref="ReturnCode.SPSS_NUME_EXP"/>.
		/// </returns>
		/// <remarks>
		/// This function sets the value of a numeric variable for the current case. The current case
		/// is not written out to the data file until <see cref="spssCommitCaseRecord"/> is called.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetValueNumeric@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern ReturnCode spssSetValueNumeric(int handle, double varHandle, double value);
 
		/// <summary>
		/// Sets the alignment of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="alignment">
		/// Alignment. Must be one of <see cref="AlignmentCode.SPSS_ALIGN_LEFT"/>,
		/// <see cref="AlignmentCode.SPSS_ALIGN_RIGHT"/>, or 
		/// <see cref="AlignmentCode.SPSS_ALIGN_CENTER"/>. If not a legal
		/// value, alignment is set to a type-appropriate default
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarAlignment@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarAlignment(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, AlignmentCode alignment);
 
		/// <summary>
		/// Sets missing values for a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="missingFormat">
		/// The number of missing values (0-3) actually supplied.
		/// </param>
		/// <param name="missingVal1">
		/// First missing value
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarCMissingValues@24", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarCMissingValues(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, MissingValueFormatCode missingFormat, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal1, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal2, [MarshalAs(UnmanagedType.VBByRefStr)] ref string missingVal3);
 
		/// <summary>
		/// Sets the column width of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="columnWidth">
		/// Column width. If negative, a value of zero is (quietly) used instead
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarColumnWidth@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarColumnWidth(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, int columnWidth);
 
		/// <summary>
		/// Changes or adds a value label for a short string variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="value">
		/// Value to be labeled
		/// </param>
		/// <param name="label">
		/// Label
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
		/// variable. The label should be a null-terminated string not exceeding 60 characters in
		/// length.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetVarCValueLabel@16", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarCValueLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string label);
 
		/// <summary>
		/// Sets the variable sets information in a data file.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varSets">
		/// Variable sets information
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
		/// be provided in the form of a null-terminated string. No validity checks are performed
		/// on the supplied string beyond ensuring that its length is not 0. Any existing variable
		/// sets information is discarded.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetVariableSets@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVariableSets(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varSets);

		/// <summary>
		/// Sets the label of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="varLabel">
		/// Variable label. The length of the string should not exceed 120 characters.
		/// If varLabel is the empty string, the existing label, if any, is
		/// deleted.
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarLabel@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varLabel);

		/// <summary>
		/// Sets the measurement level of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="measureLevel">
		/// Measurement level. Must be one of <see cref="MeasurementLevelCode.SPSS_MLVL_NOM"/>,
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_ORD"/>, 
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_RAT"/>, or 
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_UNK"/> for
		/// nominal, ordinal, scale (ratio), and unknown, respectively. If
		/// <see cref="MeasurementLevelCode.SPSS_MLVL_UNK"/>, 
		/// measurement level is set to a type-appropriate default.
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarMeasureLevel@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarMeasureLevel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, int measureLevel);
 
		/// <summary>
		/// Creates a new variable with a given name.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="varType">
		/// Type and size of the variable
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
		///		<description>Dictionary has already been written with <see cref="spssCommitHeader"/></description>
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarName@12", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarName(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, int varType);

		/// <summary>
		/// Sets the missing values for a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="missingFormat">
		/// Missing values format code
		/// </param>
		/// <param name="missingVal1">
		/// First missing value
		/// </param>
		/// <param name="missingVal2">
		/// Second missing value
		/// </param>
		/// <param name="missingVal3">
		/// Third missing value
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarNMissingValues@36", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarNMissingValues(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, MissingValueFormatCode missingFormat, double missingVal1, double missingVal2, double missingVal3);
 
		/// <summary>
		/// Changes or adds a value label to a numeric variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="value">
		/// Value to be labeled
		/// </param>
		/// <param name="label">
		/// Label
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
		/// variable. The label should be a null-terminated string not exceeding 60 characters in
		/// length.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSetVarNValueLabel@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarNValueLabel(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, double value, [MarshalAs(UnmanagedType.VBByRefStr)] ref string label);

		/// <summary>
		/// Sets the print format of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="printType">
		/// Print format type code (file spssdio.h defines macros of the form
		/// <see cref="FormatTypeCode">SPSS_FMT_...</see> for all 
		/// valid format type codes)
		/// </param>
		/// <param name="printDec">
		/// Number of digits after the decimal
		/// </param>
		/// <param name="printWidth">
		/// Print format width
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarPrintFormat@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarPrintFormat(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, FormatTypeCode printType, int printDec, int printWidth);

		/// <summary>
		/// Sets the write format of a variable.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="varName">
		/// Variable name
		/// </param>
		/// <param name="writeType">
		/// Write format type code (file spssdio.h defines macros of the form
		/// <see cref="FormatTypeCode">SPSS_FMT_...</see> for all valid format type codes)
		/// </param>
		/// <param name="writeDec">
		/// Number of digits after the decimal
		/// </param>
		/// <param name="writeWidth">
		/// Write format width
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
		[DllImport("spssio32.dll", EntryPoint="spssSetVarWriteFormat@20", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssSetVarWriteFormat(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string varName, FormatTypeCode writeType, int writeDec, int writeWidth);

		/// <summary>
		/// Gets the system missing value.
		/// </summary>
		/// <returns>
		/// The SPSS system-missing value for the host system.
		/// </returns>
		/// <remarks>
		/// This function returns the SPSS system-missing value for the host system. It may be
		/// called at any time.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssSysmisVal@0", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		public static extern double spssSysmisVal();
 
		/// <summary>
		/// Reads in the raw data for an entire case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseRec">
		/// Buffer to contain the case
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
		/// required size of the buffer may be obtained by calling <see cref="spssGetCaseSize"/>. This is a fairly
		/// low-level function whose use should not be mixed with calls to <see cref="spssReadCaseRecord"/>
		/// using the same file handle because both procedures read a new case from the data file.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssWholeCaseIn@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssWholeCaseIn(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string caseRec);
 
		/// <summary>
		/// Writes out raw data as a case.
		/// </summary>
		/// <param name="handle">
		/// Handle to the data file
		/// </param>
		/// <param name="caseRec">
		/// Case record to be written to the data file
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
		/// to <see cref="spssCommitCaseRecord"/> using the same file handle because both procedures write a
		/// new case to the data file.
		/// </remarks>
		[DllImport("spssio32.dll", EntryPoint="spssWholeCaseOut@8", CharSet=CharSet.Ansi, SetLastError=true, ExactSpelling=true)]
		protected static extern ReturnCode spssWholeCaseOut(int handle, [MarshalAs(UnmanagedType.VBByRefStr)] ref string caseRec);


		/// <summary>
		/// Constructor for <see cref="SpssThinWrapper"/> class.  Not to be used.
		/// </summary>
		/// <remarks>
		/// This class is a static class, and should not be instantiated.
		/// </remarks>
		protected SpssThinWrapper() {} // no construction
	}
}
