Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim relations3d As SolidEdgeAssembly.Relations3d = Nothing
        Dim groundRelation3d As SolidEdgeAssembly.GroundRelation3d = Nothing
        Dim axialRelation3d As SolidEdgeAssembly.AxialRelation3d = Nothing
        Dim planarRelation3d As SolidEdgeAssembly.PlanarRelation3d = Nothing
        Dim occurrence1 As SolidEdgeAssembly.Occurrence = Nothing
        Dim occurrence2 As SolidEdgeAssembly.Occurrence = Nothing
        Dim detailedStatus As SolidEdgeAssembly.Relation3dDetailedStatusConstants
        Dim status As SolidEdgeAssembly.Relation3dStatusConstants

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active document.
            assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            ' Get a reference to the Relations3d collection.
            relations3d = assemblyDocument.Relations3d

            For Each relation3d In relations3d.OfType(Of Object)()
                Try
                    ' Not used in this sample but a good example of how to get the runtime type.
                    Dim relationType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(relation3d)

                    ' Use helper class to get the object type.
                    Dim relationObjectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgeFramework.ObjectType)(relation3d, "Type", CType(0, SolidEdgeFramework.ObjectType))

                    ' Reset statuses.
                    detailedStatus = CType(0, SolidEdgeAssembly.Relation3dDetailedStatusConstants)
                    status = CType(0, SolidEdgeAssembly.Relation3dStatusConstants)

                    ' Handle specific object type. There are other possible relation types...
                    Select Case relationObjectType
                        Case SolidEdgeFramework.ObjectType.igGroundRelation3d
                            ' Cast relation3d object to GroundRelation3d type.
                            groundRelation3d = DirectCast(relation3d, SolidEdgeAssembly.GroundRelation3d)

                            ' Get a reference to the grounded occurrence.
                            occurrence1 = groundRelation3d.Occurrence

                            ' Get the detailed status.
                            detailedStatus = groundRelation3d.DetailedStatus

                            ' Get the status.
                            status = groundRelation3d.Status

                        Case SolidEdgeFramework.ObjectType.igAxialRelation3d
                            ' Cast relation3d object to AxialRelation3d type.
                            axialRelation3d = DirectCast(relation3d, SolidEdgeAssembly.AxialRelation3d)

                            ' Get a reference to the related occurrences.
                            occurrence1 = axialRelation3d.Occurrence1
                            occurrence2 = axialRelation3d.Occurrence2

                            ' Get the detailed status.
                            detailedStatus = axialRelation3d.DetailedStatus

                            ' Get the status.
                            status = axialRelation3d.Status

                        Case SolidEdgeFramework.ObjectType.igPlanarRelation3d
                            ' Cast relation3d object to PlanarRelation3d type.
                            planarRelation3d = DirectCast(relation3d, SolidEdgeAssembly.PlanarRelation3d)

                            ' Get a reference to the related occurrences.
                            occurrence1 = planarRelation3d.Occurrence1
                            occurrence2 = planarRelation3d.Occurrence2

                            ' Get the detailed status.
                            detailedStatus = planarRelation3d.DetailedStatus

                            ' Get the status.
                            status = planarRelation3d.Status
                        Case Else
                    End Select

                    ' Analyze the detailed status.
                    Select Case detailedStatus
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusBetweenFixed
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusBetweenSetMembers
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusMissingGeometry
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusSolved
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusSuppressed
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusUnknown
                        Case SolidEdgeAssembly.Relation3dDetailedStatusConstants.igRelation3dDetailedStatusUnsatisfied
                    End Select

                    ' Analyze the status.
                    Select Case status
                        Case SolidEdgeAssembly.Relation3dStatusConstants.igRelation3dStatusSolved
                        Case SolidEdgeAssembly.Relation3dStatusConstants.igRelation3dStatusUnsolved
                    End Select
                Catch
                End Try
            Next relation3d
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
