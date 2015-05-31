Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim bSilent = False

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            If application IsNot Nothing Then
                If bSilent Then
                    ' Disable alerts. This will prevent the Save dialog from showing.
                    application.DisplayAlerts = False
                End If

                ' Get a reference to the documents collection.
                documents = application.Documents

                ' Close all documents.
                documents.Close()
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            If application IsNot Nothing Then
                ' Re-enable alerts.
                application.DisplayAlerts = True
            End If

            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
