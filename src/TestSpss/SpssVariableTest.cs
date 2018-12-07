// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss.Testing
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Xunit;
    using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssVariableTest
    {
        public SpssVariableTest()
        {
        }

        [Fact]
        public void NewStringVariable()
        {
            SpssStringVariable var = new SpssStringVariable();
            var.Name = "new string";
        }

        [Fact]
        public void NewNumericVariable()
        {
            SpssNumericVariable var = new SpssNumericVariable();
            var.Name = "new numeric";
        }

        [Fact]
        public void NewDateVariable()
        {
            SpssDateVariable var = new SpssDateVariable();
            var.Name = "new date";
        }

        [Fact]
        public void VarNameTooLong()
        {
            SpssVariable var = new SpssStringVariable();
            Assert.Throws<ArgumentOutOfRangeException>(() => var.Name = new string('a', SpssSafeWrapper.SPSS_MAX_VARNAME + 1));
        }

        [Fact]
        public void VarNameToNull()
        {
            SpssVariable var = new SpssStringVariable();
            Assert.Throws<ArgumentNullException>(() => var.Name = null);
        }

        [Fact]
        public void VarLabelTooLong()
        {
            SpssVariable var = new SpssStringVariable();
            Assert.Throws<ArgumentOutOfRangeException>(() => var.Label = new string('a', SpssSafeWrapper.SPSS_MAX_VARLABEL + 1));
        }

        [Fact]
        public void VarLabelNeverNull()
        {
            SpssVariable var = new SpssStringVariable();
            Assert.NotNull(var.Label);
            var.Label = null;
            Assert.NotNull(var.Label);
            Assert.Equal(string.Empty, var.Label);
        }

        [Fact]
        public void SetStringValueLabels()
        {
            SpssStringVariable var = new SpssStringVariable();
            var.ValueLabels.Add("h", "hello");
            var.ValueLabels.Add("H", "hi");
            var.ValueLabels.Add("b", "bye");
            Assert.Equal("hello", var.ValueLabels["h"]);
            Assert.Equal("hi", var.ValueLabels["H"]);
            Assert.Equal(3, var.ValueLabels.Count);
        }

        [Fact]
        public void GetLongStringValueLabels()
        {
            using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                SpssStringVariable var = (SpssStringVariable)docRead.Variables["longStr"];

                // long strings can never have value labels
                Assert.Equal(0, var.ValueLabels.Count);
            }
        }

        [Fact]
        public void SetNumericValueLabels()
        {
            SpssNumericVariable var = new SpssNumericVariable();
            var.ValueLabels.Add(1, "Never");
            var.ValueLabels[2] = "Rarely";
            var.ValueLabels.Add(3, "Sometimes");
            var.ValueLabels.Add(4, "Often");
            var.ValueLabels.Add(5, "Very Often");
            Assert.Equal(5, var.ValueLabels.Count);
            Assert.Equal("Sometimes", var.ValueLabels[3]);
            Assert.Equal("Rarely", var.ValueLabels[2]);

            var.ValueLabels.Remove(4);
            Assert.False(var.ValueLabels.ContainsKey(4));
            Assert.Equal(4, var.ValueLabels.Count);
        }

        [Fact]
        public void SetStringTooLong()
        {
            using (SpssDataDocument docAppend = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
            {
                SpssStringVariable var = (SpssStringVariable)docAppend.Variables["charLabels"];
                Debug.Assert(var.Length == 8);
                SpssCase row = docAppend.Cases.New();
                Assert.Throws<ArgumentOutOfRangeException>(() => row["charLabels"] = new string('a', var.Length + 1));
            }
        }

        [Fact]
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
                Assert.Equal(double.NaN, val);
            }
        }

        [Fact]
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
                Assert.False(val.HasValue);
            }
        }

        [Fact]
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
                Assert.False(val.HasValue);
            }
        }

        [Fact]
        public void ReadDecimalPlaces()
        {
            using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                SpssNumericVariable var = docRead.Variables[0] as SpssNumericVariable;
                Assert.NotNull(var); // First variable expected to be numeric.
                Assert.Equal(2, var.WriteDecimal);
            }
        }

        [Fact]
        public void ReadLabel()
        {
            using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                SpssVariable var = docRead.Variables[0];
                Assert.Equal("on butter", var.Label);
            }
        }

        [Fact]
        public void ReadNullLabel()
        {
            using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                SpssVariable var = docRead.Variables[3];
                Assert.Equal(string.Empty, var.Label);
            }
        }

        [Fact]
        public void ReadNullValueLabels()
        {
            using (SpssDataDocument docRead = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                SpssVariable var = docRead.Variables[3];
                Assert.False(var.GetValueLabels().Any());
            }
        }
    }
}
