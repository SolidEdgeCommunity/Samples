' See https://github.com/SolidEdgeCommunity/SolidEdge.Community for documentation.
' Useful Package Manager Console Commands: https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Package-Manager-Console-Powershell-Reference
' Register-SolidEdgeAddIn
' Unregister-SolidEdgeAddIn
' Set-DebugSolidEdge
' Install-SolidEdgeAddInRibbonSchema

Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text
Imports System.Windows.Forms

<ComVisible(True), Guid("BF1C1BB8-75EE-444A-8DCE-0F1521D0764B"), ProgId("SolidEdgeCommunity.Samples.DemoAddIn")> _
Public Class DemoAddIn ' Solid Edge Shortcut Menu Events -  Solid Edge EC Events -  Solid Egde New File UI Events -  Solid Egde File UI Events -  Solid Edge Feature Library Events -  Solid Edge Window Events -  Solid Edge Application Events -  Solid Edge Community provided base class. -  Must be unique! -  Must be unique!
    Inherits SolidEdgeCommunity.AddIn.SolidEdgeAddIn
    Implements SolidEdgeFramework.ISEApplicationEvents, SolidEdgeFramework.ISEApplicationWindowEvents, SolidEdgeFramework.ISEFeatureLibraryEvents, SolidEdgeFramework.ISEFileUIEvents, SolidEdgeFramework.ISENewFileUIEvents, SolidEdgeFramework.ISEECEvents, SolidEdgeFramework.ISEShortCutMenuEvents

    Private _connectionPointController As SolidEdgeCommunity.ConnectionPointController

    #Region "SolidEdgeCommunity.AddIn.SolidEdgeAddIn overrides"

    ''' <summary>
    ''' Called when the addin is first loaded by Solid Edge.
    ''' </summary>
    Public Overrides Sub OnConnection(ByVal application As SolidEdgeFramework.Application, ByVal ConnectMode As SolidEdgeFramework.SeConnectMode, ByVal AddInInstance As SolidEdgeFramework.AddIn)
        ' If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
        ' will not initialize properly.
        AddInEx.GuiVersion = 1

        ' Create an instance of the default connection point controller. It helps manage connections to COM events.
        _connectionPointController = New SolidEdgeCommunity.ConnectionPointController(Me)

        ' Uncomment the following line to attach to the Solid Edge Application Events.
        _connectionPointController.AdviseSink(Of SolidEdgeFramework.ISEApplicationEvents)(Me.Application)

        ' Not necessary unless you absolutely need to see low level windows messages.
        ' Uncomment the following line to attach to the Solid Edge Application Window Events.
        '_connectionPointController.AdviseSink<SolidEdgeFramework.ISEApplicationWindowEvents>(this.Application);

        ' Uncomment the following line to attach to the Solid Edge Feature Library Events.
        _connectionPointController.AdviseSink(Of SolidEdgeFramework.ISEFeatureLibraryEvents)(Me.Application)

        ' Uncomment the following line to attach to the Solid Edge File UI Events.
        '_connectionPointController.AdviseSink<SolidEdgeFramework.ISEFileUIEvents>(this.Application);

        ' Uncomment the following line to attach to the Solid Edge File New UI Events.
        '_connectionPointController.AdviseSink<SolidEdgeFramework.ISENewFileUIEvents>(this.Application);

        ' Uncomment the following line to attach to the Solid Edge EC Events.
        '_connectionPointController.AdviseSink<SolidEdgeFramework.ISEECEvents>(this.Application);

        ' Uncomment the following line to attach to the Solid Edge Shortcut Menu Events.
        '_connectionPointController.AdviseSink<SolidEdgeFramework.ISEShortCutMenuEvents>(this.Application);
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
        ' Disconnect from all COM events.
        _connectionPointController.UnadviseAllSinks()
    End Sub

    ''' <summary>
    ''' Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
    ''' </summary>
    Public Overrides Sub OnCreateEdgeBarPage(ByVal controller As SolidEdgeCommunity.AddIn.EdgeBarController, ByVal document As SolidEdgeFramework.SolidEdgeDocument)
        ' Note: Confirmed with Solid Edge development, OnCreateEdgeBarPage does not get called when Solid Edge is first open and the first document is open.
        ' i.e. Under the hood, SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() is not getting called.
        ' As an alternative, you can call DemoAddIn.Instance.EdgeBarController.Add() in some other event if you need.

        ' Get the document type of the passed in document.
        Dim documentType = document.Type
        Dim imageId = 1

        ' Depending on the document type, you may have different edgebar controls.
        Select Case documentType
            Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument, SolidEdgeFramework.DocumentTypeConstants.igDraftDocument, SolidEdgeFramework.DocumentTypeConstants.igPartDocument, SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument
                controller.Add(Of MyEdgeBarControl)(document, imageId)
        End Select
    End Sub

    ''' <summary>
    ''' Called directly after OnConnectToEnvironment() to give you an opportunity to configure a ribbon for a specific environment.
    ''' </summary>
    Public Overrides Sub OnCreateRibbon(ByVal controller As SolidEdgeCommunity.AddIn.RibbonController, ByVal environmentCategory As Guid, ByVal firstTime As Boolean)
        ' Depending on environment, you may or may not want to load different ribbons.
        If environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.Assembly) Then
            ' Assembly Environment
            controller.Add(Of Ribbon3d)(environmentCategory, firstTime)
        ElseIf environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.Draft) Then
            ' Draft Environment
            controller.Add(Of Ribbon2d)(environmentCategory, firstTime)
        ElseIf environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.Part) Then
            ' Traditional Part Environment
            controller.Add(Of Ribbon3d)(environmentCategory, firstTime)
        ElseIf environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.DMPart) Then
            ' Synchronous Part Environment
            controller.Add(Of Ribbon3d)(environmentCategory, firstTime)
        ElseIf environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.SheetMetal) Then
            ' Traditional SheetMetal Environment
            controller.Add(Of Ribbon3d)(environmentCategory, firstTime)
        ElseIf environmentCategory.Equals(SolidEdgeSDK.EnvironmentCategories.DMSheetMetal) Then
            ' Synchronous SheetMetal Environment
            controller.Add(Of Ribbon3d)(environmentCategory, firstTime)
        End If
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEApplicationEvents"

    ''' <summary>
    ''' Occurs after the active document changes.
    ''' </summary>
    Public Sub AfterActiveDocumentChange(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterActiveDocumentChange
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs after a specified command is run.
    ''' </summary>
    Public Sub AfterCommandRun(ByVal theCommandID As Integer) Implements SolidEdgeFramework.ISEApplicationEvents.AfterCommandRun
    End Sub

    ''' <summary>
    ''' Occurs after a specified document is opened.
    ''' </summary>
    Public Sub AfterDocumentOpen(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterDocumentOpen
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs after a specified document is printed.
    ''' </summary>
    Public Sub AfterDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer) Implements SolidEdgeFramework.ISEApplicationEvents.AfterDocumentPrint
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs when a specified document is saved.
    ''' </summary>
    Public Sub AfterDocumentSave(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterDocumentSave
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs when a specified environment is activated.
    ''' </summary>
    Public Sub AfterEnvironmentActivate(ByVal theEnvironment As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterEnvironmentActivate
        Dim environment = TryCast(theEnvironment, SolidEdgeFramework.Environment)
    End Sub

    ''' <summary>
    ''' Occurs after a new document is opened.
    ''' </summary>
    Public Sub AfterNewDocumentOpen(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterNewDocumentOpen
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs after a new window is created.
    ''' </summary>
    Public Sub AfterNewWindow(ByVal theWindow As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterNewWindow
        ' Could be either a SolidEdgeFramework.Window (3D) or SolidEdgeDraft.SheetWindow (2D).
        Dim window = TryCast(theWindow, SolidEdgeFramework.Window)
        Dim sheetWindow = TryCast(theWindow, SolidEdgeDraft.SheetWindow)
    End Sub

    ''' <summary>
    ''' Occurs after a specified window is activated.
    ''' </summary>
    Public Sub AfterWindowActivate(ByVal theWindow As Object) Implements SolidEdgeFramework.ISEApplicationEvents.AfterWindowActivate
        ' Could be either a SolidEdgeFramework.Window (3D) or SolidEdgeDraft.SheetWindow (2D).
        Dim window = TryCast(theWindow, SolidEdgeFramework.Window)
        Dim sheetWindow = TryCast(theWindow, SolidEdgeDraft.SheetWindow)
    End Sub

    ''' <summary>
    ''' Occurs before a specified command is run.
    ''' </summary>
    Public Sub BeforeCommandRun(ByVal theCommandID As Integer) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeCommandRun
    End Sub

    ''' <summary>
    ''' Occurs before a specified document is closed.
    ''' </summary>
    Public Sub BeforeDocumentClose(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeDocumentClose
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs before a specified document is printed.
    ''' </summary>
    Public Sub BeforeDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeDocumentPrint
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs before a specified document is saved.
    ''' </summary>
    Public Sub BeforeDocumentSave(ByVal theDocument As Object) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeDocumentSave
        Dim document = TryCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
    End Sub

    ''' <summary>
    ''' Occurs before a specified environment is deactivated.
    ''' </summary>
    Public Sub BeforeEnvironmentDeactivate(ByVal theEnvironment As Object) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeEnvironmentDeactivate
        Dim environment = TryCast(theEnvironment, SolidEdgeFramework.Environment)
    End Sub

    ''' <summary>
    ''' Occurs before the associated application is closed.
    ''' </summary>
    Public Sub BeforeQuit() Implements SolidEdgeFramework.ISEApplicationEvents.BeforeQuit
    End Sub

    ''' <summary>
    ''' Occurs before a specified window is deactivated.
    ''' </summary>
    Public Sub BeforeWindowDeactivate(ByVal theWindow As Object) Implements SolidEdgeFramework.ISEApplicationEvents.BeforeWindowDeactivate
        ' Could be either a SolidEdgeFramework.Window (3D) or SolidEdgeDraft.SheetWindow (2D).
        Dim window = TryCast(theWindow, SolidEdgeFramework.Window)
        Dim sheetWindow = TryCast(theWindow, SolidEdgeDraft.SheetWindow)
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEApplicationWindowEvents"

    ''' <summary>
    ''' Occurs when the application receives a window event.
    ''' </summary>
    Public Sub WindowProc(ByVal hWnd As Integer, ByVal nMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) Implements SolidEdgeFramework.ISEApplicationWindowEvents.WindowProc
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEFeatureLibraryEvents"

    ''' <summary>
    ''' Occurs when a new feature library file is created.
    ''' </summary>
    Public Sub AfterFeatureLibraryDocumentCreated(ByVal Name As String) Implements SolidEdgeFramework.ISEFeatureLibraryEvents.AfterFeatureLibraryDocumentCreated
    End Sub

    ''' <summary>
    ''' Occurs when a new feature library file is deleted.
    ''' </summary>
    Public Sub AfterFeatureLibraryDocumentDeleted(ByVal Name As String) Implements SolidEdgeFramework.ISEFeatureLibraryEvents.AfterFeatureLibraryDocumentDeleted
    End Sub

    ''' <summary>
    ''' Occurs when a user executes the Rename command on a feature library file.
    ''' </summary>
    Public Sub AfterFeatureLibraryDocumentRenamed(ByVal NewName As String, ByVal OldName As String) Implements SolidEdgeFramework.ISEFeatureLibraryEvents.AfterFeatureLibraryDocumentRenamed
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEFileUIEvents"

    ''' <summary>
    ''' Occurs before the user interface is displayed for a part created in place by Solid Edge Assembly.
    ''' </summary>
    Public Sub OnCreateInPlacePartUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String, <System.Runtime.InteropServices.Out()> ByRef Template As String) Implements SolidEdgeFramework.ISEFileUIEvents.OnCreateInPlacePartUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            Filename = Nothing
            AppendToTitle = Nothing
            Template = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    ''' <summary>
    ''' Occurs before the user interface is created for a new Solid Edge file.
    ''' </summary>
    Public Sub OnFileNewUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String) Implements SolidEdgeFramework.ISEFileUIEvents.OnFileNewUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    ''' <summary>
    ''' Occurs before the creation of the user interface for the file opened by Solid Edge.
    ''' </summary>
    Public Sub OnFileOpenUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String) Implements SolidEdgeFramework.ISEFileUIEvents.OnFileOpenUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    ''' <summary>
    ''' Occurs before the user interface is created for the file saved as image by Solid Edge.
    ''' </summary>
    Public Sub OnFileSaveAsImageUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String, ByRef Width As Integer, ByRef Height As Integer, ByRef ImageQuality As SolidEdgeFramework.SeImageQualityType) Implements SolidEdgeFramework.ISEFileUIEvents.OnFileSaveAsImageUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
            Width = 100
            Height = 100
            ImageQuality = SolidEdgeFramework.SeImageQualityType.seImageQualityHigh
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    ''' <summary>
    ''' Occurs before the user interface is created by the file saved by Solid Edge as another file.
    ''' </summary>
    Public Sub OnFileSaveAsUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String) Implements SolidEdgeFramework.ISEFileUIEvents.OnFileSaveAsUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    ''' <summary>
    ''' Occurs before the user interface is created for a part placed by Solid Edge Assembly.
    ''' </summary>
    Public Sub OnPlacePartUI(<System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String) Implements SolidEdgeFramework.ISEFileUIEvents.OnPlacePartUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISENewFileUIEvents"

    Public Sub OnNewFileUI(ByVal DocumentType As SolidEdgeFramework.DocumentTypeConstants, <System.Runtime.InteropServices.Out()> ByRef Filename As String, <System.Runtime.InteropServices.Out()> ByRef AppendToTitle As String) Implements SolidEdgeFramework.ISENewFileUIEvents.OnNewFileUI
        Dim overrideDefaultBehavior As Boolean = False

        If overrideDefaultBehavior Then
            ' If you get here, you override the default Solid Edge UI.
            Filename = Nothing
            AppendToTitle = Nothing
        Else
            ' Visual Studio will stop on this exception in debug mode. Simply F5 through it when testing.
            Throw New NotImplementedException()
        End If
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEECEvents"

    ''' <summary>
    ''' Fires File Open event for PDM workflow.
    ''' </summary>
    Public Sub PDM_OnFileOpenUI(<System.Runtime.InteropServices.Out()> ByRef bstrFilename As String) Implements SolidEdgeFramework.ISEECEvents.PDM_OnFileOpenUI
        Throw New NotImplementedException()
    End Sub

    ''' <summary>
    ''' Fires event before CPD Display.
    ''' </summary>
    Public Sub SEEC_BeforeCPDDisplay(ByVal pCPDInitializer As Object, ByVal eCPDMode As SolidEdgeFramework.eCPDMode) Implements SolidEdgeFramework.ISEECEvents.SEEC_BeforeCPDDisplay
    End Sub

    ''' <summary>
    ''' Fires PreCPD event.
    ''' </summary>
    Public Sub SEEC_IsPreCPDEventSupported(<System.Runtime.InteropServices.Out()> ByRef pvbPreCPDEventSupported As Boolean) Implements SolidEdgeFramework.ISEECEvents.SEEC_IsPreCPDEventSupported
        Throw New NotImplementedException()
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEShortCutMenuEvents"

    ''' <summary>
    ''' Notification of short-cut menu creation.
    ''' </summary>
    Public Sub BuildMenu(ByVal EnvCatID As String, ByVal Context As SolidEdgeFramework.ShortCutMenuContextConstants, ByVal pGraphicDispatch As Object, <System.Runtime.InteropServices.Out()> ByRef MenuStrings As Array, <System.Runtime.InteropServices.Out()> ByRef CommandIDs As Array) Implements SolidEdgeFramework.ISEShortCutMenuEvents.BuildMenu
        MenuStrings = Array.CreateInstance(GetType(String), 0)
        CommandIDs = Array.CreateInstance(GetType(Integer), 0)

        Select Case Context
            Case SolidEdgeFramework.ShortCutMenuContextConstants.seShortCutForFeaturePathFinder
            Case SolidEdgeFramework.ShortCutMenuContextConstants.seShortCutForFeaturePathFinderDocument
            Case SolidEdgeFramework.ShortCutMenuContextConstants.seShortCutForGraphicLocate
            Case SolidEdgeFramework.ShortCutMenuContextConstants.seShortCutForView
        End Select
    End Sub

    #End Region

    #Region "RegAsm.exe callbacks"

    ''' <summary>
    ''' Called when regasm.exe is executed against the assembly.
    ''' </summary>
    <ComRegisterFunction> _
    Private Shared Sub OnRegister(ByVal t As Type)
        ' See http://www.codeproject.com/Articles/839585/Solid-Edge-ST-AddIn-Architecture-Overview#Registration for registration details.
        ' The following code helps write registry entries that Solid Edge needs to identify an addin. You can omit this code and
        ' user installer logic if you'd like. This is simply here to help.

        Try
            Dim settings = New SolidEdgeCommunity.AddIn.RegistrationSettings(t)

            settings.Enabled = True
            settings.Environments.Add(SolidEdgeSDK.EnvironmentCategories.Application)
            settings.Environments.Add(SolidEdgeSDK.EnvironmentCategories.AllDocumentEnvrionments)

            ' See http://msdn.microsoft.com/en-us/goglobal/bb964664.aspx for LCID details.
            Dim englishCulture = CultureInfo.GetCultureInfo(1033)

            ' Title & Summary are Locale specific. 
            settings.Titles.Add(englishCulture, "SolidEdgeCommunity.Samples.DemoAddIn")
            settings.Summaries.Add(englishCulture, "Solid Edge Addin in .NET 4.0.")

            ' Optionally, you can add additional locales.
            'var spanishCultere = CultureInfo.GetCultureInfo(3082);
            'settings.Titles.Add(spanishCultere, "SolidEdge.Samples.DemoAddIn");
            'settings.Summaries.Add(spanishCultere, "Solid Edge Addin in .NET 4.0.");

            'var germanCultere = CultureInfo.GetCultureInfo(1031);
            'settings.Titles.Add(germanCultere, "SolidEdge.Samples.DemoAddIn");
            'settings.Summaries.Add(germanCultere, "Solid Edge Addin in .NET 4.0.");

            DemoAddIn.Register(settings)
        Catch ex As System.Exception
            MessageBox.Show(ex.StackTrace, ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Called when regasm.exe /u is executed against the assembly.
    ''' </summary>
    <ComUnregisterFunction> _
    Private Shared Sub OnUnregister(ByVal t As Type)
        DemoAddIn.Unregister(t)
    End Sub

    #End Region
End Class
