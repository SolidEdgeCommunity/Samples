# By default, PowerShell's execution policy is set to 'Restricted'.
# Execute command 'Get-ExecutionPolicy' to check the setting.
# Use 'Set-ExecutionPolicy Unrestricted' to enable running scripts.

Clear-Host

$application = $null

Write-Host 'Connecting to Solid Edge.'

try
{
    $application = [System.Runtime.InteropServices.Marshal]::GetActiveObject('SolidEdge.Application')
    Write-Host 'Connected to Solid Edge.'
}
catch [System.Exception]
{
    Write-Host $_.Exception.Message -foregroundcolor red

    if ($_.Exception.ErrorCode -eq -2147221021) #MK_E_UNAVAILABLE
    {
        Write-Host 'Solid Edge is not running. Starting a new instance.'
        $application = New-Object -Com SolidEdge.Application
        $application.Visible = $true
        $application.Activate()
    }
    else
    {
        Write-Host $_.Exception.Message
    }
}

if ($application -ne $null)
{
    Write-Host 'Creating a new SolidEdge.PartDocument.'

    $documents = $application.Documents
    $document = $documents.Add('SolidEdge.PartDocument')
}

Write-Host 'Done'