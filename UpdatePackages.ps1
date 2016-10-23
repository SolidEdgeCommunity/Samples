function DoUpdate
{
    param
    (
        [parameter(Mandatory=$true)]
        [String] $path
    )
    process
    {
        Write-Host "nuget restore $($path)" -foregroundcolor green
        nuget restore "$($path)"

        Write-Host "nuget update $($path)" -foregroundcolor green
        nuget update "$($path)"
    }
}

$samples_path = $PSScriptRoot

cls
Write-Host $PSCommandPath
Write-Host "Processing folder $samples_path"

pwd

foreach ($solution in Get-ChildItem $samples_path -Recurse -Include *.sln)
{
    DoUpdate $solution.FullName
}