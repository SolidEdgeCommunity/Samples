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

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        MyBase.OnHandleCreated(e)

        DoubleBuffered = True
        If Not Me.DesignMode AndAlso Environment.OSVersion.Platform = PlatformID.Win32NT AndAlso Environment.OSVersion.Version.Major >= 6 Then
            SetWindowTheme(Me.Handle, "explorer", Nothing)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        If e.KeyCode = Keys.A AndAlso e.Control Then
            SelectAllItems()
        ElseIf e.KeyCode = Keys.C AndAlso e.Control Then
            CopySelectedItemsToClipboard()
        ElseIf e.KeyCode = Keys.Escape Then
            SelectedItems.Clear()
        End If
    End Sub

    Public Overloads Sub AutoResizeColumns()
        For Each header As ColumnHeader In Me.Columns
            header.Width = -2
        Next header
    End Sub

    Public Sub CopySelectedItemsToClipboard()
        Dim clipboardText As New StringBuilder()

        For Each item As ListViewItem In SelectedItems
            Dim line As New StringBuilder()

            For Each subItem As ListViewItem.ListViewSubItem In item.SubItems
                line.AppendFormat("{0}" & ControlChars.Tab, subItem.Text)
            Next subItem

            clipboardText.AppendLine(line.ToString())
        Next item

        Clipboard.Clear()
        Clipboard.SetText(clipboardText.ToString(), TextDataFormat.UnicodeText)
    End Sub

    Public Sub SelectAllItems()
        If MultiSelect = True Then
            BeginUpdate()
            For Each item As ListViewItem In Items
                item.Selected = True
            Next item
            EndUpdate()
        End If
    End Sub
End Class
