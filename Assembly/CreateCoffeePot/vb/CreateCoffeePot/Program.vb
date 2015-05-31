Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing
        Dim occurrence As SolidEdgeAssembly.Occurrence = Nothing
        Dim transformFilenames() As String = { "strainer.asm", "handle.par" }

        ' A single-dimension array that defines a valid transformation matrix. 
        Dim matrix() As Double = { 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.07913, 1.0 }

        ' Jagged array
        ' {OriginX, OriginY, OriginZ, AngleX, AngleY, AngleZ}
        ' Origin in meters.
        ' Angle in radians.
        Dim transforms()() As Double = { _
            New Double() {0, 0, 0.02062, 0, 0, 0}, _
            New Double() {-0.06943, -0.00996, 0.05697, 0, 0, 0} _
        }

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new assembly document.
            assemblyDocument = documents.AddAssemblyDocument()

            ' Always a good idea to give SE a chance to breathe.
            application.DoIdle()

            ' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
            Dim trainingDirectory As New DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath())

            ' Build path to file.
            Dim filename As String = System.IO.Path.Combine(trainingDirectory.FullName, "Coffee Pot.par")

            ' Get a reference to the occurrences collection.
            occurrences = assemblyDocument.Occurrences

            ' Add the base feature at 0 (X), 0 (Y), 0 (Z).
            occurrences.AddByFilename(OccurrenceFileName:= filename)

            ' Add each occurrence in array.
            For i As Integer = 0 To transforms.Length - 1
                ' Build path to file.
                filename = Path.Combine(trainingDirectory.FullName, transformFilenames(i))

                ' Add the new occurrence using a transform.
                occurrence = occurrences.AddWithTransform(OccurrenceFileName:= filename, OriginX:= transforms(i)(0), OriginY:= transforms(i)(1), OriginZ:= transforms(i)(2), AngleX:= transforms(i)(3), AngleY:= transforms(i)(4), AngleZ:= transforms(i)(5))
            Next i

            ' Build path to part file.
            filename = System.IO.Path.Combine(trainingDirectory.FullName, "Strap Handle.par")

            ' Convert from double[] to System.Array.
            Dim matrixArray As Array = matrix

            ' Add the new occurrence using a matrix.
            occurrences.AddWithMatrix(OccurrenceFileName:= filename, Matrix:= matrixArray)

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
