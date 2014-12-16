Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading.Tasks

Namespace ExtractStructure
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
                    ' Stopwatch class is good for performance testing.
                    Dim stopwatch = New Stopwatch()
                    stopwatch.Start()

                    Dim rootItem = New OccurrenceItem()
                    rootItem.FileName = assemblyDocument.FullName

                    For Each occurrenceItem In GetOccurrenceItems(assemblyDocument.Occurrences)
                        rootItem.Occurrence.Add(occurrenceItem)
                    Next occurrenceItem

                    ' There are numerous ways to serialize data. In the following example, I'm
                    ' demonstrating using JSON.NET.
                    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented)
                    Dim xml = Newtonsoft.Json.JsonConvert.DeserializeXNode(json, "Root").ToString()

                    stopwatch.Stop()
                    Dim elapsedTime = stopwatch.Elapsed
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

        Private Shared Iterator Function GetOccurrenceItems(ByVal occurrences As SolidEdgeAssembly.Occurrences) As IEnumerable(Of OccurrenceItem)
            For Each occurrence As SolidEdgeAssembly.Occurrence In occurrences
                Dim occurrenceItem = New OccurrenceItem(occurrence)

                If occurrence.SubOccurrences IsNot Nothing Then
                    For Each childOccurrenceItem In GetOccurrenceItems(occurrence.SubOccurrences, occurrenceItem)
                        occurrenceItem.Occurrence.Add(childOccurrenceItem)
                    Next childOccurrenceItem
                End If

                Yield occurrenceItem
            Next occurrence
        End Function

        Private Shared Iterator Function GetOccurrenceItems(ByVal subOccurrences As SolidEdgeAssembly.SubOccurrences, ByVal parentOccurrenceItem As OccurrenceItem) As IEnumerable(Of OccurrenceItem)
            For Each subOccurrence As SolidEdgeAssembly.SubOccurrence In subOccurrences
                Dim childOccurrenceItem = New OccurrenceItem(subOccurrence)

                If subOccurrence.SubOccurrences IsNot Nothing Then
                    For Each childOccurrenceItem2 In GetOccurrenceItems(subOccurrence.SubOccurrences, childOccurrenceItem)
                        childOccurrenceItem.Occurrence.Add(childOccurrenceItem2)
                    Next childOccurrenceItem2
                End If

                parentOccurrenceItem.Occurrence.Add(childOccurrenceItem)

                Yield childOccurrenceItem
            Next subOccurrence
        End Function
    End Class
End Namespace
