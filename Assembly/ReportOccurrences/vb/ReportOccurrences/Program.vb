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
        Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get the active document.
            assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            If assemblyDocument IsNot Nothing Then
                ' Get a reference to the Occurrences collection.
                occurrences = assemblyDocument.Occurrences

                For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
                    ' Allocate a new array to hold transform.
                    Dim transform(5) As Double

                    ' Get the occurrence transform.
                    occurrence.GetTransform(OriginX:= transform(0), OriginY:= transform(1), OriginZ:= transform(2), AngleX:= transform(3), AngleY:= transform(4), AngleZ:= transform(5))

                    ' Report the occurrence transform.
                    Console.WriteLine("{0} transform:", occurrence.Name)
                    Console.WriteLine("OriginX: {0} (meters)", transform(0))
                    Console.WriteLine("OriginY: {0} (meters)", transform(1))
                    Console.WriteLine("OriginZ: {0} (meters)", transform(2))
                    Console.WriteLine("AngleX: {0} (radians)", transform(3))
                    Console.WriteLine("AngleY: {0} (radians)", transform(4))
                    Console.WriteLine("AngleZ: {0} (radians)", transform(5))
                    Console.WriteLine()

                    ' Allocate a new array to hold matrix.
                    Dim matrix As Array = Array.CreateInstance(GetType(Double), 16)

                    ' Get the occurrence matrix.
                    occurrence.GetMatrix(matrix)

                    ' Convert from System.Array to double[].  double[] is easier to work with.
                    Dim m() As Double = matrix.OfType(Of Double)().ToArray()

                    ' Report the occurrence matrix.
                    Console.WriteLine("{0} matrix:", occurrence.Name)
                    Console.WriteLine("|{0}, {1}, {2}, {3}|", m(0), m(1), m(2), m(3))
                    Console.WriteLine("|{0}, {1}, {2}, {3}|", m(4), m(5), m(6), m(7))
                    Console.WriteLine("|{0}, {1}, {2}, {3}|", m(8), m(9), m(10), m(11))
                    Console.WriteLine("|{0}, {1}, {2}, {3}|", m(12), m(13), m(14), m(15))

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
End Class
