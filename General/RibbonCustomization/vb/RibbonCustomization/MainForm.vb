Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Private _themeName As String = "Community"
    'private string _tabName = "Community";
    Private _tabName As String = "Home"
    Private _groupName As String = "Group 1"
    Private _macro As String = "Notepad.exe"

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        OleMessageFilter.Register()
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        OleMessageFilter.Unregister()
    End Sub

    Private Sub buttonCreateTheme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonCreateTheme.Click
        Dim application = SolidEdgeUtils.Connect(False)
        Dim customization = application.Customization
        Dim ribbonBarThemes = customization.RibbonBarThemes
        Dim ribbonBarTheme As SolidEdgeFramework.RibbonBarTheme = Nothing

        ' Look for our custom theme.
        For Each theme As SolidEdgeFramework.RibbonBarTheme In ribbonBarThemes
            If theme.Name.Equals(_themeName, StringComparison.Ordinal) Then
                ribbonBarTheme = theme
            End If
        Next theme

        customization.BeginCustomization()

        ' If our theme is not found, create it.
        If ribbonBarTheme Is Nothing Then
            ribbonBarTheme = ribbonBarThemes.Create(Nothing)
            ribbonBarTheme.Name = _themeName
        End If

        Dim ribbonBars = ribbonBarTheme.RibbonBars
        For Each ribbonBar As SolidEdgeFramework.RibbonBar In ribbonBars
            ' For this demo, only change the ribbon for the active environment.
            If ribbonBar.Environment.Equals(application.ActiveEnvironment) Then
                Dim ribbonBarTabs = ribbonBar.RibbonBarTabs

                ' Some environments likely dont' have RibbonBarTabs by default! i.e. Application environment.
                If ribbonBarTabs IsNot Nothing Then
                    Dim ribbonBarTab As SolidEdgeFramework.RibbonBarTab = Nothing
                    Dim ribbonBarGroup As SolidEdgeFramework.RibbonBarGroup = Nothing
                    Dim ribbonBarControl As SolidEdgeFramework.RibbonBarControl = Nothing

                    ' Check to see if the tab exists.
                    For Each tab As SolidEdgeFramework.RibbonBarTab In ribbonBarTabs
                        If tab.Name.Equals(_tabName, StringComparison.Ordinal) Then
                            ribbonBarTab = tab
                        End If
                    Next tab

                    ' Create the tab if it does not already exist.
                    If ribbonBarTab Is Nothing Then
'INSTANT VB NOTE: The variable tabIndex was renamed since Visual Basic does not handle local variables named the same as class members well:
                        Dim tabIndex_Renamed = ribbonBarTabs.Count ' Insert at the end.
                        ribbonBarTab = ribbonBarTabs.Insert(_tabName, tabIndex_Renamed, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreate)
                        ribbonBarTab.Visible = True
                    End If

                    Dim ribbonBarGroups = ribbonBarTab.RibbonBarGroups

                    ' Check to see if the group exists.
                    For Each group As SolidEdgeFramework.RibbonBarGroup In ribbonBarGroups
                        If group.Name.Equals(_groupName, StringComparison.Ordinal) Then
                            ribbonBarGroup = group
                        End If
                    Next group

                    ' Create the group if it does not already exist.
                    If ribbonBarGroup Is Nothing Then
                        Dim groupIndex = ribbonBarGroups.Count ' Insert at the end.
                        ribbonBarGroup = ribbonBarGroups.Insert(_groupName, groupIndex, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreate)
                        ribbonBarGroup.Visible = True
                    End If

                    Dim ribbonBarControls = ribbonBarGroup.RibbonBarControls

                    ' Check to see if the control exists.
                    For Each control As SolidEdgeFramework.RibbonBarControl In ribbonBarControls
                        If control.Name.Equals(_macro, StringComparison.Ordinal) Then
                            ribbonBarControl = control
                        End If
                    Next control

                    ' Create the control if it does not already exist.
                    If ribbonBarControl Is Nothing Then
                        Dim itemArray() As Object = { _macro }
                        ribbonBarControl = ribbonBarControls.Insert(itemArray, Nothing, SolidEdgeFramework.RibbonBarInsertMode.seRibbonBarInsertCreateButton)
                        ribbonBarControl.Visible = True
                    End If

                    Exit For
                End If
            End If
        Next ribbonBar

        ribbonBarThemes.ActivateTheme(_themeName)
        ribbonBarThemes.Commit()
        customization.EndCustomization()
    End Sub

    Private Sub buttonDeleteTheme_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonDeleteTheme.Click
        Dim application = SolidEdgeUtils.Connect(False)
        Dim customization = application.Customization
        Dim ribbonBarThemes = customization.RibbonBarThemes
        Dim ribbonBarTheme As SolidEdgeFramework.RibbonBarTheme = Nothing

        ' Look for our custom theme.
        For Each theme As SolidEdgeFramework.RibbonBarTheme In ribbonBarThemes
            If theme.Name.Equals(_themeName, StringComparison.Ordinal) Then
                ribbonBarTheme = theme
            End If
        Next theme

        ' If found, delete it.
        If ribbonBarTheme IsNot Nothing Then
            customization.BeginCustomization()
            ribbonBarThemes.Remove(ribbonBarTheme)
            ribbonBarThemes.Commit()
            customization.EndCustomization()
        End If
    End Sub
End Class
