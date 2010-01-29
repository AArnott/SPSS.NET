using Spss;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Spss.Testing
{
	/// <summary>
	///This is a test class for Spss.SpssSafeWrapper and is intended
	///to contain all Spss.SpssSafeWrapper Unit Tests
	///</summary>
	[TestClass()]
	public class SpssSafeWrapperTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		/// <summary>
		///Initialize() is called once during test execution before
		///test methods in this test class are executed.
		///</summary>
		[TestInitialize()]
		public void Initialize()
		{
			ReturnCode result = SpssSafeWrapper.spssOpenRead(TestBase.GoodFilename, out handle);
			Assert.AreEqual(ReturnCode.SPSS_OK, result, "Error opening SPSS file.");
		}

		/// <summary>
		///Cleanup() is called once during test execution after
		///test methods in this class have executed unless
		///this test class' Initialize() method throws an exception.
		///</summary>
		[TestCleanup()]
		public void Cleanup()
		{
			if (handle != 0)
			{
				SpssSafeWrapper.spssCloseRead(handle);
				handle = 0;
			}
		}


		protected internal static readonly string[] Test1VarNames = new string[] { "numLabels", "num", "charLabels", "noLabels", "longStr" };

		int handle = 0;

		[TestMethod]
		public void spssConvertDate()
		{
			const double expectedDate = 12598070400.0;
			double spssDate;
			SpssSafeWrapper.spssConvertDate(1, 1, 1982, out spssDate);
			Assert.AreEqual(expectedDate, spssDate);
		}
		[TestMethod]
		public void spssGetVarNValueLabels()
		{
			string varName = "numLabels";
			double[] values;
			string[] labels;
			SpssSafeWrapper.spssGetVarNValueLabels(handle, varName, out values, out labels);
			Assert.AreEqual(23, values.Length);
			Assert.AreEqual(23, labels.Length);

			Assert.AreEqual(1d, values[0]);
			Assert.AreEqual("fattening", labels[0]);

			Assert.AreEqual(2d, values[1]);
			Assert.AreEqual("men", labels[1]);
		}

		[TestMethod]
		public void spssGetVarCValueLabels()
		{
			string varName = "charLabels";
			string[] values;
			string[] labels;
			SpssSafeWrapper.spssGetVarCValueLabels(handle, varName, out values, out labels);
			Assert.AreEqual(2, values.Length);
			Assert.AreEqual(2, labels.Length);

			Assert.AreEqual("b", values[0].TrimEnd());
			Assert.AreEqual("goodbye", labels[0]);

			Assert.AreEqual("h", values[1].TrimEnd());
			Assert.AreEqual("hello", labels[1]);
		}

		[TestMethod]
		public void spssGetVarNames()
		{
			string[] varNames;
			int[] formatTypes;
			SpssSafeWrapper.spssGetVarNames(handle, out varNames, out formatTypes);
			Assert.AreEqual(Test1VarNames.Length, varNames.Length);
			Assert.AreEqual(Test1VarNames.Length, formatTypes.Length);

			for (int i = 0; i < Test1VarNames.Length; i++)
				Assert.AreEqual(Test1VarNames[i], varNames[i]);
		}
		[TestMethod]
		public void spssGetNumberofCases()
		{
			int count = 0;
			SpssSafeWrapper.spssGetNumberofCases(handle, out count);
			Assert.AreEqual(138, count);
		}
		[TestMethod]
		public void spssGetVarInfo()
		{
			const int varIdx = 0;
			string varName = new string(' ', SpssSafeWrapper.SPSS_MAX_VARNAME + 1);
			int varType;
			ReturnCode result = SpssSafeWrapper.spssGetVarInfo(handle, varIdx, out varName, out varType);
			Assert.AreEqual(ReturnCode.SPSS_OK, result);
			Assert.AreEqual(Test1VarNames[varIdx], varName);
			Assert.AreEqual(0, varType);
		}
		[TestMethod]
		public void spssGetReleaseInfo()
		{
			int[] relInfo;
			ReturnCode result = SpssSafeWrapper.spssGetReleaseInfo(handle, out relInfo);
			Assert.AreEqual(12, relInfo[0]);
			Assert.AreEqual(0, relInfo[1]);
			Assert.AreEqual(0, relInfo[7]);
		}
	}


}
