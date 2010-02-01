using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Linq;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Spss
{
	/// <summary>
	/// Used to convert a DataTable into a persisted SPSS.SAV file. 
	/// </summary>
	public class SpssConvert
	{
		private EventHandler notifyDoneCallback;
		/// <summary>
		/// The callback method for when the process is complete.
		/// </summary>
		protected EventHandler NotifyDoneCallback { get { return notifyDoneCallback; } }

		#region Construction
		/// <summary>
		/// Creates an instance of the <see cref="SpssConvert"/> class.
		/// </summary>
		/// <param name="notifyDoneCallback">
		/// The callback method for when the process is complete.
		/// </param>
		/// <remarks>
		/// Used internally for asynchronous operations.
		/// </remarks>
		internal SpssConvert(EventHandler notifyDoneCallback) 
		{
			this.notifyDoneCallback = notifyDoneCallback;
		}
		#endregion

		private delegate void ToFileAsyncDelegate(DataTable dataTable, IEnumerable<DataRow> data,
			 string spssSavFilename, Action<SpssVariable> fillInMetaDataCallBack);

		/// <summary>
		/// Call to convert data to SPSS format using a passed in SQL query to provide the data.
		/// </summary>
		/// <param name="dataTable">The DataTable to convert to SPSS format</param>
		/// <param name="spssSavFilename">The fully-qualified target .SAV file to save results to</param>
		/// <param name="fillInMetaDataCallBack">Callback function to provide per-variable metadata</param>
		public static void ToFile(DataTable dataTable,
			string spssSavFilename, Action<SpssVariable> fillInMetaDataCallBack)
		{
			ToFile(dataTable, dataTable.Rows.Cast<DataRow>(), spssSavFilename, fillInMetaDataCallBack);
		}

		/// <summary>
		/// Call to convert data to SPSS format using a passed in SQL query to provide the data.
		/// </summary>
		/// <param name="dataTable">The DataTable to convert to SPSS format</param>
		/// <param name="data">An enumerable list of DataRows.</param>
		/// <param name="spssSavFilename">The fully-qualified target .SAV file to save results to</param>
		/// <param name="fillInMetaDataCallBack">Callback function to provide per-variable metadata</param>
		public static void ToFile(DataTable dataTable, IEnumerable<DataRow> data,
			string spssSavFilename, Action<SpssVariable> fillInMetaDataCallBack)
		{
			// Remove the file if it already exists.
			if( File.Exists( spssSavFilename ) ) File.Delete( spssSavFilename );
			// Open up the document with "using" so that it will definitely close afterward.
			using( SpssDataDocument Sav = SpssDataDocument.Create( spssSavFilename ) )
			{
				// Create the schema from the table, passing in a callback
				// function for filling in each variable's metadata
				Sav.Variables.ImportSchema( dataTable, fillInMetaDataCallBack );
				// Import data
				Sav.CommitDictionary();
				Sav.ImportData( dataTable, data );
			} 
		}

		/// <summary>
		/// Call to asynchronously convert data to SPSS format using a passed in SQL query 
		/// to provide the data.
		/// </summary>
		/// <param name="dataTable">
		/// The DataTable to convert to SPSS format
		/// </param>
		/// <param name="data">An enumerable list of DataRows.</param>
		/// <param name="spssSavFilename">
		/// The fully-qualified target .SAV file to save results to
		/// </param>
		/// <param name="fillInMetaDataCallback">
		/// Callback function to provide per-variable metadata
		/// </param>
		/// <param name="notifyDoneCallback">
		/// The method to call when the process is complete.
		/// </param>
		///	<returns>
		///	Returns a handle to poll the status of the conversion.
		///	</returns>
		public static IAsyncResult ToFileAsync(DataTable dataTable, IEnumerable<DataRow> data, string spssSavFilename,
			Action<SpssVariable> fillInMetaDataCallback, EventHandler notifyDoneCallback)
		{
			// Spin off an asynchronous thread to do the work 
			// Be sure to use a callback function, even if we don't care when this 
			// conversion is done, since we must call EndInvoke.
			ToFileAsyncDelegate dlgt = new ToFileAsyncDelegate( ToFile );

			// Instantiate an instance of this class, to save the vbNotifyDone parameter
			// so that we know who to tell when this operation is complete.
			SpssConvert instance = new SpssConvert( notifyDoneCallback );
			return dlgt.BeginInvoke( dataTable, data, spssSavFilename, fillInMetaDataCallback, 
				new AsyncCallback( instance.ToFileAsyncCB ), dlgt );
		}

		private void ToFileAsyncCB(IAsyncResult ar)
		{
			ToFileAsyncDelegate dlgt = (ToFileAsyncDelegate) ar.AsyncState;

			// Call EndInvoke, since the docs say we MUST
			dlgt.EndInvoke(ar);

			// Inform caller that the asynchronous operation is now complete
			if( NotifyDoneCallback != null ) NotifyDoneCallback(this, null);
		}

		/// <summary>
		/// Converts a <see cref="DataTable"/> to an SPSS .SAV file.
		/// </summary>
		/// <param name="dataTable">
		/// The <see cref="DataTable"/> with the schema and data to fill into the SPSS .SAV file.
		/// </param>
		/// <param name="data">An enumerable list of DataRows.</param>
		/// <param name="fillInMetaDataCallBack">
		/// The callback method that will provide additional metadata on each column.
		/// </param>
		/// <returns>
		/// A <see cref="MemoryStream"/> containing the contents of the SPSS .SAV data file.
		/// </returns>
		/// <remarks>
		/// A temporary file is created during this process, but is guaranteed to be removed
		/// as the method returns.
		/// </remarks>
		public static MemoryStream ToStream(DataTable dataTable, IEnumerable<DataRow> data, Action<SpssVariable> fillInMetaDataCallBack)
		{
			// Create a temporary file for the SPSS data that we will generate.
			using( TempFileCollection tfc = new TempFileCollection() )
			{
				string filename = tfc.AddExtension("sav", false);
				ToFile(dataTable, data, filename, fillInMetaDataCallBack);

				// Now read the file into memory
				using( FileStream fs = File.OpenRead(filename) )
				{
					MemoryStream ms = new MemoryStream((int)fs.Length);
					int b = 0;
					while( (b = fs.ReadByte()) >= 0 )
						ms.WriteByte((byte)b);

					// reset to start of stream.
					ms.Position = 0;

					// return the memory stream.  All temporary files will delete as we exit.
					return ms;
				}
			}
		}
		
		/// <summary>
		/// Converts the metadata in an SPSS .SAV data file into a DDI codebook.
		/// </summary>
		/// <param name="spssSav">
		/// The stream containing the SPSS .SAV data file.
		/// </param>
		/// <returns>
		/// The <see cref="XmlDocument"/> containing all the metadata.
		/// </returns>
		public static XmlDocument ToDdi(Stream spssSav)
		{
			// To read an SPSS file, spssio32.dll requires that the file is actually on disk, 
			// so persist this stream to a temporary file on disk.
			using( TempFileCollection tfc = new TempFileCollection() )
			{
				string filename = tfc.AddExtension("sav", false);
				using( FileStream fs = new FileStream(filename, FileMode.CreateNew) )
				{
					int b;
					while( (b = spssSav.ReadByte()) >= 0 )
						fs.WriteByte((byte)b);
				}
				return ToDdi(filename);
				// leaving this block will remove the temporary file automatically
			}
		}

		/// <summary>
		/// Converts the metadata in an SPSS .SAV data file into a DDI codebook.
		/// </summary>
		/// <param name="spssSavFilename">
		/// The filename of the SPSS .SAV data file.
		/// </param>
		/// <returns>
		/// The <see cref="XmlDocument"/> containing all the metadata.
		/// </returns>
		public static XmlDocument ToDdi(string spssSavFilename)
		{
						
			const string ddiNamespace = "http://www.icpsr.umich.edu/DDI";
			if( spssSavFilename == null ) throw new ArgumentNullException( "spssSavFilename" );
			XmlDocument ddi = new XmlDocument();
			// Build initial ddi document up.
			// Open up SPSS file and fill in the ddi var tags.
			using (SpssDataDocument doc = SpssDataDocument.Open(spssSavFilename, SpssFileAccess.Read)) {
				ddi.PreserveWhitespace = true;
				//Read from the embedded xml file: blankDdi.xml into the ddi document
				ddi.LoadXml(EmbeddedResources.LoadFileFromAssemblyWithNamespace("/blankDdi.xml", Project.DefaultNamespace));
				//This is where the hard coding ends and methods are called to extract data from the sav file

				XmlElement xmlRoot = ddi.DocumentElement;
				XmlNamespaceManager xmlNS = new XmlNamespaceManager(ddi.NameTable);
				xmlNS.AddNamespace("ddi", ddiNamespace);
				XmlNode nData = xmlRoot.SelectSingleNode(@"ddi:dataDscr", xmlNS);

				foreach (SpssVariable var in doc.Variables) {
					string nameOfVar = var.Name;

					//variable name and its ID and then if its a numeric : its interval
					XmlElement variable = ddi.CreateElement("ddi:var", ddiNamespace);
					variable.SetAttribute("ID", string.Empty, nameOfVar);
					variable.SetAttribute("name", string.Empty, nameOfVar);

					//This is the variable that holds the characteristic whether the variable has discrete or continuous interval
					int Dec;
					if (var is SpssNumericVariable) {
						Dec = ((SpssNumericVariable)var).DecimalPlaces;
						string interval = string.Empty;
						if (Dec == 0) {
							interval = "discrete";
						} else {
							interval = "contin";
						}
						variable.SetAttribute("intrvl", string.Empty, interval);
					}

					//for the location width part
					XmlElement location = ddi.CreateElement("ddi:location", ddiNamespace);
					int Wid = var.ColumnWidth;
					location.SetAttribute("width", Wid.ToString());
					variable.AppendChild(location);

					//label of the variable is set in "varlabel" and extracted using var.Label 
					XmlElement varLabel = ddi.CreateElement("ddi:labl", ddiNamespace);
					varLabel.InnerText = var.Label;
					variable.AppendChild(varLabel);

					foreach (var response in var.GetValueLabels()) {
						XmlElement answer = ddi.CreateElement("ddi:catgry", ddiNamespace);

						//catValue(category Value) is the element storing the text i.e. option number
						XmlElement catValue = ddi.CreateElement("ddi:catValu", ddiNamespace);
						catValue.InnerText = response.Key;
						answer.AppendChild(catValue);

						//catLabel(category Label) is the element storing the text i.e. name of answer
						XmlElement catLabel = ddi.CreateElement("ddi:labl", ddiNamespace);
						catLabel.InnerText = response.Value;
						answer.AppendChild(catLabel);

						//appending the answer option to the parent "variable" node i.e. the question node
						variable.AppendChild(answer);
					}

					// end of extracting the response values for each variable 

					XmlElement varFormat = ddi.CreateElement("ddi:varFormat", ddiNamespace);

					if (var is SpssNumericVariable) {
						varFormat.SetAttribute("type", "numeric");
					} else if (var is SpssStringVariable) {
						varFormat.SetAttribute("type", "character");
					} else {
						throw new NotSupportedException("Variable " + nameOfVar + " is not a string or a numeric variable type.");
					}
					variable.AppendChild(varFormat);

					nData.AppendChild(variable);

				}
				
				//end of extraction of each variable and now we have put all the variable data into ndata
				// Return the completed ddi file.
				return ddi;
			}
		}

		/// <summary>
		/// Converts the metadata in an SPSS .SAV data file into a DataTable.
		/// </summary>
		/// <param name="spssSav">
		/// The stream containing the SPSS .SAV data file.
		/// </param>
		/// <returns>
		/// The <see cref="DataTable"/> containing all the metadata.
		/// </returns>
		public static DataTable ToDataTable(Stream spssSav)
		{
			if (spssSav == null) throw new ArgumentNullException("spssSav");
			// To read an SPSS file, spssio32.dll requires that the file is actually on disk, 
			// so persist this stream to a temporary file on disk.
			using (TempFileCollection tfc = new TempFileCollection())
			{
				string filename = tfc.AddExtension("sav", false);
				using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
				{
					int b;
					while ((b = spssSav.ReadByte()) >= 0) {
						fs.WriteByte((byte)b);
					}
				}
				return ToDataTable(filename);

				// leaving this block will remove the temporary file automatically
			}
		}

		/// <summary>
		/// Converts the metadata in an SPSS .SAV data file into a DataTable.
		/// </summary>
		/// <param name="spssSavFilename">
		/// The filename of the SPSS .SAV data file.
		/// </param>
		/// <returns>
		/// The <see cref="DataTable"/> containing all the metadata.
		/// </returns>
		public static DataTable ToDataTable(string spssSavFilename)
		{
			if (spssSavFilename == null) throw new ArgumentNullException("spssSavFilename");
			DataTable dataTable = new DataTable();
			using (SpssDataDocument doc = SpssDataDocument.Open(spssSavFilename, SpssFileAccess.Read)) {
				ToDataTable(doc, dataTable);
			}

			// Return the completed DataTable.
			return dataTable;
		}

		public static void ToDataTable(SpssDataDocument doc, DataTable dataTable) {
			if (doc == null) {
				throw new ArgumentNullException("doc");
			}
			if (dataTable == null) {
				throw new ArgumentNullException("dataTable");
			}

			// Build initial DataTable up.
			// Fill in the metadata.
			//set up the columns with the metadata
			foreach (SpssVariable var in doc.Variables) {
				string nameOfVar = var.Name;

				//add a column of the variable name to the DataTable
				DataColumn dataColumn = dataTable.Columns.Add(nameOfVar);

				//label of the variable is set in "varlabel" and extracted using var.Label 
				dataColumn.Caption = var.Label;

				//set the type of the column
				if (var is SpssNumericVariable) {
					dataColumn.DataType = typeof(double);
				} else if (var is SpssStringVariable) {
					dataColumn.DataType = typeof(string);
				} else if (var is SpssDateVariable) {
					dataColumn.DataType = typeof(DateTime);
				} else {
					throw new NotSupportedException("Variable " + nameOfVar + " is not a string or a numeric variable type.");
				}
			}//end of extraction of metadata

			//add data into the DataTable
			foreach (SpssCase rowCase in doc.Cases) {
				List<object> values = new List<object>();
				foreach (SpssVariable column in doc.Variables) {
					values.Add(rowCase[column.Name]);
				}
				dataTable.Rows.Add(values.ToArray());
			}
		}
	}
}
