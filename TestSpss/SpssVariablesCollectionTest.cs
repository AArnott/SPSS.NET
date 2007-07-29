using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	///This is a test class for Spss.SpssVariablesCollection and is intended
	///to contain all Spss.SpssVariablesCollection Unit Tests
	///</summary>
	[TestClass()]
	public class SpssVariablesCollectionTest : TestBase
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


		[TestMethod]
		public void Count()
		{
			Assert.AreEqual(SpssSafeWrapperTest.Test1VarNames.Length, docRead.Variables.Count);
		}

		[TestMethod]
		public void Enumerate()
		{
			int i = 0;
			foreach (SpssVariable var in docRead.Variables)
			{
				Assert.IsNotNull(var);
				Assert.IsTrue(i < SpssSafeWrapperTest.Test1VarNames.Length, "Too many variables!");
				Assert.AreEqual(SpssSafeWrapperTest.Test1VarNames[i], var.Name);
				i++;
			}
			Assert.AreEqual(SpssSafeWrapperTest.Test1VarNames.Length, i);
		}
		[TestMethod]
		public void IndexerByName()
		{
			SpssVariable var = docRead.Variables[SpssSafeWrapperTest.Test1VarNames[0]];
			Assert.IsNotNull(var);
			Assert.AreEqual(SpssSafeWrapperTest.Test1VarNames[0], var.Name);
		}
		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void IndexerByNameBad()
		{
			SpssVariable var = docRead.Variables["some bad var name"];
		}
		[TestMethod]
		[ExpectedException(typeof(KeyNotFoundException))]
		public void IndexerByNameEmpty()
		{
			SpssVariable var = docRead.Variables[""];
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IndexerByNameNull()
		{
			SpssVariable var = docRead.Variables[null];
		}
		[TestMethod]
		public void IndexerByOrdinal()
		{
			SpssVariable var = docRead.Variables[0];
			Assert.IsNotNull(var);
			Assert.AreEqual(SpssSafeWrapperTest.Test1VarNames[0], var.Name);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IndexerByOrdinalNegative()
		{
			SpssVariable var = docRead.Variables[-1];
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IndexerByOrdinalTooHigh()
		{
			SpssVariable var = docRead.Variables[4096]; // just some insanely high number
		}
		[TestMethod]
		public void SameVarObjectRepeatedly()
		{
			SpssVariable var1 = docRead.Variables[0];
			SpssVariable var2 = docRead.Variables[0];
			Assert.AreSame(var1, var2);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddNull()
		{
			docRead.Variables.Add(null);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InsertNull()
		{
			docRead.Variables.Insert(0, null);
		}
		[TestMethod]
		[ExpectedException(typeof(SpssVariableNameConflictException))]
		public void AddVarWithSameName()
		{
			SpssVariable var1 = new SpssStringVariable();
			var1.Name = "var1";
			docWrite.Variables.Add(var1);

			SpssVariable var2 = new SpssStringVariable();
			var2.Name = "Var1";
			docWrite.Variables.Add(var2);
		}
		[TestMethod]
		[ExpectedException(typeof(SpssVariableNameConflictException))]
		public void RenameVarToConflictingName()
		{
			SpssVariable var1 = new SpssStringVariable();
			var1.Name = "var1";
			docWrite.Variables.Add(var1);

			SpssVariable var2 = new SpssStringVariable();
			var2.Name = "var2";
			docWrite.Variables.Add(var2);

			// Now rename one variable to match the name of the other.
			// Capitalization differences should still cause an exception.
			var2.Name = "Var1";
		}
		[TestMethod]
		public void AddVariableToNewFile()
		{
			SpssVariable var = new SpssStringVariable();
			var.Name = "var1";
			docWrite.Variables.Add(var);
			Assert.AreEqual(1, docWrite.Variables.Count);
		}
		[TestMethod]
		public void CommitVariableToNewFile()
		{
			try
			{
				AddVariableToNewFile();
			}
			catch (Exception)
			{
				Assert.Inconclusive("AddVariableToNewFile() failed.");
			}
			docWrite.CommitDictionary();
		}
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void AddVariableAfterCommit()
		{
			try
			{
				CommitVariableToNewFile();
			}
			catch (Exception)
			{
				Assert.Inconclusive("AddVariableToNewFileAndCommit() failed.");
			}
			SpssVariable var = new SpssStringVariable();
			var.Name = "anothervar";
			docWrite.Variables.Add(var);
		}
		[TestMethod]
		public void CommitVariableWithLabels()
		{
			SpssNumericVariable var = new SpssNumericVariable();
			var.Name = "var1";
			var.ValueLabels.Add(1, "Never");
			var.ValueLabels.Add(2, "Rarely");
			docWrite.Variables.Add(var);
			docWrite.CommitDictionary();
			docWrite.Close();
		}
		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ChangeLabelAfterCommit()
		{
			try
			{
				CommitVariableWithLabels();
			}
			catch (Exception)
			{
				Assert.Inconclusive("CommitVariableWithLabels() failed.");
			}
			SpssNumericVariable var = (SpssNumericVariable)docWrite.Variables[0];
			var.ValueLabels[6] = "break";
		}
		[TestMethod]
		public void LookUpLoadedLabels()
		{
			SpssNumericVariable var = (SpssNumericVariable)docRead.Variables["numLabels"];
			Assert.AreEqual(23, var.ValueLabels.Count);
			Assert.AreEqual("fattening", var.ValueLabels[1]);
		}
	}


}
