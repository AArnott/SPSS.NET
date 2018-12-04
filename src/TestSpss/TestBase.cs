// Copyright (c) Andrew Arnott. All rights reserved.

namespace Spss.Testing
{
    using System;
    using System.IO;
    using Xunit;

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
                this.docRead = SpssDataDocument.Open(GoodFilename, SpssFileAccess.Read);
                this.docAppend = SpssDataDocument.Open(AppendFilename, SpssFileAccess.Append);
                if (File.Exists(DisposableFilename))
                {
                    File.Delete(DisposableFilename);
                }

                this.docWrite = SpssDataDocument.Create(DisposableFilename);
            }
            catch
            {
                this.docRead?.Dispose();
                this.docAppend?.Dispose();
                this.docWrite?.Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.docWrite.Close();
            File.Delete(DisposableFilename);
            this.docRead.Dispose();
            this.docAppend.Dispose();
        }
    }
}
