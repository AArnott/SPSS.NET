// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss.Testing
{
    using System;
    using System.Data;
    using Xunit;
    using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssCasesCollectionTest : TestBase
    {
        private const int expectedRowCount = 138;

        [Fact]
        public void Count()
        {
            Assert.Equal(expectedRowCount, this.docRead.Cases.Count);
        }

        [Fact]
        public void ReadOnlyAddNew()
        {
            Assert.Throws<InvalidOperationException>(() => this.docRead.Cases.New());
        }

        [Fact]
        public void New()
        {
            int oldCount = this.docAppend.Cases.Count;
            SpssCase Case = this.docAppend.Cases.New();
            Case["num"] = 5;
            Case.Commit();
            Assert.Equal(oldCount + 1, this.docAppend.Cases.Count);
        }

        [Fact]
        public void SeveralNewRows()
        {
            int oldCount = this.docAppend.Cases.Count;
            for (int i = 1; i <= 5; i++)
            {
                SpssCase Case = this.docAppend.Cases.New();
                Case["num"] = i;
                Case.Commit();
                Assert.Equal(oldCount + i, this.docAppend.Cases.Count);
            }
        }

        [Fact]
        public void GetFirstRowValue()
        {
            SpssCase Case = this.docRead.Cases[0];
            Assert.Equal(82d, Case["num"]);
        }

        [Fact]
        public void GetMiddleRowValue()
        {
            SpssCase Case = this.docRead.Cases[5];
            Assert.Equal(6d, Case["num"]);
        }

        [Fact]
        public void GetSeveralRowsValues()
        {
            SpssCase case1 = this.docRead.Cases[0];
            SpssCase case2 = this.docRead.Cases[5];
            Assert.Equal(82d, case1["num"]);
            Assert.Equal(6d, case2["num"]);
            Assert.Equal(82d, case1["num"]);
        }

        [Fact]
        public void Enumerator()
        {
            int c = 0;
            foreach (SpssCase Case in this.docRead.Cases)
            {
                Assert.NotNull(Case);
                Assert.Equal(c, Case.Position);
                c++;
            }
            Assert.Equal(expectedRowCount, this.docRead.Cases.Count);
        }

        [Fact]
        public void ToDataTable()
        {
            DataTable dt = this.docRead.Cases.ToDataTable();
            Assert.Equal(this.docRead.Cases.Count, dt.Rows.Count);
            Assert.Equal(dt.Columns.Count, this.docRead.Variables.Count);

            Assert.Equal(1d, dt.Rows[0][0]);
            Assert.Equal(82d, dt.Rows[0][1]);
            Assert.Equal(23d, dt.Rows[137][0]);
            Assert.Equal(15d, dt.Rows[137][1]);
        }
    }
}
