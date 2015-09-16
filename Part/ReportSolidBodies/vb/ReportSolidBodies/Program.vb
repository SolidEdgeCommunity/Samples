Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ReportSolidBodies
    Friend Class Program
        <STAThread>
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim models As SolidEdgePart.Models = Nothing
            Dim body As SolidEdgeGeometry.Body = Nothing
            Dim shells As SolidEdgeGeometry.Shells = Nothing
            Dim shell As SolidEdgeGeometry.Shell = Nothing
            Dim constructions As SolidEdgePart.Constructions = Nothing
            Dim constructionModel As SolidEdgePart.ConstructionModel = Nothing
            Dim bodyCount As Integer = 0

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Bring Solid Edge to the foreground.
                application.Activate()

                ' Get a reference to the active part document.
                partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

                If partDocument IsNot Nothing Then
                    models = partDocument.Models

                    ' Check for solid design bodies.
                    For Each model In models.OfType(Of SolidEdgePart.Model)()
                        body = DirectCast(model.Body, SolidEdgeGeometry.Body)

                        If body.IsSolid Then
                            shells = DirectCast(body.Shells, SolidEdgeGeometry.Shells)

                            For i As Integer = 1 To shells.Count
                                shell = DirectCast(shells.Item(i), SolidEdgeGeometry.Shell)

                                If shell IsNot Nothing Then
                                    If (shell.IsClosed) AndAlso (shell.IsVoid = False) Then
                                        bodyCount += 1
                                    End If
                                End If
                            Next i
                        End If
                    Next model

                    ' Check for solid construction bodies.
                    constructions = partDocument.Constructions

                    For i As Integer = 1 To constructions.Count
                        constructionModel = TryCast(constructions.Item(i), SolidEdgePart.ConstructionModel)

                        If constructionModel IsNot Nothing Then
                            body = DirectCast(constructionModel.Body, SolidEdgeGeometry.Body)

                            If body.IsSolid Then
                                bodyCount += 1
                            End If
                        End If
                    Next i

                    Console.WriteLine("Active part document contains ({0}) solid bodies.", bodyCount)
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
End Namespace
