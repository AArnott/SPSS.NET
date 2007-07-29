using System;
using System.Data;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	///This is a test class for Spss.SpssConvert and is intended
	///to contain all Spss.SpssConvert Unit Tests
	///</summary>
	[TestClass()]
	public class SpssConvertTest
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
			tblTest = new DataTable();
			DataColumn cID = tblTest.Columns.Add("ID", typeof(int));
			DataColumn cStr = tblTest.Columns.Add("str", typeof(string));
			DataColumn cDate = tblTest.Columns.Add("date", typeof(DateTime));
			DataColumn cFloat = tblTest.Columns.Add("float", typeof(float));

			const int cIDv = 3;
			const string cStrv = "hello";
			DateTime cDatev = DateTime.Now;
			const float cFloatv = 3.553F;
			object[] values = { cIDv, cStrv, cDatev, cFloatv };
			tblTest.Rows.Add(values);
		}

		/// <summary>
		///Cleanup() is called once during test execution after
		///test methods in this class have executed unless
		///this test class' Initialize() method throws an exception.
		///</summary>
		[TestCleanup()]
		public void Cleanup()
		{
			//  TODO: Add test cleanup code
		}

		DataTable tblTest;

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void toDdiNull()
		{
			SpssConvert.ToDdi((string)null);
		}

		[Ignore]
		[TestMethod]
		public void testT1()
		{
			SpssConvert.ToDdi(@"C:\Program Files\SPSS\T1.sav");
		}

		[Ignore]
		[TestMethod]
		public void testlistSurveys()
		{
			SpssConvert.ToDdi(@"C:\Program Files\SPSS\listsurveys.sav");
		}

		[TestMethod]
		public void smoking()
		{
			SpssConvert.ToDdi(@"C:\Program Files\SPSS\smoking.sav");
		}

		[TestMethod]
		public void anorectic()
		{
			SpssConvert.ToDdi(@"C:\Program Files\SPSS\anorectic.sav");
		}

		[TestMethod]
		public void MetaData()
		{
			string SAVfilename;
			using (System.CodeDom.Compiler.TempFileCollection tfc = new System.CodeDom.Compiler.TempFileCollection())
			{
				SAVfilename = tfc.AddExtension("sav", true);
				SpssConvert.ToFile(tblTest, tblTest.Select(), SAVfilename, new MetadataProviderCallback(FillInMetaData));
				Console.WriteLine("The file with metadata is stored at: " + SAVfilename);
			}
		}
		protected void FillInMetaData(VarMetaData var)
		{
			switch (var.Name)
			{
				case "ID":
					var.Label = "Some ID label";
					var[1] = "one";
					var[2] = "two";
					var[3] = "three";
					var[4] = "four";
					break;
				case "str":
					var.Label = "some str label";
					break;
				case "date":
					var.Label = "some date label";
					break;
				case "float":
					var.Label = "some float label";
					break;
			}
		}

		[TestMethod]
		public void ToFile()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("var1", typeof(int));
			dt.Columns.Add("var2", typeof(string));
			dt.Rows.Add(new object[] { 1, "hi" });

			using (TempFileCollection tfc = new TempFileCollection())
			{
				tfc.KeepFiles = false;
				string filename = tfc.AddExtension("sav");
				SpssConvert.ToFile(dt, dt.Select(), filename, null);
			}
		}

	}


}
