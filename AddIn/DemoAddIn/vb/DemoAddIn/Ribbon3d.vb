Imports SolidEdgeCommunity.AddIn
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Friend Class Ribbon3d
    Inherits SolidEdgeCommunity.AddIn.Ribbon

    Private Const _embeddedResourceName As String = "DemoAddIn.Ribbon3d.xml"
    Private _buttonBoundingBox As RibbonButton
    Private _buttonOpenGlBoxes As RibbonButton
    Private _buttonGdiPlus As RibbonButton

    Public Sub New()
        ' Get a reference to the current assembly. This is where the ribbon XML is embedded.
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly()

        ' In this example, XML file must have a build action of "Embedded Resource".
        Me.LoadXml(assembly, _embeddedResourceName)

        ' Example of how to bind a local variable to a particular ribbon control.
        _buttonBoundingBox = GetButton(20)
        _buttonOpenGlBoxes = GetButton(21)
        _buttonGdiPlus = GetButton(22)

        ' Example of how to bind a particular ribbon control click event.
        AddHandler _buttonBoundingBox.Click, AddressOf _buttonBoundingBox_Click
        AddHandler _buttonOpenGlBoxes.Click, AddressOf _buttonOpenGlBoxes_Click
        AddHandler _buttonGdiPlus.Click, AddressOf _buttonGdiPlus_Click

        ' Get the Solid Edge version.
        Dim version = DemoAddIn.Instance.SolidEdgeVersion

        ' View.GetModelRange() is only available in ST6 or greater.
        If version.Major < 106 Then
            _buttonBoundingBox.Enabled = False
        End If
    End Sub

    Public Overrides Sub OnControlClick(ByVal control As RibbonControl)
        Dim application = DemoAddIn.Instance.Application

        ' Demonstrate how to handle commands without binding to a local variable.
        Select Case control.CommandId
            Case 0
                Using dialog = New SaveFileDialog()
                    ' The ShowDialog() extension method is exposed by:
                    ' using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                    If application.ShowDialog(dialog) = DialogResult.OK Then

                    End If
                End Using
            Case 1
                Using dialog = New FolderBrowserDialog()
                    ' The ShowDialog() extension method is exposed by:
                    ' using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                    If application.ShowDialog(dialog) = DialogResult.OK Then
                    End If
                End Using
            Case 2
                Using dialog = New MyCustomDialog()
                    ' The ShowDialog() extension method is exposed by:
                    ' using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                    If application.ShowDialog(dialog) = DialogResult.OK Then
                    End If
                End Using
            Case 8
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartToolsOptions)
            Case 11
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartHelpSolidEdgeontheWeb)
        End Select
    End Sub

    Private Sub _buttonGdiPlus_Click(ByVal control As RibbonControl)
        Dim overlay = GetActiveOverlay()

        ' Toggle the button check state.
        _buttonGdiPlus.Checked = Not _buttonGdiPlus.Checked
        overlay.ShowGDIPlus = _buttonGdiPlus.Checked
    End Sub

    Private Sub _buttonOpenGlBoxes_Click(ByVal control As RibbonControl)
        Dim overlay = GetActiveOverlay()

        ' Toggle the button check state.
        _buttonOpenGlBoxes.Checked = Not _buttonOpenGlBoxes.Checked
        overlay.ShowOpenGlBoxes = _buttonOpenGlBoxes.Checked
    End Sub

    Private Sub _buttonBoundingBox_Click(ByVal control As RibbonControl)
        Dim overlay = GetActiveOverlay()

        ' Toggle the button check state.
        _buttonBoundingBox.Checked = Not _buttonBoundingBox.Checked
        overlay.ShowBoundingBox = _buttonBoundingBox.Checked
    End Sub

    Private Function GetActiveOverlay() As MyViewOverlay
        Dim controlller = DemoAddIn.Instance.ViewOverlayController
        Dim window = DirectCast(DemoAddIn.Instance.Application.ActiveWindow, SolidEdgeFramework.Window)
        Dim overlay = CType(controlller.GetOverlay(window), MyViewOverlay)

        If overlay Is Nothing Then
            ' If the overlay has not been created yet, add a new one.
            overlay = controlller.Add(Of MyViewOverlay)(window)
        End If

        Return overlay
    End Function
End Class
