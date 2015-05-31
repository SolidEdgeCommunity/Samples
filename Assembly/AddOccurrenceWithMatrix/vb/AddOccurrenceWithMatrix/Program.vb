Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
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

        ' A single-dimension array that defines a valid transformation matrix. 
        Dim matrix() As Double = { 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 0.07913, 1.0 }

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

            ' Build path to part file.
            Dim filename As String = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), "Strap Handle.par")

            ' Get a reference to the Occurrences collection.
            occurrences = assemblyDocument.Occurrences

            ' Convert from double[] to System.Array.
            Dim matrixArray As Array = matrix

            ' Add the new occurrence using a matrix.
            occurrence = occurrences.AddWithMatrix(OccurrenceFileName:= filename, Matrix:= matrixArray)

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
