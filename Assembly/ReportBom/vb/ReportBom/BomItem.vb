Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

''' <summary>
''' Class to hold BOM data.
''' </summary>
Public Class BomItem
    Private _children As New List(Of BomItem)()

    Public Sub New()
    End Sub

    Public Sub New(ByVal occurrence As SolidEdgeAssembly.Occurrence, ByVal level As Integer)
        Me.Level = level
        FileName = System.IO.Path.GetFullPath(occurrence.OccurrenceFileName)
        IsMissing = occurrence.FileMissing()
        Quantity = 1
        IsSubassembly = occurrence.Subassembly

        ' If the file exists, extract file properties.
        If IsMissing = False Then
            Dim document = DirectCast(occurrence.OccurrenceDocument, SolidEdgeFramework.SolidEdgeDocument)
            Dim summaryInfo = DirectCast(document.SummaryInfo, SolidEdgeFramework.SummaryInfo)

            DocumentNumber = summaryInfo.DocumentNumber
            Title = summaryInfo.Title
            Revision = summaryInfo.RevisionNumber
        End If
    End Sub

    Public Property Level() As Integer?
    Public Property DocumentNumber() As String
    Public Property Revision() As String
    Public Property Title() As String
    Public Property Quantity() As Integer?
    Public Property FileName() As String

    <JsonIgnore> _
    Public Property IsSubassembly() As Boolean?

    <JsonIgnore> _
    Public IsMissing? As Boolean

    ''' <summary>
    ''' Returns all direct children.
    ''' </summary>
    <JsonProperty("Child")> _
    Public Property Children() As List(Of BomItem)
        Get
            Return _children
        End Get
        Set(ByVal value As List(Of BomItem))
            _children = value
        End Set
    End Property

    ''' <summary>
    ''' Returns all direct and descendant children.
    ''' </summary>
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
    <JsonIgnore> _
    Public ReadOnly Iterator Property AllChildren() As IEnumerable(Of BomItem)
        Get
            For Each bomItem In Children
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
                Yield bomItem

                If bomItem.IsSubassembly = True Then
                    For Each childBomItem In bomItem.AllChildren
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
                        Yield childBomItem
                    Next childBomItem
                End If
            Next bomItem
        End Get
    End Property

    ' Demonstration of how to exclude empty collections during JSON.NET serialization.
    Public Function ShouldSerializeChildren() As Boolean
        Return Children.Count > 0
    End Function

    Public Overrides Function ToString() As String
        Return FileName
    End Function
End Class
