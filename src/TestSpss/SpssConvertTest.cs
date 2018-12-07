// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss.Testing
{
    using System;
    using System.CodeDom.Compiler;
    using System.Data;
    using System.IO;
    using Xunit;
    using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssConvertTest
    {
        public SpssConvertTest()
        {
            this.tblTest = new DataTable();
            DataColumn cID = this.tblTest.Columns.Add("ID", typeof(int));
            DataColumn cStr = this.tblTest.Columns.Add("str", typeof(string));
            DataColumn cDate = this.tblTest.Columns.Add("date", typeof(DateTime));
            DataColumn cFloat = this.tblTest.Columns.Add("float", typeof(float));

            const int cIDv = 3;
            const string cStrv = "hello";
            DateTime cDatev = DateTime.Now;
            const float cFloatv = 3.553F;
            object[] values = { cIDv, cStrv, cDatev, cFloatv };
            this.tblTest.Rows.Add(values);
        }

        private DataTable tblTest;

        [Fact]
        public void toDdiNull()
        {
            Assert.Throws<ArgumentNullException>(() => SpssConvert.ToDdi((string)null));
        }

        [SkippableFact]
        public void testT1()
        {
            const string fileName = @"C:\Program Files\SPSS\T1.sav";
            Skip.IfNot(File.Exists(fileName));
            SpssConvert.ToDdi(fileName);
        }

        [SkippableFact]
        public void testlistSurveys()
        {
            const string fileName = @"C:\Program Files\SPSS\listsurveys.sav";
            Skip.IfNot(File.Exists(fileName));
            SpssConvert.ToDdi(fileName);
        }

        [SkippableFact]
        public void smoking()
        {
            const string fileName = @"C:\Program Files\SPSS\smoking.sav";
            Skip.IfNot(File.Exists(fileName));
            SpssConvert.ToDdi(fileName);
        }

        [SkippableFact]
        public void anorectic()
        {
            const string fileName = @"C:\Program Files\SPSS\anorectic.sav";
            Skip.IfNot(File.Exists(fileName));
            SpssConvert.ToDdi(fileName);
        }

        [Fact]
        public void MetaData()
        {
            string SAVfilename;
            using (System.CodeDom.Compiler.TempFileCollection tfc = new System.CodeDom.Compiler.TempFileCollection())
            {
                SAVfilename = tfc.AddExtension("sav", true);
                SpssConvert.ToFile(this.tblTest, this.tblTest.Select(), SAVfilename, this.FillInMetaData);
                Console.WriteLine("The file with metadata is stored at: " + SAVfilename);
            }
        }

        protected void FillInMetaData(SpssVariable var)
        {
            switch (var.Name)
            {
                case "ID":
                    var numericVar = (SpssNumericVariable)var;
                    var.Label = "Some ID label";
                    numericVar.ValueLabels[1] = "one";
                    numericVar.ValueLabels[2] = "two";
                    numericVar.ValueLabels[3] = "three";
                    numericVar.ValueLabels[4] = "four";
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

        [Fact]
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
