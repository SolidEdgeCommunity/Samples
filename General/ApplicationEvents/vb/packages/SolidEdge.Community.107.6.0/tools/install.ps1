param($installPath, $toolsPath, $package, $project)

$outputtype = $project.Properties.Item("OutputType").Value

# SLangProj.prjOutputType.prjOutputTypeLibrary for class libraries.
# If the OutputType project is set to prjOutputTypeLibrary, notify of 107.6.0 changes.
if ($outputtype -eq 2)
{
    $message = "As of 107.6.0 release, addin functionality that previously existed in this package has been moved to the 'SolidEdge.Community.AddIn' NuGet package. If you were using this package for addin functionality, you will need to install the 'SolidEdge.Community.AddIn' package manually before your code will compile."
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
    [Windows.Forms.MessageBox]::Show($message, "Notice", [Windows.Forms.MessageBoxButtons]::OK, [Windows.Forms.MessageBoxIcon]::Information)
}