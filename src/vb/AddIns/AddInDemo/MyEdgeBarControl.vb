Imports SolidEdgeFramework.Extensions
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace AddInDemo
	Partial Public Class MyEdgeBarControl
		Inherits SolidEdgeCommunity.AddIn.EdgeBarControl

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub MyEdgeBarControl_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' You can set the tooltip in the designer or at runtime.
			Me.ToolTip = "My EdgeBar Control"

			' Trick to use the default system font.
			Me.Font = SystemFonts.MessageBoxFont
		End Sub

		Private Sub MyEdgeBarControl_AfterInitialize(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.AfterInitialize
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
End Namespace
