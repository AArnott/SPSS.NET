using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	/// Summary description for TestBase.
	/// </summary>
	public class TestBase
	{
		public TestBase()
		{
		}

		public const string GoodFilename = @"test1.sav";
		public const string AppendFilename = @"test2.sav";
		public const string DisposableFilename = @"__temptest.sav";

		protected SpssDataDocument docRead;
		protected SpssDataDocument docAppend;
		protected SpssDataDocument docWrite;

		[TestInitialize] public virtual void Initialize()
		{
			docRead = SpssDataDocument.Open(GoodFilename, SpssFileAccess.Read);
			docAppend = SpssDataDocument.Open(AppendFilename, SpssFileAccess.Append);
			if (File.Exists(DisposableFilename))
				File.Delete(DisposableFilename);
			docWrite = SpssDataDocument.Create(DisposableFilename);
		}

		[TestCleanup] public virtual void Cleanup()
		{
			docWrite.Close();
			File.Delete(DisposableFilename);
			docRead.Dispose();
			docAppend.Dispose();
		}

	}
}
