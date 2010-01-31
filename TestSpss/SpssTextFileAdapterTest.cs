namespace Spss.Testing {
	using Spss.DataAdapter;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using System.Data;
	using System.IO;

	[TestClass]
	public class SpssTextFileAdapterTest {
		private const string VariablesTableTextFormat = @"Name	Label	Width	Length	PrintFormat	PrintDecimal	PrintWidth	WriteFormat	WriteDecimal	WriteWidth
v1	What is your age?	3	0	5	0	3	5	0	3
v2	What is your name?	15	15	1	0	15	1	0	15
";

		/// <summary>
		/// A test for SaveDataTable
		/// </summary>
		[TestMethod()]
		public void SaveDataTableTest() {
			var dataTable = GetVariablesDataTable();
			StringWriter writer = new StringWriter();
			char delimiter = '\t';
			SpssTextFileAdapter.SaveDataTable(dataTable, writer, delimiter);
			Assert.AreEqual(VariablesTableTextFormat, writer.ToString());
		}

		/// <summary>
		/// A test for LoadDataTable
		/// </summary>
		[TestMethod]
		public void LoadDataTableTest() {
			DataTable dataTable = new SpssDataSet.VariablesDataTable();
			TextReader file = new StringReader(VariablesTableTextFormat);
			char delimiter = '\t';
			SpssTextFileAdapter.LoadDataTable(dataTable, file, delimiter);
			AssertEquals(dataTable, GetVariablesDataTable());
		}

		private void AssertEquals(DataTable expected, DataTable actual) {
			if (expected == actual) {
				return;
			}

			// Since both are not null (or we would have returned already), neither can be.
			Assert.IsNotNull(expected);
			Assert.IsNotNull(actual);

			// For purposes of these tests, the schemas are assumed to be equal.
			Assert.AreEqual(expected.Rows.Count, actual.Rows.Count, "Unequal number of rows in tables.");
			for (var i = 0; i < expected.Rows.Count; i++) {
				for (int j = 0; j < expected.Columns.Count; j++) {
					Assert.AreEqual(expected.Rows[i][j], actual.Rows[i][j]);
				}
			}
		}

		private static SpssDataSet.VariablesDataTable GetVariablesDataTable() {
			var dataTable = new SpssDataSet.VariablesDataTable();
			dataTable.AddVariablesRow("v1", "What is your age?", 3, 0, FormatTypeCode.SPSS_FMT_F, 0, 3, FormatTypeCode.SPSS_FMT_F, 0, 3);
			dataTable.AddVariablesRow("v2", "What is your name?", 15, 15, FormatTypeCode.SPSS_FMT_A, 0, 15, FormatTypeCode.SPSS_FMT_A, 0, 15);
			return dataTable;
		}
	}
}
