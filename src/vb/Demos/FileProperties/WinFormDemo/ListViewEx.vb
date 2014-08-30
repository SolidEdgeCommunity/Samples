Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Friend Class ListViewEx
	Inherits ListView

	<DllImport("uxtheme.dll", CharSet := CharSet.Unicode)> _
	Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal pszSubAppName As String, ByVal pszSubIdList As String) As Integer
	End Function

	Public Sub New()
		MyBase.New()
	End Sub

	Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
		MyBase.OnHandleCreated(e)

		DoubleBuffered = True

		If (Not Me.DesignMode) AndAlso Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.Major >= 6 Then
			SetWindowTheme(Me.Handle, "explorer", Nothing)
		End If
	End Sub
End Class
