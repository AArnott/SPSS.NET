using System;
using System.Linq;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	/// Summary description for SpssVariableTest
	/// </summary>
	[TestClass]
	[DeploymentItem("spssio32.dll"), DeploymentItem("msvcp71.dll"), DeploymentItem("msvcr71.dll"), DeploymentItem("icuuc32.dll"), DeploymentItem("icudt32.dll"), DeploymentItem("icuin32.dll")]
	public class SpssVariableTest
	{
		public SpssVariableTest()
		{
		}

		[TestMethod]
		public void NewStringVariable()
		{
			SpssStringVariable var = new SpssStringVariable();
			var.Name = "new string";
		}

		[TestMethod]
		public void NewNumericVariable()
		{
			SpssNumericVariable var = new SpssNumericVariable();
			var.Name = "new numeric";
		}

		[TestMethod]
		public void NewDateVariable()
		{
			SpssDateVariable var = new SpssDateVariable();
			var.Name = "new date";
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void VarNameTooLong()
		{
			SpssVariable var = new SpssStringVariable();
			var.Name = new string('a', SpssSafeWrapper.SPSS_MAX_VARNAME + 1);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void VarNameToNull()
		{
			SpssVariable var = new SpssStringVariable();
			var.Name = null;
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void VarLabelTooLong()
		{
			SpssVariable var = new SpssStringVariable();
			var.Label = new string('a', SpssSafeWrapper.SPSS_MAX_VARLABEL + 1);
		}

		[TestMethod]
		public void VarLabelNeverNull()
		{
			SpssVariable var = new SpssStringVariable();
			Assert.IsNotNull(var.Label);
			var.Label = null;
			Assert.IsNotNull(var.Label);
			Assert.AreEqual(string.Empty, var.Label);
		}
		[TestMethod]
		public void SetStringValueLabels()
		{
			SpssStringVariable var = new SpssStringVariable();
			var.ValueLabels.Add("h", "hello");
			var.ValueLabels.Add("H", "hi");
			var.ValueLabels.Add("b", "bye");
			Assert.AreEqual("hello", var.ValueLabels["h"]);
			Assert.AreEqual("hi", var.ValueLabels["H"]);
			Assert.AreEqual(3, var.ValueLabels.Count);
		}
		[TestMethod]
		public void GetLongStringValueLabels()
		{
			using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
			{
				SpssStringVariable var = (SpssStringVariable)docRead.Variables["longStr"];
				// long strings can never have value labels
				Assert.AreEqual(0, var.ValueLabels.Count);
			}
		}
		[TestMethod]
		public void SetNumericValueLabels()
		{
			SpssNumericVariable var = new SpssNumericVariable();
			var.ValueLabels.Add(1, "Never");
			var.ValueLabels[2] = "Rarely";
			var.ValueLabels.Add(3, "Sometimes");
			var.ValueLabels.Add(4, "Often");
			var.ValueLabels.Add(5, "Very Often");
			Assert.AreEqual(5, var.ValueLabels.Count);
			Assert.AreEqual("Sometimes", var.ValueLabels[3]);
			Assert.AreEqual("Rarely", var.ValueLabels[2]);

			var.ValueLabels.Remove(4);
			Assert.IsFalse(var.ValueLabels.ContainsKey(4));
			Assert.AreEqual(4, var.ValueLabels.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void SetStringTooLong()
		{
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
			{
				SpssStringVariable var = (SpssStringVariable)docAppend.Variables["charLabels"];
				Debug.Assert(var.Length == 8);
				SpssCase row = docAppend.Cases.New();
				row["charLabels"] = new string('a', var.Length + 1);
			}
		}
		[TestMethod]
		public void SetMissingValueNumeric()
		{
			int rowIndex;
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
			{
				SpssCase row = docAppend.Cases.New();
				rowIndex = row.Position;
				row["num"] = double.NaN;
				row.Commit();
			}
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Read))
			{
				SpssCase row = docAppend.Cases[rowIndex];
				double val = (double)row["num"];
				Assert.AreEqual(double.NaN, val);
			}
		}
		[TestMethod]
		public void SetMissingValueNumericByNull()
		{
			int rowIndex;
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
			{
				SpssCase row = docAppend.Cases.New();
				rowIndex = row.Position;
				row["num"] = null;
				row.Commit();
			}
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Read))
			{
				SpssCase row = docAppend.Cases[rowIndex];
				double? val = (double?)row["num"];
				Assert.IsFalse(val.HasValue);
			}
		}
		[TestMethod]
		public void SetMissingValueDateByNull()
		{
			int rowIndex;
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
			{
				SpssCase row = docAppend.Cases.New();
				rowIndex = row.Position;
				row["dateVar"] = null;
				row.Commit();
			}
			using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Read))
			{
				SpssCase row = docAppend.Cases[rowIndex];
				DateTime? val = (DateTime?)row["dateVar"];
				Assert.IsFalse(val.HasValue);
			}
		}
		[TestMethod]
		public void ReadDecimalPlaces()
		{
			using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
			{
				SpssNumericVariable var = docRead.Variables[0] as SpssNumericVariable;
				Assert.IsNotNull(var, "First variable expected to be numeric.");
				Assert.AreEqual(2, var.DecimalPlaces);
			}
		}
		[TestMethod]
		public void ReadLabel()
		{
			using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
			{
				SpssVariable var = docRead.Variables[0];
				Assert.AreEqual("on butter", var.Label);
			}
		}
		[TestMethod]
		public void ReadNullLabel()
		{
			using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
			{
				SpssVariable var = docRead.Variables[3];
				Assert.AreEqual(string.Empty, var.Label);
			}
		}
		[TestMethod]
		public void ReadNullValueLabels()
		{
			using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
			{
				SpssVariable var = docRead.Variables[3];
				Assert.IsFalse(var.GetValueLabels().Any());
			}
		}
	}
}
