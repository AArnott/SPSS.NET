using System;
using System.IO;
using Xunit;
using DeploymentItemAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.DeploymentItemAttribute;

namespace Spss.Testing
{
    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64", "x64")]
    public class SpssDataDocumentTest : IDisposable
    {
        public SpssDataDocumentTest()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
        }

        public void Dispose()
        {
        }


        [Fact]
        public void OpenExistingDocumentForRead()
        {
            using (SpssDataDocument doc = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read))
            {
                Assert.Equal(SpssFileAccess.Read, doc.AccessMode);
                Assert.False(doc.IsClosed, "Newly opened document claims to be closed.");
                Assert.Equal(TestBase.GoodFilename, doc.Filename);
                Assert.False(doc.IsAuthoringDictionary, "Cannot be authoring dictionary when appending data.");
            }
        }

        [Fact]
        public void OpenExistingDocumentForAppend()
        {
            using (SpssDataDocument doc = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append))
            {
                Assert.Equal(SpssFileAccess.Append, doc.AccessMode);
                Assert.False(doc.IsClosed, "Newly opened document claims to be closed.");
                Assert.Equal(TestBase.AppendFilename, doc.Filename);
                Assert.False(doc.IsAuthoringDictionary, "Cannot be authoring dictionary when appending data.");
            }
        }

        [Fact]
        public void CreateDocument()
        {
            if (File.Exists(TestBase.DisposableFilename)) File.Delete(TestBase.DisposableFilename);
            try
            {
                using (SpssDataDocument doc = SpssDataDocument.Create(TestBase.DisposableFilename))
                {
                    Assert.Equal(SpssFileAccess.Create, doc.AccessMode);
                    Assert.False(doc.IsClosed, "Newly opened document claims to be closed.");
                    Assert.Equal(TestBase.DisposableFilename, doc.Filename);
                    Assert.True(doc.IsAuthoringDictionary, "Newly created data file should be in dictionary authoring mode.");
                    Assert.True(doc.IsCompressed, "Newly created documents should default to being Compressed.");
                }
            }
            finally
            {
                if (File.Exists(TestBase.DisposableFilename)) File.Delete(TestBase.DisposableFilename);
            }
        }
    }
}
