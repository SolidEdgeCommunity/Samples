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
        Dim filenames() As String = { "strainer.asm", "handle.par" }

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

            ' Get a reference to the Occurrences collection.
            occurrences = assemblyDocument.Occurrences

            ' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
            Dim trainingDirectory As New DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath())

            ' Add each occurrence in array.
            For i As Integer = 0 To transforms.Length - 1
                ' Build path to file.
                Dim filename As String = Path.Combine(trainingDirectory.FullName, filenames(i))

                ' Add the new occurrence using a transform.
                occurrence = occurrences.AddWithTransform(OccurrenceFileName:= filename, OriginX:= transforms(i)(0), OriginY:= transforms(i)(1), OriginZ:= transforms(i)(2), AngleX:= transforms(i)(3), AngleY:= transforms(i)(4), AngleZ:= transforms(i)(5))

            Next i

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
