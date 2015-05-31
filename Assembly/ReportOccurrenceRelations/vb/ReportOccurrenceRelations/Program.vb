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
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active assembly document.
            Dim document = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            If document IsNot Nothing Then
                ' Get a reference to the occurrences collection.
                Dim occurrences = document.Occurrences

                For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
                    Console.WriteLine("Processing occurrence {0} relations.", occurrence.Name)

                    Dim relations3d = DirectCast(occurrence.Relations3d, SolidEdgeAssembly.Relations3d)

                    For Each relation3d In relations3d
                        ' Determine the relation object type.
                        Dim objectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgeFramework.ObjectType)(relation3d, "Type")

                        Select Case objectType
                            Case SolidEdgeFramework.ObjectType.igAngularRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.AngularRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igAxialRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.AxialRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igCamFollowerRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.CamFollowerRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igCenterPlaneRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.CenterPlaneRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igGearRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.GearRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igGroundRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.GroundRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igPathRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.PathRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igPlanarRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.PlanarRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igPointRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.PointRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igRigidSetRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.RigidSetRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentAngularRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentAngularRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentDirectionRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentDirectionRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentDistanceRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentDistanceRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentPointRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentPointRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentRadiusRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentRadiusRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.seSegmentTangentRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.SegmentTangentRelation3d), objectType)
                            Case SolidEdgeFramework.ObjectType.igTangentRelation3d
                                ReportRelation(DirectCast(relation3d, SolidEdgeAssembly.TangentRelation3d), objectType)
                            Case Else
                        End Select
                    Next relation3d

                    Console.WriteLine("----------------------------------")
                    Console.WriteLine()
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

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.AngularRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("Angle: {0}", relation3d.Angle)
        Catch
        End Try

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("RangedAngle: {0}", relation3d.RangedAngle)
        Catch
        End Try

        Try
            Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh)
        Catch
        End Try

        Try
            Console.WriteLine("RangeLow: {0}", relation3d.RangeLow)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.AxialRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("FixedParallelOffset: {0}", relation3d.FixedParallelOffset)
        Catch
        End Try

        Try
            Console.WriteLine("FixedRotate: {0}", relation3d.FixedRotate)
        Catch
        End Try

        Try
            Console.WriteLine("Offset: {0}", relation3d.Offset)
        Catch
        End Try

        Try
            Console.WriteLine("Orientation: {0}", relation3d.Orientation)
        Catch
        End Try

        Try
            Console.WriteLine("ParallelOffset: {0}", relation3d.ParallelOffset)
        Catch
        End Try

        Try
            Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset)
        Catch
        End Try

        Try
            Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh)
        Catch
        End Try

        Try
            Console.WriteLine("RangeLow: {0}", relation3d.RangeLow)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.CamFollowerRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.CenterPlaneRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.GearRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("RatioValue1: {0}", relation3d.RatioValue1)
        Catch
        End Try

        Try
            Console.WriteLine("RatioValue2: {0}", relation3d.RatioValue2)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.GroundRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.PathRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.PlanarRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("FixedOffset: {0}", relation3d.FixedOffset)
        Catch
        End Try

        Try
            Console.WriteLine("NormalsAligned: {0}", relation3d.NormalsAligned)
        Catch
        End Try

        Try
            Console.WriteLine("Offset: {0}", relation3d.Offset)
        Catch
        End Try

        Try
            Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset)
        Catch
        End Try

        Try
            Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh)
        Catch
        End Try

        Try
            Console.WriteLine("RangeLow: {0}", relation3d.RangeLow)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.PointRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset)
        Catch
        End Try

        Try
            Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh)
        Catch
        End Try

        Try
            Console.WriteLine("RangeLow: {0}", relation3d.RangeLow)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.RigidSetRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("OccurrenceCount: {0}", relation3d.OccurrenceCount)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentAngularRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("AngleCounterclockwise: {0}", relation3d.AngleCounterclockwise)
        Catch
        End Try

        Try
            Console.WriteLine("AngleToPositiveDirection: {0}", relation3d.AngleToPositiveDirection)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentDirectionRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DirectionType: {0}", relation3d.DirectionType)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentDistanceRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DistanceType: {0}", relation3d.DistanceType)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentPointRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentRadiusRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("Radius: {0}", relation3d.Radius)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.SegmentTangentRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Console.WriteLine()
    End Sub

    Private Shared Sub ReportRelation(ByVal relation3d As SolidEdgeAssembly.TangentRelation3d, ByVal objectType As SolidEdgeFramework.ObjectType)
        Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType)

        Try
            Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus)
        Catch
        End Try

        Try
            Console.WriteLine("HalfSpacePositive: {0}", relation3d.HalfSpacePositive)
        Catch
        End Try

        Try
            Console.WriteLine("Offset: {0}", relation3d.Offset)
        Catch
        End Try

        Try
            Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset)
        Catch
        End Try

        Try
            Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh)
        Catch
        End Try

        Try
            Console.WriteLine("RangeLow: {0}", relation3d.RangeLow)
        Catch
        End Try

        Try
            Console.WriteLine("Status: {0}", relation3d.Status)
        Catch
        End Try

        Try
            Console.WriteLine("Suppress: {0}", relation3d.Suppress)
        Catch
        End Try

        Console.WriteLine()
    End Sub
End Class
