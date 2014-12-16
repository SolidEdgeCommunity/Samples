Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace ExtractBOM
    Public Class BomItem
        Private _children As New List(Of BomItem)()

        Public Sub New()
        End Sub

        Public Sub New(ByVal occurrence As SolidEdgeAssembly.Occurrence)
            FileName = occurrence.OccurrenceFileName
        End Sub

        Public FileName As String
        Public Missing? As Boolean
        Public Property Occurrence() As List(Of BomItem)
            Get
                Return _children
            End Get
            Set(ByVal value As List(Of BomItem))
                _children = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return FileName
        End Function
    End Class
End Namespace
