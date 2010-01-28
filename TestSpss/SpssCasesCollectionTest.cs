using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	///This is a test class for Spss.SpssCasesCollection and is intended
	///to contain all Spss.SpssCasesCollection Unit Tests
	///</summary>
	[TestClass()]
	[DeploymentItem("spssio32.dll"), DeploymentItem("msvcp71.dll"), DeploymentItem("msvcr71.dll"), DeploymentItem("icuuc32.dll"), DeploymentItem("icudt32.dll"), DeploymentItem("icuin32.dll")]
	public class SpssCasesCollectionTest : TestBase
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

		const int expectedRowCount = 138;
		[TestMethod]
		public void Count()
		{
			Assert.AreEqual(expectedRowCount, docRead.Cases.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ReadOnlyAddNew()
		{
			docRead.Cases.New();
		}

		[TestMethod]
		public void New()
		{
			int oldCount = docAppend.Cases.Count;
			SpssCase Case = docAppend.Cases.New();
			Case["num"] = 5;
			Case.Commit();
			Assert.AreEqual(oldCount + 1, docAppend.Cases.Count);
		}
		[TestMethod]
		public void SeveralNewRows()
		{
			int oldCount = docAppend.Cases.Count;
			for (int i = 1; i <= 5; i++)
			{
				SpssCase Case = docAppend.Cases.New();
				Case["num"] = i;
				Case.Commit();
				Assert.AreEqual(oldCount + i, docAppend.Cases.Count);
			}
		}

		[TestMethod]
		public void GetFirstRowValue()
		{
			SpssCase Case = docRead.Cases[0];
			Assert.AreEqual(82d, Case["num"]);
		}

		[TestMethod]
		public void GetMiddleRowValue()
		{
			SpssCase Case = docRead.Cases[5];
			Assert.AreEqual(6d, Case["num"]);
		}

		[TestMethod]
		public void GetSeveralRowsValues()
		{
			SpssCase case1 = docRead.Cases[0];
			SpssCase case2 = docRead.Cases[5];
			Assert.AreEqual(82d, case1["num"]);
			Assert.AreEqual(6d, case2["num"]);
			Assert.AreEqual(82d, case1["num"]);
		}
		[TestMethod]
		public void Enumerator()
		{
			int c = 0;
			foreach (SpssCase Case in docRead.Cases)
			{
				Assert.IsNotNull(Case);
				Assert.AreEqual(c, Case.Position);
				c++;
			}
			Assert.AreEqual(expectedRowCount, docRead.Cases.Count);
		}
		[TestMethod]
		public void ToDataTable()
		{
			DataTable dt = docRead.Cases.ToDataTable();
			Assert.AreEqual(docRead.Cases.Count, dt.Rows.Count);
			Assert.AreEqual(dt.Columns.Count, docRead.Variables.Count);

			Assert.AreEqual(1d, dt.Rows[0][0]);
			Assert.AreEqual(82d, dt.Rows[0][1]);
			Assert.AreEqual(23d, dt.Rows[137][0]);
			Assert.AreEqual(15d, dt.Rows[137][1]);
		}
	}


}
