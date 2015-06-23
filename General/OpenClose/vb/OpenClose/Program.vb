Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        ' Connect to Solid Edge.
        Dim application = SolidEdgeUtils.Connect(True, True)

        ' Get a reference to the Documents collection.
        Dim documents = application.Documents

        ' Get a folder that has Solid Edge files.
        Dim folder = New DirectoryInfo(SolidEdgeUtils.GetTrainingFolderPath())

        ' Get the installed version of Solid Edge.
        Dim solidEdgeVesion = application.GetVersion()

        ' Disable prompts.
        application.DisplayAlerts = False

        ' Process the files.
        For Each file In folder.EnumerateFiles("*.par", SearchOption.AllDirectories)
            Console.WriteLine(file.FullName)

            ' Open the document.
            Dim document = DirectCast(documents.Open(file.FullName), SolidEdgeFramework.SolidEdgeDocument)

            ' Give Solid Edge a chance to do processing.
            application.DoIdle()

            ' Prior to ST8, we needed a reference to a document to close it.
            ' That meant that SE can't fully close the document because we're holding a reference.
            If solidEdgeVesion.Major < 108 Then
                ' Close the document.
                document.Close()

                ' Release our reference to the document.
                Marshal.FinalReleaseComObject(document)
                document = Nothing

                ' Give SE a chance to do post processing (finish closing the document).
                application.DoIdle()
            Else
                ' Release our reference to the document.
                Marshal.FinalReleaseComObject(document)
                document = Nothing

                ' Starting with ST8, the Documents collection has a CloseDocument() method.
                documents.CloseDocument(file.FullName, False, Missing.Value, Missing.Value, True)
            End If
        Next file

        application.DisplayAlerts = True

        ' Additional cleanup.
        Marshal.FinalReleaseComObject(documents)
        Marshal.FinalReleaseComObject(application)
    End Sub
End Class
