Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text

<Serializable> _
Public Class OpenSaveSettings
    Private _applicationSettings As New ApplicationSettings()
    Private _assemblySettings As New AssemblySettings()
    Private _draftSettings As New DraftSettings()
    Private _partSettings As New PartSettings()
    Private _sheetMetalSettings As New SheetMetalSettings()
    Private _weldmentSettings As New WeldmentSettings()

    Public Sub New()
        Application = New ApplicationSettings()
        Assembly = New AssemblySettings()
        Draft = New DraftSettings()
        Part = New PartSettings()
        SheetMetal = New SheetMetalSettings()
        Weldment = New WeldmentSettings()
    End Sub

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property Application() As ApplicationSettings

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property Assembly() As AssemblySettings

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property Draft() As DraftSettings

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property Part() As PartSettings

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property SheetMetal() As SheetMetalSettings

    <TypeConverter(GetType(ExpandableObjectConverter))> _
    Public Property Weldment() As WeldmentSettings
End Class

<Serializable> _
Public Class ApplicationSettings
    Public Sub New()
        Visible = True
    End Sub

    Public Property DisableAddins() As Boolean
    Public Property DisplayAlerts() As Boolean
    Public Property Visible() As Boolean

    Public Overrides Function ToString() As String
        Return "Application Settings"
    End Function
End Class

<Serializable> _
Public Class AssemblySettings
    Public Overrides Function ToString() As String
        Return "Assembly Settings"
    End Function
End Class

<Serializable> _
Public Class DraftSettings
    Public Property UpdateDrawingViews() As Boolean

    Public Overrides Function ToString() As String
        Return "Draft Settings"
    End Function
End Class

<Serializable> _
Public Class PartSettings
    Public Overrides Function ToString() As String
        Return "Part Settings"
    End Function
End Class

<Serializable> _
Public Class SheetMetalSettings
    Public Overrides Function ToString() As String
        Return "SheetMetal Settings"
    End Function
End Class

<Serializable> _
Public Class WeldmentSettings
    Public Overrides Function ToString() As String
        Return "Weldment Settings"
    End Function
End Class
