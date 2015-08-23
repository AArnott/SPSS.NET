using System;
using System.Data;
using Xunit;
using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

namespace Spss.Testing
{
    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssCasesCollectionTest : TestBase
    {
        const int expectedRowCount = 138;
        [Fact]
        public void Count()
        {
            Assert.Equal(expectedRowCount, docRead.Cases.Count);
        }

        [Fact]
        public void ReadOnlyAddNew()
        {
            Assert.Throws<InvalidOperationException>(() => docRead.Cases.New());
        }

        [Fact]
        public void New()
        {
            int oldCount = docAppend.Cases.Count;
            SpssCase Case = docAppend.Cases.New();
            Case["num"] = 5;
            Case.Commit();
            Assert.Equal(oldCount + 1, docAppend.Cases.Count);
        }
        [Fact]
        public void SeveralNewRows()
        {
            int oldCount = docAppend.Cases.Count;
            for (int i = 1; i <= 5; i++)
            {
                SpssCase Case = docAppend.Cases.New();
                Case["num"] = i;
                Case.Commit();
                Assert.Equal(oldCount + i, docAppend.Cases.Count);
            }
        }

        [Fact]
        public void GetFirstRowValue()
        {
            SpssCase Case = docRead.Cases[0];
            Assert.Equal(82d, Case["num"]);
        }

        [Fact]
        public void GetMiddleRowValue()
        {
            SpssCase Case = docRead.Cases[5];
            Assert.Equal(6d, Case["num"]);
        }

        [Fact]
        public void GetSeveralRowsValues()
        {
            SpssCase case1 = docRead.Cases[0];
            SpssCase case2 = docRead.Cases[5];
            Assert.Equal(82d, case1["num"]);
            Assert.Equal(6d, case2["num"]);
            Assert.Equal(82d, case1["num"]);
        }
        [Fact]
        public void Enumerator()
        {
            int c = 0;
            foreach (SpssCase Case in docRead.Cases)
            {
                Assert.NotNull(Case);
                Assert.Equal(c, Case.Position);
                c++;
            }
            Assert.Equal(expectedRowCount, docRead.Cases.Count);
        }
        [Fact]
        public void ToDataTable()
        {
            DataTable dt = docRead.Cases.ToDataTable();
            Assert.Equal(docRead.Cases.Count, dt.Rows.Count);
            Assert.Equal(dt.Columns.Count, docRead.Variables.Count);

            Assert.Equal(1d, dt.Rows[0][0]);
            Assert.Equal(82d, dt.Rows[0][1]);
            Assert.Equal(23d, dt.Rows[137][0]);
            Assert.Equal(15d, dt.Rows[137][1]);
        }
    }


}
