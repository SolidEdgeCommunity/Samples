Imports Newtonsoft.Json
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

            ' Connect to a running instance of Solid Edge.
            application = SolidEdgeUtils.Connect()

            ' Connect to the active assembly document.
            Dim assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            ' Optional settings you may tweak for performance improvements. Results may vary.
            application.DelayCompute = True
            application.DisplayAlerts = False
            application.Interactive = False
            application.ScreenUpdating = False

            If assemblyDocument IsNot Nothing Then
                Dim rootBomItem = New BomItem()
                rootBomItem.FileName = assemblyDocument.FullName

                ' Begin the recurisve extraction process.
                PopulateBom(0, assemblyDocument, rootBomItem)

                ' Write each BomItem to console.
                For Each bomItem In rootBomItem.AllChildren
                    Console.WriteLine("{0}" & ControlChars.Tab & "{1}" & ControlChars.Tab & "{2}" & ControlChars.Tab & "{3}" & ControlChars.Tab & "{4}", bomItem.Level, bomItem.DocumentNumber, bomItem.Revision, bomItem.Title, bomItem.Quantity)
                Next bomItem

                ' Demonstration of how to save the BOM to various formats.

                ' Define the Json serializer settings.
                Dim jsonSettings = New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore}

                ' Convert the BOM to JSON.
                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(rootBomItem, Newtonsoft.Json.Formatting.Indented, jsonSettings)
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

    Private Shared Sub PopulateBom(ByVal level As Integer, ByVal assemblyDocument As SolidEdgeAssembly.AssemblyDocument, ByVal parentBomItem As BomItem)
        ' Increment level (depth).
        level += 1

        ' This sample BOM is not exploded. Define a dictionary to store unique occurrences.
        Dim uniqueOccurrences As New Dictionary(Of String, SolidEdgeAssembly.Occurrence)()

        ' Populate the unique occurrences dictionary.
        For Each occurrence As SolidEdgeAssembly.Occurrence In assemblyDocument.Occurrences
            ' To make sure nothing silly happens with our dictionary key, force the file path to lowercase.
            Dim lowerFileName = occurrence.OccurrenceFileName.ToLower()

            ' If the dictionary does not already contain the occurrence, add it.
            If uniqueOccurrences.ContainsKey(lowerFileName) = False Then
                uniqueOccurrences.Add(lowerFileName, occurrence)
            End If
        Next occurrence

        ' Loop through the unique occurrences.
        For Each occurrence As SolidEdgeAssembly.Occurrence In uniqueOccurrences.Values.ToArray()
            ' Filter out certain occurrences.
            If Not occurrence.IncludeInBom Then
                Continue For
            End If
            If occurrence.IsPatternItem Then
                Continue For
            End If
            If occurrence.OccurrenceDocument Is Nothing Then
                Continue For
            End If

            ' Create an instance of the child BomItem.
            Dim bomItem = New BomItem(occurrence, level)

            ' Add the child BomItem to the parent.
            parentBomItem.Children.Add(bomItem)

            If bomItem.IsSubassembly = True Then
                ' Sub Assembly. Recurisve call to drill down.
                PopulateBom(level, DirectCast(occurrence.OccurrenceDocument, SolidEdgeAssembly.AssemblyDocument), bomItem)
            End If
        Next occurrence
    End Sub
End Class
