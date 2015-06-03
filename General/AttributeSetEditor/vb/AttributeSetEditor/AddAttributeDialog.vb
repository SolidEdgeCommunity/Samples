Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Partial Public Class AddAttributeDialog
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub textBoxSetName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles textBoxSetName.TextChanged
        UpdateControls()
    End Sub

    Private Sub textBoxAttributeName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles textBoxAttributeName.TextChanged
        UpdateControls()
    End Sub

    Private Sub textBoxAttributeValue_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles textBoxAttributeValue.TextChanged
        UpdateControls()
    End Sub

    Private Sub UpdateControls()
        If (textBoxSetName.Text.Length = 0) AndAlso (textBoxAttributeName.Text.Length = 0) AndAlso (textBoxAttributeValue.Text.Length = 0) Then
            buttonOK.Enabled = False
        Else
            buttonOK.Enabled = True
        End If
    End Sub

    Public ReadOnly Property SetName() As String
        Get
            Return textBoxSetName.Text
        End Get
    End Property

    Public ReadOnly Property AttributeName() As String
        Get
            Return textBoxAttributeName.Text
        End Get
    End Property

    Public ReadOnly Property AttributeValue() As String
        Get
            Return textBoxAttributeValue.Text
        End Get
    End Property

    Private Sub buttonOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonOK.Click
        DialogResult = System.Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonCancel.Click
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Close()
    End Sub
End Class
