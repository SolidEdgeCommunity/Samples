Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing
        Dim interferenceStatus As SolidEdgeAssembly.InterferenceStatusConstants = Nothing
        Dim compare As SolidEdgeConstants.InterferenceComparisonConstants = SolidEdgeConstants.InterferenceComparisonConstants.seInterferenceComparisonSet1vsAllOther
        Dim reportType As SolidEdgeConstants.InterferenceReportConstants = SolidEdgeConstants.InterferenceReportConstants.seInterferenceReportPartNames

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active assembly document.
            assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            If assemblyDocument IsNot Nothing Then
                ' Get a reference to the Occurrences collection.
                occurrences = assemblyDocument.Occurrences

                For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
                    Dim set1 As Array = Array.CreateInstance(occurrence.GetType(), 1)
                    Dim numInterferences As Object = 0
                    Dim retSet1 As Object = Array.CreateInstance(GetType(SolidEdgeAssembly.Occurrence), 0)
                    Dim retSet2 As Object = Array.CreateInstance(GetType(SolidEdgeAssembly.Occurrence), 0)
                    Dim confirmedInterference As Object = Nothing
                    Dim interferenceOccurrence As Object = Nothing

                    set1.SetValue(occurrence, 0)

                    ' Check interference.
                    assemblyDocument.CheckInterference(NumElementsSet1:= set1.Length, Set1:= set1, Status:= interferenceStatus, ComparisonMethod:= compare, NumElementsSet2:= 0, Set2:= Missing.Value, AddInterferenceAsOccurrence:= False, ReportFilename:= Missing.Value, ReportType:= reportType, NumInterferences:= numInterferences, InterferingPartsSet1:= retSet1, InterferingPartsOtherSet:= retSet2, ConfirmedInterference:= confirmedInterference, InterferenceOccurrence:= interferenceOccurrence, IgnoreThreadInterferences:= Missing.Value)

                    ' Process status.
                    Select Case interferenceStatus
                        Case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusNoInterference
                        Case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedAndProbableInterference, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedInterference, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusIncompleteAnalysis, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusProbableInterference
                            If retSet2 IsNot Nothing Then
                                For j As Integer = 0 To (DirectCast(numInterferences, Integer)) - 1
                                    Dim obj1 As Object = DirectCast(retSet1, Array).GetValue(j)
                                    Dim obj2 As Object = DirectCast(retSet2, Array).GetValue(j)

                                    ' Use helper class to get the object type.
                                    Dim objectType1 = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgeFramework.ObjectType)(obj1, "Type", CType(0, SolidEdgeFramework.ObjectType))
                                    Dim objectType2 = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgeFramework.ObjectType)(obj2, "Type", CType(0, SolidEdgeFramework.ObjectType))

                                    Dim reference1 As SolidEdgeFramework.Reference = Nothing
                                    Dim reference2 As SolidEdgeFramework.Reference = Nothing
                                    Dim occurrence1 As SolidEdgeAssembly.Occurrence = Nothing
                                    Dim occurrence2 As SolidEdgeAssembly.Occurrence = Nothing

                                    Select Case objectType1
                                        Case SolidEdgeFramework.ObjectType.igReference
                                            reference1 = DirectCast(obj1, SolidEdgeFramework.Reference)
                                        Case SolidEdgeFramework.ObjectType.igPart, SolidEdgeFramework.ObjectType.igOccurrence
                                            occurrence1 = DirectCast(obj1, SolidEdgeAssembly.Occurrence)
                                    End Select

                                    Select Case objectType2
                                        Case SolidEdgeFramework.ObjectType.igReference
                                            reference2 = DirectCast(obj2, SolidEdgeFramework.Reference)
                                        Case SolidEdgeFramework.ObjectType.igPart, SolidEdgeFramework.ObjectType.igOccurrence
                                            occurrence2 = DirectCast(obj2, SolidEdgeAssembly.Occurrence)
                                    End Select
                                Next j
                            End If
                    End Select
                Next occurrence
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
