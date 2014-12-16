Imports Newtonsoft.Json
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


Namespace ExtractBOM
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

                    Dim rootItem = New BomItem()
                    rootItem.FileName = assemblyDocument.FullName

                    For Each bomItem In GetBomItems(assemblyDocument)
                        rootItem.Occurrence.Add(bomItem)
                    Next bomItem

                    ' There are numerous ways to serialize data. In the following example, I'm
                    ' demonstrating using JSON.NET.
                    Dim jsonSettings = New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore}
                    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented, jsonSettings)
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

        Private Shared Iterator Function GetBomItems(ByVal assemblyDocument As SolidEdgeAssembly.AssemblyDocument) As IEnumerable(Of BomItem)
            For Each occurrence As SolidEdgeAssembly.Occurrence In assemblyDocument.Occurrences
                Dim bomItem As New BomItem(occurrence)

                ' Filter out certain occurrences.
                If Not occurrence.IncludeInBom Then
                    Continue For
                End If
                If occurrence.IsPatternItem Then
                    Continue For
                End If

                If occurrence.FileMissing() = False Then
                    If occurrence.OccurrenceDocument Is Nothing Then
                        Continue For
                    End If

                    ' Get a reference to the SolidEdgeDocument.
                    Dim document As SolidEdgeFramework.SolidEdgeDocument = DirectCast(occurrence.OccurrenceDocument, SolidEdgeFramework.SolidEdgeDocument)

                    If occurrence.Subassembly Then
                        Dim occurrenceAssemblyDocument = DirectCast(occurrence.OccurrenceDocument, SolidEdgeAssembly.AssemblyDocument)
                        For Each childBomItem In GetBomItems(occurrenceAssemblyDocument)
                            bomItem.Occurrence.Add(childBomItem)
                        Next childBomItem
                    End If
                Else
                    bomItem.Missing = True
                End If

                Yield bomItem
            Next occurrence
        End Function
    End Class
End Namespace
