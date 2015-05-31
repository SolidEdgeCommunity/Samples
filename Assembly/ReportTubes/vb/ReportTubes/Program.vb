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
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing
        Dim tube As SolidEdgeAssembly.Tube = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            Try
                ' Get the active document.
                assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)
            Catch
            End Try

            ' If there is not an active assembly, we can use an assembly from the training folder.
            If assemblyDocument Is Nothing Then
                documents = application.Documents

                ' Build path to part file.
                Dim filename As String = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), "Try It\zone_try_it.asm")
                assemblyDocument = DirectCast(documents.Open(filename), SolidEdgeAssembly.AssemblyDocument)
            End If

            If assemblyDocument IsNot Nothing Then
                ' Get a reference to the Occurrences collection.
                occurrences = assemblyDocument.Occurrences

                For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
                    If occurrence.IsTube() Then
                        tube = occurrence.GetTube()

                        Console.WriteLine("Occurrences[{0}] is a tube.", occurrence.Index)
                        Console.WriteLine("PartFileName: {0}", tube.PartFileName)

                        Dim CutLength As Object = 0.0
                        Dim NumOfBends As Object = 0
                        Dim FeedLengthArray As Object = New Double() { }
                        Dim RotationAngleArray As Object = New Double() { }
                        Dim BendRadiusArray As Object = New Double() { }
                        Dim ReverseBendOrder As Object = 0
                        Dim SaveToFileName As Object = Missing.Value
                        Dim BendAngleArray As Object = New Double() { }

                        tube.BendTable(CutLength:= CutLength, NumOfBends:= NumOfBends, FeedLength:= FeedLengthArray, RotationAngle:= RotationAngleArray, BendRadius:= BendRadiusArray, ReverseBendOrder:= ReverseBendOrder, SaveToFileName:= SaveToFileName, BendAngle:= BendAngleArray)

                        Dim FeedLength As New StringBuilder()
                        For Each d As Double In DirectCast(FeedLengthArray, Double())
                            FeedLength.AppendFormat("{0}, ", d)
                        Next d

                        Dim RotationAngle As New StringBuilder()
                        For Each d As Double In DirectCast(RotationAngleArray, Double())
                            RotationAngle.AppendFormat("{0}, ", d)
                        Next d

                        Dim BendRadius As New StringBuilder()
                        For Each d As Double In DirectCast(BendRadiusArray, Double())
                            BendRadius.AppendFormat("{0}, ", d)
                        Next d

                        Dim BendAngle As New StringBuilder()
                        For Each d As Double In DirectCast(BendAngleArray, Double())
                            BendAngle.AppendFormat("{0}, ", d)
                        Next d

                        Console.WriteLine("BendTable information:")
                        Console.WriteLine("CutLength: {0}", CutLength)
                        Console.WriteLine("NumOfBends: {0}", NumOfBends)
                        Console.WriteLine("FeedLength: {0}", FeedLength.ToString().Trim().TrimEnd(","c))
                        Console.WriteLine("RotationAngle: {0}", RotationAngle.ToString().Trim().TrimEnd(","c))
                        Console.WriteLine("BendRadius: {0}", BendRadius.ToString().Trim().TrimEnd(","c))
                        Console.WriteLine("BendAngle: {0}", BendAngle.ToString().Trim().TrimEnd(","c))
                        Console.WriteLine()
                    End If
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
