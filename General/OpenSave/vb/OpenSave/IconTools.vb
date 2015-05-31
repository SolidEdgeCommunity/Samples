' Building a Better ExtractAssociatedIcon
' Bradley Smith - 2010/07/28
'http://www.brad-smith.info/blog/archives/164

Imports System
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports Microsoft.Win32

''' <summary>
''' Defines a set of utility methods for extracting icons for files and file types.
''' </summary>
Public NotInheritable Class IconTools

    Private Sub New()
    End Sub
    #Region "Win32"

    ''' <summary>
    ''' Retrieve the handle to the icon that represents the file and the index of the icon within the system image list. The handle is copied to the hIcon member of the structure specified by psfi, and the index is copied to the iIcon member.
    ''' </summary>
    Friend Const SHGFI_ICON As UInteger = &H100
    ''' <summary>
    ''' Modify SHGFI_ICON, causing the function to retrieve the file's large icon. The SHGFI_ICON flag must also be set.
    ''' </summary>
    Friend Const SHGFI_LARGEICON As UInteger = &H0
    ''' <summary>
    ''' Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
    ''' </summary>
    Friend Const SHGFI_SMALLICON As UInteger = &H1

    ''' <summary>
    ''' Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
    ''' </summary>
    ''' <param name="pszPath">A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and file name. Both absolute and relative paths are valid.</param>
    ''' <param name="dwFileAttributes">A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h).</param>
    ''' <param name="psfi">The address of a SHFILEINFO structure to receive the file information.</param>
    ''' <param name="cbSizeFileInfo">The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.</param>
    ''' <param name="uFlags">The flags that specify the file information to retrieve.</param>
    ''' <returns>Nonzero if successful, or zero otherwise.</returns>
    <DllImport("shell32.dll")> _
    Private Shared Function SHGetFileInfo(ByVal pszPath As String, ByVal dwFileAttributes As UInteger, ByRef psfi As SHFILEINFO, ByVal cbSizeFileInfo As UInteger, ByVal uFlags As ShellIconSize) As IntPtr
    End Function

    ''' <summary>
    ''' Creates an array of handles to large or small icons extracted from the specified executable file, DLL, or icon file. 
    ''' </summary>
    ''' <param name="libName">The name of an executable file, DLL, or icon file from which icons will be extracted.</param>
    ''' <param name="iconIndex">The zero-based index of the first icon to extract. If this value is a negative number and either phiconLarge or phiconSmall is not NULL, the function begins by extracting the icon whose resource identifier is equal to the absolute value of nIconIndex. For example, use -3 to extract the icon whose resource identifier is 3.</param>
    ''' <param name="largeIcon">An array of icon handles that receives handles to the large icons extracted from the file. If this parameter is NULL, no large icons are extracted from the file.</param>
    ''' <param name="smallIcon">An array of icon handles that receives handles to the small icons extracted from the file. If this parameter is NULL, no small icons are extracted from the file.</param>
    ''' <param name="nIcons">The number of icons to be extracted from the file.</param>
    ''' <returns>If the nIconIndex parameter is -1, the phiconLarge parameter is NULL, and the phiconSmall  parameter is NULL, then the return value is the number of icons contained in the specified file. Otherwise, the return value is the number of icons successfully extracted from the file.</returns>
    <DllImport("Shell32.dll")> _
    Public Shared Function ExtractIconEx(ByVal libName As String, ByVal iconIndex As Integer, ByVal largeIcon() As IntPtr, ByVal smallIcon() As IntPtr, ByVal nIcons As UInteger) As Integer
    End Function

    ''' <summary>
    ''' Contains information about a file object.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)> _
    Private Structure SHFILEINFO
        ''' <summary>
        ''' A handle to the icon that represents the file.
        ''' </summary>
        Public hIcon As IntPtr
        ''' <summary>
        ''' The index of the icon image within the system image list.
        ''' </summary>
        Public iIcon As IntPtr
        ''' <summary>
        ''' An array of values that indicates the attributes of the file object.
        ''' </summary>
        Public dwAttributes As UInteger
        ''' <summary>
        ''' A string that contains the name of the file as it appears in the Windows Shell, or the path and file name of the file that contains the icon representing the file.
        ''' </summary>
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 260)> _
        Public szDisplayName As String
        ''' <summary>
        ''' A string that describes the type of file.
        ''' </summary>
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 80)> _
        Public szTypeName As String
    End Structure

    #End Region

    ''' <summary>
    ''' Returns an icon representation of the specified file.
    ''' </summary>
    ''' <param name="filename">The path to the file.</param>
    ''' <param name="size">The desired size of the icon.</param>
    ''' <returns>An icon that represents the file.</returns>
    Public Shared Function GetIconForFile(ByVal filename As String, ByVal size As ShellIconSize) As Icon
        Dim shinfo As New SHFILEINFO()
        SHGetFileInfo(filename, 0, shinfo, CUInt(Marshal.SizeOf(shinfo)), size)
        Return Icon.FromHandle(shinfo.hIcon)
    End Function

    ''' <summary>
    ''' Returns the default icon representation for files with the specified extension.
    ''' </summary>
    ''' <param name="extension">File extension (including the leading period).</param>
    ''' <param name="size">The desired size of the icon.</param>
    ''' <returns>The default icon for files with the specified extension.</returns>
    Public Shared Function GetIconForExtension(ByVal extension As String, ByVal size As ShellIconSize) As Icon
        ' locate the key corresponding to the file extension
        Using keyForExt As RegistryKey = Registry.ClassesRoot.OpenSubKey(extension)
            If keyForExt Is Nothing Then
                Return Nothing
            End If

            ' the extension will point to a class name, leading to another key
            Dim className As String = Convert.ToString(keyForExt.GetValue(Nothing))
            Using keyForClass As RegistryKey = Registry.ClassesRoot.OpenSubKey(className)
                If keyForClass Is Nothing Then
                    Return Nothing
                End If

                ' this key may have a DefaultIcon subkey
                Dim keyForIcon As RegistryKey = keyForClass.OpenSubKey("DefaultIcon")
                If keyForIcon Is Nothing Then
                    ' if not, see if it has a CLSID subkey
                    Dim keyForCLSID As RegistryKey = keyForClass.OpenSubKey("CLSID")
                    If keyForCLSID Is Nothing Then
                        Return Nothing
                    End If

                    ' the clsid value leads to another key that might contain DefaultIcon
                    Dim clsid As String = "CLSID\" & Convert.ToString(keyForCLSID.GetValue(Nothing))
                    keyForIcon = Registry.ClassesRoot.OpenSubKey(clsid & "\DefaultIcon")
                    If keyForIcon Is Nothing Then
                        Return Nothing
                    End If
                End If

                ' the value of DefaultIcon will either be a path only or a path with a resource index
                Dim defaultIcon() As String = Convert.ToString(keyForIcon.GetValue(Nothing)).Split(","c)
                Dim index As Integer = If(defaultIcon.Length > 1, Int32.Parse(defaultIcon(1)), 0)

                keyForIcon.Dispose()

                ' get the requested icon
                Dim [handles](0) As IntPtr
                If ExtractIconEx(defaultIcon(0), index,If(size = ShellIconSize.LargeIcon, [handles], Nothing),If(size = ShellIconSize.SmallIcon, [handles], Nothing), 1) > 0 Then
                    Return Icon.FromHandle([handles](0))
                Else
                    Return Nothing
                End If
            End Using
        End Using
    End Function
End Class

''' <summary>
''' Represents the different icon sizes that can be extracted using the ExtractAssociatedIcon method.
''' </summary>
Public Enum ShellIconSize As UInteger
    ''' <summary>
    ''' Specifies a small (16x16) icon.
    ''' </summary>
    SmallIcon = IconTools.SHGFI_ICON Or IconTools.SHGFI_SMALLICON
    ''' <summary>
    ''' Specifies a large (32x32) icon.
    ''' </summary>
    LargeIcon = IconTools.SHGFI_ICON Or IconTools.SHGFI_LARGEICON
End Enum