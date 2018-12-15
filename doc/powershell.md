# Scripting .SAV file access from Windows PowerShell

Chuck Moore shares a powershell script sample for reading an .SAV file from PowerShell using this project:

```ps1

# load up the SPSS.NET widgets and gizmos (this path assumes you've restored it with nuget previously)
$_spsslibrary = "$env:USERPROFILE\.nuget\packages\spss.net\1.0.15-beta\lib\net45\Spss.dll"

try { [system.reflection.assembly]::loadfrom( $_spsslibrary ) | out-null }
catch [exception] {
    write-host ( "Error: Unable to load required SPSS-file access library '" + $_spsslibrary + ". Can not continue." )
    exit 999
}

# define some fixed file-access-parameters used for each file
$_fileMode = [System.IO.FileMode]::Open
$_fileAccess = [System.IO.FileAccess]::Read
$_fileSharing = [IO.FileShare]::None

# get some ‘SAV’ files, then walk through them
$_srcfiles = Get-ChildItem ‘C:\Whereever_the_SAV_files_are\*.sav’
foreach ( $_file in $_srcfiles ) {
    write-host ( "reading : " + $_file.fullname )

    $_fileStream = New-Object IO.FileStream( $_file.fullname, $_fileMode, $_fileAccess, $_fileSharing )
    $_spssReader = New-Object SpssLib.DataReader.SpssReader( $_fileStream )

    # get this file’s variables
    $_spssVariables = $_spssReader.variables

    foreach ( $_record in $_spssReader.records ) {
        foreach ( $_variable in $_spssVariables ) {
            $_data = $_record.getvalue( $_variable )
            if ( $_data -eq $null )
            { $_data = "NULL" }
            else { $_data = $_data.tostring() }
            Write-host ( $_variable.name + " : " + $_data )

        }
    }

    $_spssReader.dispose()
    $_fileStream.close()
    $_fileStream.dispose()
}
```

Consider exploring the API using PowerShell's reflection functions:

```ps
$_spssVariables | get-member

TypeName: SpssLib.SpssDataset.Variable

   Name             MemberType Definition
----             ---------- ----------
Equals           Method     bool Equals(System.Object obj)
GetHashCode      Method     int GetHashCode()
GetType          Method     type GetType()
GetValue         Method     System.Object GetValue(System.Object value)
IsDate           Method     bool IsDate()
ToString         Method     string ToString()
Alignment        Property   SpssLib.SpssDataset.Alignment Alignment {get;set;}
Index            Property   int Index {get;}
Label            Property   string Label {get;set;}
MeasurementType  Property   SpssLib.SpssDataset.MeasurementType MeasurementType {get;set;}
MissingValues    Property   double[] MissingValues {get;}
MissingValueType Property   SpssLib.SpssDataset.MissingValueType MissingValueType {get;set;}
Name             Property   string Name {get;set;}
PrintFormat      Property   SpssLib.SpssDataset.OutputFormat PrintFormat {get;set;}
TextWidth        Property   int TextWidth {get;set;}
Type             Property   SpssLib.SpssDataset.DataType Type {get;set;}
ValueLabels      Property   System.Collections.Generic.IDictionary[double,string] ValueLabels {get;set;}
Width            Property   int Width {get;set;}
WriteFormat      Property   SpssLib.SpssDataset.OutputFormat WriteFormat {get;set;}
```
