using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Spss.Testing
{
	/// <summary>
	///This is a test class for Spss.SpssDataDocument and is intended
	///to contain all Spss.SpssDataDocument Unit Tests
	///</summary>
	[TestClass()]
    [DeploymentItem("x86", "x86")]
    [DeploymentItem("x64","x64")]
    public class SpssDataDocumentTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		/// <summary>
		///Initialize() is called once during test execution before
		///test methods in this test class are executed.
		///</summary>
		[TestInitialize()]
		public void Initialize()
		{
			Console.WriteLine(Directory.GetCurrentDirectory());
		}

		/// <summary>
		///Cleanup() is called once during test execution after
		///test methods in this class have executed unless
		///this test class' Initialize() method throws an exception.
		///</summary>
		[TestCleanup()]
		public void Cleanup()
		{
		}


		[TestMethod]
		public void OpenExistingDocumentForRead()
		{
			using( SpssDataDocument doc = SpssDataDocument.Open(TestBase.GoodFilename, SpssFileAccess.Read) )
			{
				Assert.AreEqual( SpssFileAccess.Read, doc.AccessMode );
				Assert.IsFalse( doc.IsClosed, "Newly opened document claims to be closed." );
				Assert.AreEqual( TestBase.GoodFilename, doc.Filename );
				Assert.IsFalse( doc.IsAuthoringDictionary, "Cannot be authoring dictionary when appending data." );
			}
		}

		[TestMethod]
		public void OpenExistingDocumentForAppend()
		{
			using( SpssDataDocument doc = SpssDataDocument.Open(TestBase.AppendFilename, SpssFileAccess.Append) )
			{
				Assert.AreEqual( SpssFileAccess.Append, doc.AccessMode );
				Assert.IsFalse( doc.IsClosed, "Newly opened document claims to be closed." );
				Assert.AreEqual( TestBase.AppendFilename, doc.Filename );
				Assert.IsFalse( doc.IsAuthoringDictionary, "Cannot be authoring dictionary when appending data." );
			}
		}

		[TestMethod]
		public void CreateDocument()
		{
			if( File.Exists(TestBase.DisposableFilename) ) File.Delete(TestBase.DisposableFilename);
			try 
			{
				using( SpssDataDocument doc = SpssDataDocument.Create(TestBase.DisposableFilename) )
				{
					Assert.AreEqual( SpssFileAccess.Create, doc.AccessMode );
					Assert.IsFalse( doc.IsClosed, "Newly opened document claims to be closed." );
					Assert.AreEqual( TestBase.DisposableFilename, doc.Filename );
					Assert.IsTrue( doc.IsAuthoringDictionary, "Newly created data file should be in dictionary authoring mode." );
					Assert.IsTrue( doc.IsCompressed, "Newly created documents should default to being Compressed." );
				}
			}
			finally
			{
				if( File.Exists(TestBase.DisposableFilename) ) File.Delete(TestBase.DisposableFilename);
			}
		}
	}


}
