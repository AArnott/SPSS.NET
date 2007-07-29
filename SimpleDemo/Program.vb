Module Program

	Sub Main()
		Console.WriteLine("SPSS file writing demo:")
		If File.Exists("example.sav") Then File.Delete("example.sav")
		Using doc As SpssDataDocument = SpssDataDocument.Create("example.sav")
			CreateMetaData(doc)
			CreateData(doc)
		End Using
		Console.WriteLine("Examine example.sav for the results.")
		Console.WriteLine("SPSS file reading demo:")
		Using doc As SpssDataDocument = SpssDataDocument.Open(GetFileName, SpssFileAccess.Read)
			PrintMetaData(doc)
			PrintData(doc)
		End Using
		Console.WriteLine("SPSS dictionary copying demo:")
		If File.Exists("example2.sav") Then File.Delete("example2.sav")
		Using doc As SpssDataDocument = SpssDataDocument.Create("example2.sav", GetFileName)
			PrintMetaData(doc)
		End Using

		Console.WriteLine("Demo concluded.  Press any key to end.")
		Console.ReadKey()


	End Sub

	Function GetFileName() As String
		If File.Exists("..\..\demo.sav") Then Return "..\..\demo.sav"
		If File.Exists("demo.sav") Then Return "demo.sav"
		Throw New ApplicationException("Cannot find demo.sav file.")
	End Function
	Sub CreateMetaData(ByVal doc As SpssDataDocument)
		' Define dictionary
		Dim v1 As New SpssStringVariable()
		v1.Name = "v1"
		v1.Label = "What is your name?"
		doc.Variables.Add(v1)
		Dim v2 As New SpssNumericVariable
		v2.Name = "v2"
		v2.Label = "How old are you?"
		doc.Variables.Add(v2)
		Dim v3 As New SpssNumericVariable
		v3.Name = "v3"
		v3.Label = "What is your gender?"
		v3.ValueLabels.Add(1, "Male")
		v3.ValueLabels.Add(2, "Female")
		doc.Variables.Add(v3)
		Dim v4 As New SpssDateVariable
		v4.Name = "v4"
		v4.Label = "What is your birthdate?"
		doc.Variables.Add(v4)
		' Add some data
		doc.CommitDictionary()
	End Sub
	Sub CreateData(ByVal doc As SpssDataDocument)
		Dim case1 As SpssCase = doc.Cases.[New]()
		case1("v1") = "Andrew"
		case1("v2") = 24
		case1("v3") = 1
		case1("v4") = DateTime.Parse("1/1/1982 7:32 PM")
		case1.Commit()
		Dim case2 As SpssCase = doc.Cases.[New]()
		case2("v1") = "Cindy"
		case2("v2") = 21
		case2("v3") = 2
		case2("v4") = DateTime.Parse("12/31/2002")
		case2.Commit()
	End Sub

	Sub PrintMetaData(ByVal doc As SpssDataDocument)
		Console.WriteLine("Variables:")
		For Each var As SpssVariable In doc.Variables
			Console.WriteLine("{0}" & vbTab & "{1}", var.Name, var.Label)
			If TypeOf var Is SpssNumericVariable Then
				Dim varNum As SpssNumericVariable = CType(var, SpssNumericVariable)
				For Each label As DictionaryEntry In varNum.ValueLabels
					Console.WriteLine(vbTab & label.Key.ToString & vbTab & label.Value.ToString)
				Next
			End If
		Next
	End Sub
	Sub PrintData(ByVal doc As SpssDataDocument)
		For Each var As SpssVariable In doc.Variables
			Console.Write(var.Name & vbTab)
		Next
		Console.WriteLine()

		For Each row As SpssCase In doc.Cases
			For Each var As SpssVariable In doc.Variables
				If (row(var.Name) Is Nothing) Then
					Console.Write("<SYSMISS>")
				Else
					Console.Write(row(var.Name))
				End If
				Console.Write(vbTab)
			Next
			Console.WriteLine()
		Next
	End Sub
End Module
