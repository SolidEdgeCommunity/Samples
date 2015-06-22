Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MyEdgeBarControl
    Inherits SolidEdgeCommunity.AddIn.EdgeBarControl

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MyEdgeBarControl_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Trick to use the default system font.
        Me.Font = SystemFonts.MessageBoxFont
    End Sub

    Private Sub MyEdgeBarControl_AfterInitialize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.AfterInitialize
        ' These properties are not initialized until AfterInitialize is called.
'INSTANT VB NOTE: The variable edgeBarPage was renamed since Visual Basic does not handle local variables named the same as class members well:
        Dim edgeBarPage_Renamed = Me.EdgeBarPage
'INSTANT VB NOTE: The variable document was renamed since Visual Basic does not handle local variables named the same as class members well:
        Dim document_Renamed = Me.Document
        Dim application = document_Renamed.Application

        ' Populate the richtextbox with some text.
        Me.richTextBox1.Text = application.GetGlobalParameter(Of String)(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo)
    End Sub
End Class
