Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing

        Try
            OleMessageFilter.Register()
            application = SolidEdgeUtils.Connect()
            Dim assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            ' Optional settings you may tweak for performance improvements. Results may vary.
            application.DelayCompute = True
            application.DisplayAlerts = False
            application.Interactive = False
            application.ScreenUpdating = False

            If assemblyDocument IsNot Nothing Then
                Dim rootItem = New DocumentItem()
                rootItem.FileName = assemblyDocument.FullName

                ' Begin the recurisve extraction process.
                PopulateDocumentItems(assemblyDocument.Occurrences, rootItem)

                ' Write each DocumentItem to console.
                For Each documentItem In rootItem.AllDocumentItems
                    Console.WriteLine(documentItem.FileName)
                Next documentItem

                ' Demonstration of how to save the BOM to various formats.

                ' Convert the document items to JSON.
                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented)
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            If application IsNot Nothing Then
                application.DelayCompute = False
                application.DisplayAlerts = True
                application.Interactive = True
                application.ScreenUpdating = True
            End If

            OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Sub PopulateDocumentItems(ByVal occurrences As SolidEdgeAssembly.Occurrences, ByVal parentItem As DocumentItem)
        For Each occurrence As SolidEdgeAssembly.Occurrence In occurrences
            Dim occurrenceItem = New DocumentItem(occurrence)

            ' Make sure the DocumentItem hasn't already been added.
            If parentItem.DocumentItems.Contains(occurrenceItem) = False Then
                parentItem.DocumentItems.Add(occurrenceItem)

                If occurrence.SubOccurrences IsNot Nothing Then
                    PopulateDocumentItems(occurrence.SubOccurrences, occurrenceItem)
                End If
            End If
        Next occurrence
    End Sub

    Private Shared Sub PopulateDocumentItems(ByVal subOccurrences As SolidEdgeAssembly.SubOccurrences, ByVal parentItem As DocumentItem)
        For Each subOccurrence As SolidEdgeAssembly.SubOccurrence In subOccurrences
            Dim occurrenceItem = New DocumentItem(subOccurrence)

            ' Make sure the DocumentItem hasn't already been added.
            If parentItem.DocumentItems.Contains(occurrenceItem) = False Then
                parentItem.DocumentItems.Add(occurrenceItem)

                If subOccurrence.SubOccurrences IsNot Nothing Then
                    PopulateDocumentItems(subOccurrence.SubOccurrences, occurrenceItem)
                End If
            End If
        Next subOccurrence
    End Sub
End Class
