param($installPath, $toolsPath, $package, $project)

Import-Module (Join-Path $toolsPath VS.psd1)
$nativeBinDirectory = Join-Path $installPath "NativeBinaries"
if ($project.Type -eq 'Web Site') {
    $projectRoot = Get-ProjectRoot $project
    if (!$projectRoot) {
        return;
    }

    $binDirectory = Join-Path $projectRoot "bin"
    $libDirectory = Join-Path $installPath "lib\net35"
    Add-FilesToDirectory $libDirectory $binDirectory
    Add-FilesToDirectory $nativeBinDirectory $binDirectory
}
elseif($project.ExtenderNames -contains "WebApplication") {
	$depAsm = Ensure-Folder $Project "_bin_deployableAssemblies";
	if($depAsm) {
		$win64 = Ensure-Folder $depAsm "win64";
		if($win64) {
			$win64dir = (Join-Path $nativeBinDirectory "win64")
			
			Add-ProjectItem $win64 (Join-Path $win64dir "icudt38.dll");
			Add-ProjectItem $win64 (Join-Path $win64dir "icuin38.dll");
			Add-ProjectItem $win64 (Join-Path $win64dir "icuuc38.dll");
			Add-ProjectItem $win64 (Join-Path $win64dir "spssio64.dll");
		}
		$win32 = Ensure-Folder $depAsm "win32";
		if($win32) {
			$win32dir = (Join-Path $nativeBinDirectory "win32")
			Add-ProjectItem $win32 (Join-Path $win32dir "icudt38.dll");
			Add-ProjectItem $win32 (Join-Path $win32dir "icuin38.dll");
			Add-ProjectItem $win32 (Join-Path $win32dir "icuuc38.dll");
			Add-ProjectItem $win32 (Join-Path $win32dir "spssio32.dll");
		}
	}
}
else {
    Add-PostBuildEvent $project $installPath
}
Remove-Module VS