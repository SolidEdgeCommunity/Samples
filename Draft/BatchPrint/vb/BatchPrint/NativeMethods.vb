Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

''' <summary>
''' Static class containing PInvoke signatures.
''' </summary>

Friend NotInheritable Class NativeMethods
    #Region "Winuser.h"

    Public Const WM_ERASEBKGND As Integer = &H14

    #End Region 'Winuser.h

    #Region "Shellapi.h"

    Public Const SHGFI_ICON As Integer = &H100
    Public Const SHGFI_DISPLAYNAME As Integer = &H200
    Public Const SHGFI_TYPENAME As Integer = &H400
    Public Const SHGFI_ATTRIBUTES As Integer = &H800
    Public Const SHGFI_ICONLOCATION As Integer = &H1000
    Public Const SHGFI_EXETYPE As Integer = &H2000
    Public Const SHGFI_SYSICONINDEX As Integer = &H4000
    Public Const SHGFI_LINKOVERLAY As Integer = &H8000
    Public Const SHGFI_SELECTED As Integer = &H10000
    Public Const SHGFI_ATTR_SPECIFIED As Integer = &H20000
    Public Const SHGFI_LARGEICON As Integer = &H0
    Public Const SHGFI_SMALLICON As Integer = &H1
    Public Const SHGFI_OPENICON As Integer = &H2
    Public Const SHGFI_SHELLICONSIZE As Integer = &H4
    Public Const SHGFI_PIDL As Integer = &H8
    Public Const SHGFI_USEFILEATTRIBUTES As Integer = &H10

    <StructLayout(LayoutKind.Sequential, CharSet := CharSet.Auto)> _
    Public Structure SHFILEINFO
        Public hIcon As IntPtr
        Public iIcon As Integer
        Public dwAttributes As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 260)> _
        Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst := 80)> _
        Public szTypeName As String
    End Structure

    <DllImport("shell32.dll", CharSet := CharSet.Auto)> _
    Public Shared Function SHGetFileInfo(ByVal pszPath As String, ByVal dwFileAttributes As Integer, <System.Runtime.InteropServices.Out()> ByRef psfi As SHFILEINFO, ByVal cbFileInfo As Integer, ByVal uFlags As Integer) As IntPtr
    End Function

    #End Region ' Shellapi.h

    #Region "Windows.h"

    Public Const FILE_ATTRIBUTE_NORMAL As Integer = &H80 ' Normal file

    #End Region ' Windows.h

    #Region "Uxtheme.h"

    <DllImport("uxtheme.dll", CharSet := CharSet.Auto, SetLastError := True, ExactSpelling := True)> _
    Public Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal subApp As String, ByVal subIdList As String) As IntPtr
    End Function

    #End Region 'Uxtheme.h

End Class
