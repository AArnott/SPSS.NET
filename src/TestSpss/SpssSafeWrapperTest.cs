using System;
using Spss;
using Xunit;

namespace Spss.Testing
{
    public class SpssSafeWrapperTest : IDisposable
    {
        public SpssSafeWrapperTest()
        {
            ReturnCode result = SpssSafeWrapper.spssOpenRead(TestBase.GoodFilename, out handle);
            Assert.Equal(ReturnCode.SPSS_OK, result); // Error opening SPSS file.
        }

        public void Dispose()
        {
            if (handle != 0)
            {
                SpssSafeWrapper.spssCloseRead(handle);
                handle = 0;
            }
        }


        protected internal static readonly string[] Test1VarNames = new string[] { "numLabels", "num", "charLabels", "noLabels", "longStr" };

        private int handle = 0;

        [Fact]
        public void spssConvertDate()
        {
            const double expectedDate = 12598070400.0;
            double spssDate;
            SpssSafeWrapper.spssConvertDate(1, 1, 1982, out spssDate);
            Assert.Equal(expectedDate, spssDate);
        }
        [Fact]
        public void spssGetVarNValueLabels()
        {
            string varName = "numLabels";
            double[] values;
            string[] labels;
            SpssSafeWrapper.spssGetVarNValueLabels(handle, varName, out values, out labels);
            Assert.Equal(23, values.Length);
            Assert.Equal(23, labels.Length);

            Assert.Equal(1d, values[0]);
            Assert.Equal("fattening", labels[0]);

            Assert.Equal(2d, values[1]);
            Assert.Equal("men", labels[1]);
        }

        [Fact]
        public void spssGetVarCValueLabels()
        {
            string varName = "charLabels";
            string[] values;
            string[] labels;
            SpssSafeWrapper.spssGetVarCValueLabels(handle, varName, out values, out labels);
            Assert.Equal(2, values.Length);
            Assert.Equal(2, labels.Length);

            Assert.Equal("b", values[0].TrimEnd());
            Assert.Equal("goodbye", labels[0]);

            Assert.Equal("h", values[1].TrimEnd());
            Assert.Equal("hello", labels[1]);
        }

        [Fact]
        public void spssGetVarNames()
        {
            string[] varNames;
            int[] formatTypes;
            SpssSafeWrapper.spssGetVarNames(handle, out varNames, out formatTypes);
            Assert.Equal(Test1VarNames.Length, varNames.Length);
            Assert.Equal(Test1VarNames.Length, formatTypes.Length);

            for (int i = 0; i < Test1VarNames.Length; i++)
                Assert.Equal(Test1VarNames[i], varNames[i]);
        }
        [Fact]
        public void spssGetNumberofCases()
        {
            int count = 0;
            SpssSafeWrapper.spssGetNumberofCases(handle, out count);
            Assert.Equal(138, count);
        }
        [Fact]
        public void spssGetVarInfo()
        {
            const int varIdx = 0;
            string varName = new string(' ', SpssSafeWrapper.SPSS_MAX_VARNAME + 1);
            int varType;
            ReturnCode result = SpssSafeWrapper.spssGetVarInfo(handle, varIdx, out varName, out varType);
            Assert.Equal(ReturnCode.SPSS_OK, result);
            Assert.Equal(Test1VarNames[varIdx], varName);
            Assert.Equal(0, varType);
        }
        [Fact]
        public void spssGetReleaseInfo()
        {
            int[] relInfo;
            ReturnCode result = SpssSafeWrapper.spssGetReleaseInfo(handle, out relInfo);
            Assert.Equal(12, relInfo[0]);
            Assert.Equal(0, relInfo[1]);
            Assert.Equal(0, relInfo[7]);
        }
    }
}
