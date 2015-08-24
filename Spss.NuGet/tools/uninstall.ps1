param($installPath, $toolsPath, $package, $project)

Import-Module (Join-Path $toolsPath VS.psd1)
if ($project.Type -eq 'Web Site') {
    $projectRoot = Get-ProjectRoot $project
    if (!$projectRoot) {
        return;
    }

    $binDirectory = Join-Path $projectRoot "bin"
    $libDirectory = Join-Path $installPath "lib\net35"
    $nativeBinDirectory = Join-Path $installPath "NativeBinaries"

    Remove-FilesFromDirectory $libDirectory $binDirectory
    Remove-FilesFromDirectory $nativeBinDirectory $binDirectory
}
elseif($project.ExtenderNames -contains "WebApplication") {
	$depAsm = Get-ChildProjectItem $Project "_bin_deployableAssemblies";
	if($depAsm) {
		$win64 = Get-ChildProjectItem $depAsm "win64";
		if($win64) {
			Remove-Child $win64 "icudt38.dll";
			Remove-Child $win64 "icuin38.dll";
			Remove-Child $win64 "icuuc38.dll";
			Remove-Child $win64 "spssio64.dll";
			Remove-EmptyFolder $win64;
		}
		$win32 = Get-ChildProjectItem $depAsm "win32";
		if($win32) {
			Remove-Child $win32 "icudt38.dll";
			Remove-Child $win32 "icuin38.dll";
			Remove-Child $win32 "icuuc38.dll";
			Remove-Child $win32 "spssio32.dll";
			Remove-EmptyFolder $win32;
		}
	}
	Remove-EmptyFolder $depAsm
}
else {
    Remove-PostBuildEvent $project $installPath
}
Remove-Module VS