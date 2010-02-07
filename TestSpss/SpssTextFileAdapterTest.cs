namespace Spss.Testing {
	using Spss.DataAdapter;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using System.Data;
	using System.IO;

	[TestClass]
	public class SpssTextFileAdapterTest {
		private const string VariablesTableTextFormat = @"Name	Label	Width	Length	PrintFormat	PrintDecimal	PrintWidth	WriteFormat	WriteDecimal	WriteWidth	MissingValue1	MissingValue2	MissingValue3	MissingFormatCode	Alignment	MeasurementLevel
v1	What is your age?	3	0	5	0	3	5	0	3				0	0	1
v2	What is your name?	15	15	1	0	15	1	0	15				0	0	1
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

		internal static void AssertEquals(DataTable expected, DataTable actual) {
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
					Assert.AreEqual(TypeCoersion(expected.Rows[i][j]), TypeCoersion(actual.Rows[i][j]), "Row {0}, column {1} did not match.", i + 1, expected.Columns[j].ColumnName);
				}
			}
		}

		private static object TypeCoersion(object value) {
			if (value == null || value == DBNull.Value) {
				return DBNull.Value;
			}

			if (value is double) {
				return value;
			}

			if (value is int) {
				return (double)(int)value;
			}

			if (value is string) {
				return ((string)value).TrimEnd();
			}

			return value;
		}

		private static SpssDataSet.VariablesDataTable GetVariablesDataTable() {
			var dataTable = new SpssDataSet.VariablesDataTable();
			dataTable.AddVariablesRow("v1", "What is your age?", 3, 0, FormatTypeCode.SPSS_FMT_F, 0, 3, FormatTypeCode.SPSS_FMT_F, 0, 3, string.Empty, string.Empty, string.Empty, MissingValueFormatCode.SPSS_NO_MISSVAL, AlignmentCode.SPSS_ALIGN_LEFT, MeasurementLevelCode.SPSS_MLVL_NOM);
			dataTable.AddVariablesRow("v2", "What is your name?", 15, 15, FormatTypeCode.SPSS_FMT_A, 0, 15, FormatTypeCode.SPSS_FMT_A, 0, 15, string.Empty, string.Empty, string.Empty, MissingValueFormatCode.SPSS_NO_MISSVAL, AlignmentCode.SPSS_ALIGN_LEFT, MeasurementLevelCode.SPSS_MLVL_NOM);
			return dataTable;
		}
	}
}
