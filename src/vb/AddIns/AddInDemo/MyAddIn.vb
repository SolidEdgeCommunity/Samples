Imports SolidEdgeContrib.AddIn
Imports SolidEdgeContrib.Extensions
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Namespace AddInDemo
	<ComVisible(True), Guid("00000000-0000-0000-0000-50FC26B3C501"), ProgId("SolidEdge.Samples.AddInDemo")> _
	Public Class MyAddIn
		Inherits SolidEdgeContrib.AddIn.SolidEdgeAddIn

		Public Overrides Sub OnConnection(ByVal application As SolidEdgeFramework.Application, ByVal ConnectMode As SolidEdgeFramework.SeConnectMode, ByVal AddInInstance As SolidEdgeFramework.AddIn)
			MyBase.OnConnection(application, ConnectMode, AddInInstance)

			' Put your custom OnConnection code here.
			Dim applicationEvents = application.GetApplicationEvents()
			AddHandler applicationEvents.AfterWindowActivate, AddressOf applicationEvents_AfterWindowActivate
		End Sub

		Public Overrides Sub OnConnectToEnvironment(ByVal environment As SolidEdgeFramework.Environment, ByVal firstTime As Boolean)
			MyBase.OnConnectToEnvironment(environment, firstTime)

			' Put your custom OnConnectToEnvironment code here.
		End Sub

		Public Overrides Sub OnDisconnection(ByVal DisconnectMode As SolidEdgeFramework.SeDisconnectMode)
			MyBase.OnDisconnection(DisconnectMode)

			' Put your custom OnDisconnection code here.
		End Sub

		Public Overrides Sub OnInitializeRibbon(ByVal ribbon As Ribbon, ByVal firstTime As Boolean)
			' Let the base class handle initializing the ribbon via addin.manifest.
			MyBase.OnInitializeRibbon(ribbon, firstTime)

			' Now we can customize the ribbon.
			Dim boxButton = ribbon.LookupControl("Box")

			If boxButton IsNot Nothing Then
				boxButton.Enabled = False
			End If
		End Sub

		Public Overrides Sub OnRibbonControl(ByVal ribbonControl As RibbonControl)
			' Base class doesn't do anything special when a ribbon control is invoked unless a macro is assigned.
			'base.OnRibbonControl(ribbonControl);

			If ribbonControl.Name.Equals("Save") Then
				Using dialog As New SaveFileDialog()
					' The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
					If Me.Application.ShowDialog(dialog) = DialogResult.OK Then
					End If
				End Using
			ElseIf ribbonControl.Name.Equals("Folder") Then
				Using dialog As New FolderBrowserDialog()
					' The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
					If Me.Application.ShowDialog(dialog) = DialogResult.OK Then
					End If
				End Using
			ElseIf ribbonControl.Name.Equals("Monitor") Then
				Using dialog As New CustomDialog()
					' The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
					If Me.Application.ShowDialog(dialog) = DialogResult.OK Then
					End If
				End Using
			ElseIf ribbonControl.Name.Equals("Tools") Then
				Me.Application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartToolsOptions)
			ElseIf ribbonControl.Name.Equals("Help") Then
				Me.Application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartHelpSolidEdgeontheWeb)
			ElseIf ribbonControl.Name.Equals("BoundingBox") Then
				' Toggle the check state.
				ribbonControl.Checked = Not ribbonControl.Checked

				Dim window = TryCast(Me.Application.ActiveWindow, SolidEdgeFramework.Window)

				If window IsNot Nothing Then
					' Find the relevant overlay(s).
					Dim overlays = Me.ViewOverlays.OfType(Of MyViewOverlay)().Where(Function(x) x.Window.Equals(window))

					For Each overlay In overlays
						overlay.ShowBoundingBox = ribbonControl.Checked

						' For the view to update.
						window.View.Update()
					Next overlay
				End If
			ElseIf ribbonControl.Name.Equals("OpenGLBoxes") Then
				' Toggle the check state.
				ribbonControl.Checked = Not ribbonControl.Checked

				Dim window = TryCast(Me.Application.ActiveWindow, SolidEdgeFramework.Window)

				If window IsNot Nothing Then
					' Find the relevant overlay(s).
					Dim overlays = Me.ViewOverlays.OfType(Of MyViewOverlay)().Where(Function(x) x.Window.Equals(window))

					For Each overlay In overlays
						overlay.ShowOpenGlBoxes = ribbonControl.Checked

						' For the view to update.
						window.View.Update()
					Next overlay
				End If
			ElseIf ribbonControl.Name.Equals("GDIPlus") Then
				' Toggle the check state.
				ribbonControl.Checked = Not ribbonControl.Checked

				Dim window = TryCast(Me.Application.ActiveWindow, SolidEdgeFramework.Window)

				If window IsNot Nothing Then
					' Find the relevant overlay(s).
					Dim overlays = Me.ViewOverlays.OfType(Of MyViewOverlay)().Where(Function(x) x.Window.Equals(window))

					For Each overlay In overlays
						overlay.ShowGDIPlus = ribbonControl.Checked

						' For the view to update.
						window.View.Update()
					Next overlay
				End If
			Else
				' Demonstrate toggling the check state.
				ribbonControl.Checked = Not ribbonControl.Checked
			End If
		End Sub

		Public Overrides ReadOnly Property NativeResourcesDllPath() As String
			Get
				' You can override the path to your native images if you need.
				Return MyBase.NativeResourcesDllPath
			End Get
		End Property

		Private Sub applicationEvents_AfterWindowActivate(ByVal theWindow As Object)
			Dim window = TryCast(theWindow, SolidEdgeFramework.Window)

			If window IsNot Nothing Then
				' Add the overlay.
				Me.ViewOverlays.Add(Of MyViewOverlay)(window)
			End If
		End Sub

		<ComRegisterFunction> _
		Public Shared Sub OnRegister(ByVal t As Type)
			Try
				' SolidEdgeAddIn.Register() will throw an exception if it cannot locate an embedded resource named [DEFAULT_NAMESPACE].addin.manifest.
				' If you want to take control of the registration process, simply don't call SolidEdgeAddIn.Register().
				SolidEdgeContrib.AddIn.SolidEdgeAddIn.Register(t)
			Catch ex As System.Exception
				MessageBox.Show(ex.StackTrace, ex.Message)
			End Try

			' Perform any additional registration procedures if needed.
		End Sub

		<ComUnregisterFunction> _
		Public Shared Sub OnUnregister(ByVal t As Type)
			SolidEdgeContrib.AddIn.SolidEdgeAddIn.Unregister(t)
		End Sub
	End Class
End Namespace
