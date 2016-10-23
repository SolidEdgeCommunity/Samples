function DoBuild
{
    param
    (
        [parameter(Mandatory=$true)]
        [String] $path
    )
    process
    {
        $msBuildExe = 'C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe'

        Write-Host "Cleaning $($path)" -foregroundcolor green
        & "$($msBuildExe)" "$($path)" /t:Clean /m /consoleloggerparameters:ErrorsOnly

        Write-Host "Building $($path)" -foregroundcolor green
        & "$($msBuildExe)" "$($path)" /t:Build /m /consoleloggerparameters:ErrorsOnly
    }
}

$samples_path = $PSScriptRoot

cls

Write-Host "Processing folder $samples_path"

pwd

foreach ($solution in Get-ChildItem $samples_path -Recurse -Include *.sln)
{
    DoBuild $solution.FullName
}