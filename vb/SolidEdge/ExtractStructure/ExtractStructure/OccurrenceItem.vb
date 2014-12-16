Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace ExtractStructure
	Public Class OccurrenceItem
		Private _children As New List(Of OccurrenceItem)()

		Public Sub New()
		End Sub

		Public Sub New(ByVal occurrence As SolidEdgeAssembly.Occurrence)
			FileName = occurrence.OccurrenceFileName
		End Sub

		Public Sub New(ByVal subOccurrence As SolidEdgeAssembly.SubOccurrence)
			FileName = subOccurrence.SubOccurrenceFileName
		End Sub

		Public FileName As String
		Public ReadOnly Property Occurrence() As List(Of OccurrenceItem)
			Get
				Return _children
			End Get
		End Property

		Public Overrides Function ToString() As String
			Return FileName
		End Function
	End Class
End Namespace
