Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Friend Module ControlExtensions
    <System.Runtime.CompilerServices.Extension> _
    Public Sub [Do](Of TControl As Control)(ByVal control As TControl, ByVal action As Action(Of TControl))
        If control.InvokeRequired Then
            control.BeginInvoke(action, control)
        Else
            action(control)
        End If
    End Sub
End Module
