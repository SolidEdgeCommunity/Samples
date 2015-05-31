Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim environments As SolidEdgeFramework.Environments = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the Environments collection.
            environments = application.Environments

            ' Loop through each addin.
            For Each environment In environments.OfType(Of SolidEdgeFramework.Environment)()
                Console.WriteLine("Caption: {0}", environment.Caption)
                Console.WriteLine("CATID: {0}", environment.CATID)
                Console.WriteLine("Index: {0}", environment.Index)
                Console.WriteLine("Name: {0}", environment.Name)
                Console.WriteLine("SubTypeName: {0}", environment.SubTypeName)
                Console.WriteLine()
            Next environment
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
