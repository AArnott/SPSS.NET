//-----------------------------------------------------------------------
// <copyright file="SpssDataSetAdapterTest.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
//     Copyright (c) Intereffective
// </copyright>
//-----------------------------------------------------------------------

namespace Spss.Testing {
	using Spss.DataAdapter;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using System;
	using System.CodeDom.Compiler;
	using System.IO;
	using System.Globalization;

	[TestClass]
	[DeploymentItem("spssio32.dll"), DeploymentItem("msvcp71.dll"), DeploymentItem("msvcr71.dll"), DeploymentItem("icuuc32.dll"), DeploymentItem("icudt32.dll"), DeploymentItem("icuin32.dll")]
	public class SpssDataSetAdapterTest {
		/// <summary>
		/// A test for WriteToSav
		/// </summary>
		[TestMethod]
		public void DataSetSAVRoundTripping() {
			SpssDataSet dataSet = new SpssDataSet();
			dataSet.Variables.AddVariablesRow("v1", "What is your age?", 3, 0, FormatTypeCode.SPSS_FMT_F, 0, 4, FormatTypeCode.SPSS_FMT_F, 0, 5, "88", "98", "102", MissingValueFormatCode.SPSS_THREE_MISSVAL, AlignmentCode.SPSS_ALIGN_LEFT, MeasurementLevelCode.SPSS_MLVL_ORD);
			dataSet.Variables.AddVariablesRow("v2", "What is your name?", 10, 8, FormatTypeCode.SPSS_FMT_A, 0, 0, FormatTypeCode.SPSS_FMT_A, 0, 0, "none", "empty", "who", MissingValueFormatCode.SPSS_THREE_MISSVAL, AlignmentCode.SPSS_ALIGN_LEFT, MeasurementLevelCode.SPSS_MLVL_NOM);
			dataSet.Variables.AddVariablesRow("v3", "What is your birthdate?", 30, 0, FormatTypeCode.SPSS_FMT_DATE_TIME, 0, 30, FormatTypeCode.SPSS_FMT_DATE_TIME, 0, 29, string.Empty, string.Empty, string.Empty, MissingValueFormatCode.SPSS_NO_MISSVAL, AlignmentCode.SPSS_ALIGN_CENTER, MeasurementLevelCode.SPSS_MLVL_NOM);
			dataSet.Responses.Columns.Add("v1", typeof(int));
			dataSet.Responses.Columns.Add("v2", typeof(string));
			dataSet.Responses.Columns.Add("v3", typeof(DateTime));
			var dataRow = dataSet.Responses.NewResponsesRow();
			dataRow["v1"] = 7;
			dataRow["v2"] = "Andrew";
			dataRow["v3"] = DateTime.Parse("9/9/1999", CultureInfo.InvariantCulture);
			dataSet.Responses.AddResponsesRow(dataRow);

			using (TempFileCollection tfc = new TempFileCollection()) {
				string spssFileName = tfc.AddExtension(".sav", false);
				dataSet.WriteToSav(spssFileName);
				Assert.IsTrue(File.Exists(spssFileName));

				SpssDataSet dataSet2 = new SpssDataSet();
				dataSet2.FillFromSav(spssFileName);
				SpssTextFileAdapterTest.AssertEquals(dataSet.Variables, dataSet2.Variables);
				SpssTextFileAdapterTest.AssertEquals(dataSet.Responses, dataSet2.Responses);
			}
		}
	}
}
