using System;
using System.Collections.Generic;
using Xunit;
using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

namespace Spss.Testing
{
    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssVariablesCollectionTest : TestBase
    {
        [Fact]
        public void Count()
        {
            Assert.Equal(SpssSafeWrapperTest.Test1VarNames.Length, docRead.Variables.Count);
        }

        [Fact]
        public void Enumerate()
        {
            int i = 0;
            foreach (SpssVariable var in docRead.Variables)
            {
                Assert.NotNull(var);
                Assert.True(i < SpssSafeWrapperTest.Test1VarNames.Length, "Too many variables!");
                Assert.Equal(SpssSafeWrapperTest.Test1VarNames[i], var.Name);
                i++;
            }
            Assert.Equal(SpssSafeWrapperTest.Test1VarNames.Length, i);
        }
        [Fact]
        public void IndexerByName()
        {
            SpssVariable var = docRead.Variables[SpssSafeWrapperTest.Test1VarNames[0]];
            Assert.NotNull(var);
            Assert.Equal(SpssSafeWrapperTest.Test1VarNames[0], var.Name);
        }
        [Fact]
        public void IndexerByNameBad()
        {
            Assert.Throws<KeyNotFoundException>(() => docRead.Variables["some bad var name"]);
        }
        [Fact]
        public void IndexerByNameEmpty()
        {
            Assert.Throws<KeyNotFoundException>(() => docRead.Variables[string.Empty]);
        }
        [Fact]
        public void IndexerByNameNull()
        {
            Assert.Throws<ArgumentNullException>(() => docRead.Variables[null]);
        }
        [Fact]
        public void IndexerByOrdinal()
        {
            SpssVariable var = docRead.Variables[0];
            Assert.NotNull(var);
            Assert.Equal(SpssSafeWrapperTest.Test1VarNames[0], var.Name);
        }
        [Fact]
        public void IndexerByOrdinalNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => docRead.Variables[-1]);
        }
        [Fact]
        public void IndexerByOrdinalTooHigh()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => docRead.Variables[4096]); // just some insanely high number
        }
        [Fact]
        public void SameVarObjectRepeatedly()
        {
            SpssVariable var1 = docRead.Variables[0];
            SpssVariable var2 = docRead.Variables[0];
            Assert.Same(var1, var2);
        }
        [Fact]
        public void AddNull()
        {
            Assert.Throws<ArgumentNullException>(() => docRead.Variables.Add(null));
        }
        [Fact]
        public void InsertNull()
        {
            Assert.Throws<ArgumentNullException>(() => docRead.Variables.Insert(0, null));
        }
        [Fact]
        public void AddVarWithSameName()
        {
            SpssVariable var1 = new SpssStringVariable();
            var1.Name = "var1";
            docWrite.Variables.Add(var1);

            SpssVariable var2 = new SpssStringVariable();
            var2.Name = "Var1";
            Assert.Throws<SpssVariableNameConflictException>(() => docWrite.Variables.Add(var2));
        }
        [Fact]
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
            Assert.Throws<SpssVariableNameConflictException>(() => var2.Name = "Var1");
        }
        [Fact]
        public void AddVariableToNewFile()
        {
            SpssVariable var = new SpssStringVariable();
            var.Name = "var1";
            docWrite.Variables.Add(var);
            Assert.Equal(1, docWrite.Variables.Count);
        }
        [SkippableFact]
        public void CommitVariableToNewFile()
        {
            try
            {
                AddVariableToNewFile();
            }
            catch (Exception)
            {
                Skip.If(true, "AddVariableToNewFile() failed.");
            }
            docWrite.CommitDictionary();
        }
        [Fact]
        public void AddVariableAfterCommit()
        {
            try
            {
                CommitVariableToNewFile();
            }
            catch (Exception)
            {
                Skip.If(true, "AddVariableToNewFileAndCommit() failed.");
            }
            SpssVariable var = new SpssStringVariable();
            var.Name = "anothervar";
            Assert.Throws<InvalidOperationException>(() => docWrite.Variables.Add(var));
        }
        [Fact]
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
        [Fact]
        public void ChangeLabelAfterCommit()
        {
            try
            {
                CommitVariableWithLabels();
            }
            catch (Exception)
            {
                Skip.If(true, "CommitVariableWithLabels() failed.");
            }
            SpssNumericVariable var = (SpssNumericVariable)docWrite.Variables[0];
            Assert.Throws<InvalidOperationException>(() => var.ValueLabels[6] = "break");
        }
        [Fact]
        public void LookUpLoadedLabels()
        {
            SpssNumericVariable var = (SpssNumericVariable)docRead.Variables["numLabels"];
            Assert.Equal(23, var.ValueLabels.Count);
            Assert.Equal("fattening", var.ValueLabels[1]);
        }
    }
}
