Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class DocumentItem
   Public Sub New()
        DocumentItems = New List(Of DocumentItem)()
   End Sub

    Public Sub New(ByVal occurrence As SolidEdgeAssembly.Occurrence)
        Me.New()
        FileName = occurrence.OccurrenceFileName
    End Sub

    Public Sub New(ByVal subOccurrence As SolidEdgeAssembly.SubOccurrence)
        Me.New()
        FileName = subOccurrence.SubOccurrenceFileName
    End Sub

    Public Property FileName() As String

    ''' <summary>
    ''' Returns all direct occurrences.
    ''' </summary>
    Public Property DocumentItems() As List(Of DocumentItem)

    ''' <summary>
    ''' Returns all direct and descendant children.
    ''' </summary>
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
    <JsonIgnore> _
    Public ReadOnly Iterator Property AllDocumentItems() As IEnumerable(Of DocumentItem)
        Get
            For Each bomItem In DocumentItems
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
                Yield bomItem

                For Each childBomItem In bomItem.AllDocumentItems
'INSTANT VB TODO TASK: VB does not support iterators prior to VB11 (VS 2012):
                    Yield childBomItem
                Next childBomItem
            Next bomItem
        End Get
    End Property

    Public Overrides Function GetHashCode() As Integer
        Return FileName.GetHashCode()
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If

        If TypeOf obj Is DocumentItem Then
            Return Equals(DirectCast(obj, DocumentItem))
        End If

        Return MyBase.Equals(obj)
    End Function

    Public Overloads Function Equals(ByVal obj As DocumentItem) As Boolean
        If obj Is Nothing Then
            Return False
        End If

        Return obj.FileName.Equals(FileName)
    End Function

    Public Overrides Function ToString() As String
        Return FileName
    End Function
End Class
