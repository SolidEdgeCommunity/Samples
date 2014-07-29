Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Namespace AddInDemo
	<ComVisible(True), Guid("BF1C1BB8-75EE-444A-8DCE-0F1521D0764B"), ProgId("SolidEdgeCommunity.AddInDemo.MyAddIn")> _
	Public Class MyAddIn ' Must be unique! -  Must be unique!
		Inherits SolidEdgeCommunity.AddIn.SolidEdgeAddIn

		''' <summary>
		''' Called when the addin is first loaded by Solid Edge.
		''' </summary>
		Public Overrides Sub OnConnection(ByVal application As SolidEdgeFramework.Application, ByVal ConnectMode As SolidEdgeFramework.SeConnectMode, ByVal AddInInstance As SolidEdgeFramework.AddIn)
			' If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
			' will not initialize properly.
            AddInEx.GuiVersion = 2
		End Sub

		''' <summary>
		''' Called when the addin first connects to a new Solid Edge environment.
		''' </summary>
		Public Overrides Sub OnConnectToEnvironment(ByVal environment As SolidEdgeFramework.Environment, ByVal firstTime As Boolean)
		End Sub

		''' <summary>
		''' Called when the addin is about to be unloaded by Solid Edge.
		''' </summary>
		Public Overrides Sub OnDisconnection(ByVal DisconnectMode As SolidEdgeFramework.SeDisconnectMode)
		End Sub

		''' <summary>
		''' Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
		''' </summary>
		Public Overrides Sub OnCreateEdgeBarPage(ByVal controller As SolidEdgeCommunity.AddIn.EdgeBarController, ByVal document As SolidEdgeFramework.SolidEdgeDocument)
			' Note: Confirmed with Solid Edge development, OnCreateEdgeBarPage does not get called when Solid Edge is first open and the first document is open.
			' i.e. Under the hood, SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() is not getting called.
			' As an alternative, you can call MyAddIn.Instance.EdgeBarController.Add() in some other event if you need.

			' Get the document type of the passed in document.
			Dim documentType = document.Type
			Dim imageId = 1

			' Depending on the document type, you may have different edgebar controls.
			Select Case documentType
				Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument, SolidEdgeFramework.DocumentTypeConstants.igPartDocument
					controller.Add(Of MyEdgeBarControl)(document, imageId)
			End Select
		End Sub

		''' <summary>
		''' Called directly after OnConnectToEnvironment() to give you an opportunity to configure a ribbon for a specific environment.
		''' </summary>
		Public Overrides Sub OnCreateRibbon(ByVal controller As SolidEdgeCommunity.AddIn.RibbonController, ByVal environmentCategory As Guid, ByVal firstTime As Boolean)
			' Depending on environment, you may or may not want to load different ribbons.
			If environmentCategory.Equals(SolidEdge.CATID.SEDraftGuid) Then
				' Draft Environment
				controller.Add(Of DraftRibbon)(environmentCategory, firstTime)
			ElseIf environmentCategory.Equals(SolidEdge.CATID.SEPartGuid) Then
				' Traditional Part Environment
				controller.Add(Of PartRibbon)(environmentCategory, firstTime)
			ElseIf environmentCategory.Equals(SolidEdge.CATID.SEDMPartGuid) Then
				' Synchronous Part Environment
				controller.Add(Of PartRibbon)(environmentCategory, firstTime)
			End If
		End Sub

		''' <summary>
		''' Called when regasm.exe is executed against the assembly.
		''' </summary>
		<ComRegisterFunction> _
		Public Shared Sub OnRegister(ByVal t As Type)
			Dim title As String = "SolidEdge.Community.AddInDemo.MyAddIn"
			Dim summary As String = "Solid Edge Addin in .NET 4.0."
			Dim enabled = True ' You have the option to register the addin in a disabled state.

			' List of environments that your addin supports.
			Dim environments() As Guid = { SolidEdge.CATID.SEApplicationGuid, SolidEdge.CATID.SEAllDocumentEnvrionmentsGuid }

			Try
				MyAddIn.Register(t, title, summary, enabled, environments)
			Catch ex As System.Exception
				MessageBox.Show(ex.StackTrace, ex.Message)
			End Try
		End Sub

		''' <summary>
		''' Called when regasm.exe /u is executed against the assembly.
		''' </summary>
		<ComUnregisterFunction> _
		Public Shared Sub OnUnregister(ByVal t As Type)
			MyAddIn.Unregister(t)
		End Sub
	End Class
End Namespace
