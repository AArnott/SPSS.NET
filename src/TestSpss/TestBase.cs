using System;
using System.IO;
using Xunit;

namespace Spss.Testing
{
    /// <summary>
    /// Summary description for TestBase.
    /// </summary>
    public class TestBase : IDisposable
    {
        public const string GoodFilename = @"SAVs\test1.sav";
        public const string AppendFilename = @"SAVs\test2.sav";
        public const string DisposableFilename = @"__temptest.sav";

        protected SpssDataDocument docRead;
        protected SpssDataDocument docAppend;
        protected SpssDataDocument docWrite;

        public TestBase()
        {
            try
            {
                docRead = SpssDataDocument.Open(GoodFilename, SpssFileAccess.Read);
                docAppend = SpssDataDocument.Open(AppendFilename, SpssFileAccess.Append);
                if (File.Exists(DisposableFilename))
                    File.Delete(DisposableFilename);
                docWrite = SpssDataDocument.Create(DisposableFilename);
            }
            catch
            {
                docRead?.Dispose();
                docAppend?.Dispose();
                docWrite?.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            docWrite.Close();
            File.Delete(DisposableFilename);
            docRead.Dispose();
            docAppend.Dispose();
        }
    }
}
