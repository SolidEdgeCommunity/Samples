Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        BeginInvoke(New MethodInvoker(AddressOf MainForm_Load_Async))
    End Sub

    ''' <summary>
    ''' Asynchronous version of MainForm_Load.
    ''' </summary>
    Private Sub MainForm_Load_Async()
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing

        ' Connect to or start Solid Edge.
        application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True)

        ' Ensure Solid Edge is visible.
        application.Visible = True

        ' Get a reference to the Documents collection.
        documents = application.Documents

        If documents.Count = 0 Then
            documents.Add("SolidEdge.PartDocument")
        End If

        externalPropertyGrid.SelectedObject = New ExternalExample()
        internalPropertyGrid.SelectedObject = New InternalExample()
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub externalPropertyGrid_PropertyValueChanged(ByVal s As Object, ByVal e As PropertyValueChangedEventArgs) Handles externalPropertyGrid.PropertyValueChanged
        externalPropertyGrid.Refresh()
    End Sub

    Private Sub internalPropertyGrid_PropertyValueChanged(ByVal s As Object, ByVal e As PropertyValueChangedEventArgs) Handles internalPropertyGrid.PropertyValueChanged
        internalPropertyGrid.Refresh()
    End Sub
End Class
