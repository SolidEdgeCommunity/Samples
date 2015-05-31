Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Structure SearchResultItem
    Public Sub New(ByVal file As String, ByVal version As Version)
        FileName = file
        Me.Version = version
    End Sub

    Public FileName As String
    Public Version As Version
End Structure
