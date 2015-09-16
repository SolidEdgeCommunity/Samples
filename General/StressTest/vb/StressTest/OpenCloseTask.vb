Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks

Namespace StressTest
    Public Class OpenCloseTask
        Inherits IsolatedTaskProxy

        Public Sub DoOpenClose(ByVal fileName As String)
            InvokeSTAThread(Of String)(AddressOf DoOpenCloseInternal, fileName)
        End Sub

        Private Sub DoOpenCloseInternal(ByVal fileName As String)
            'SolidEdgeFramework.Application application = null;
            Dim documents As SolidEdgeFramework.Documents = Nothing
'INSTANT VB NOTE: The variable document was renamed since Visual Basic does not handle local variables named the same as class members well:
            Dim document_Renamed As SolidEdgeFramework.SolidEdgeDocument = Nothing

            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            Try
                ' Get reference to application object.
'INSTANT VB NOTE: The variable application was renamed since Visual Basic does not handle local variables named the same as class members well:
                Dim application_Renamed = Me.Application

                ' Get reference to documents collection.
                documents = application_Renamed.Documents

                ' Open the document.
                document_Renamed = DirectCast(documents.Open(fileName), SolidEdgeFramework.SolidEdgeDocument)

                ' Not sure why but as of ST8, I had to add a Sleep() call to prevent crashing...
                System.Threading.Thread.Sleep(500)

                ' Do idle processing.
                application_Renamed.DoIdle()

                ' Close the document.
                document_Renamed.Close(False)

                'System.Threading.Thread.Sleep(500);

                ' Do idle processing.
                application_Renamed.DoIdle()

                documents.Close()
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
