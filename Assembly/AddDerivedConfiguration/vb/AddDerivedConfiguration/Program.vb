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
        Dim document As SolidEdgeAssembly.AssemblyDocument = Nothing
        Dim configurations As SolidEdgeAssembly.Configurations = Nothing
        Dim configuration As SolidEdgeAssembly.Configuration = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active assembly document.
            document = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            If document IsNot Nothing Then
                ' Get a reference to the Configurations collection.
                configurations = document.Configurations

                ' Configuration name has to be unique so for demonstration
                ' purposes, use a random number.
                Dim random As New Random()
                Dim configName As String = String.Format("Configuration {0}", random.Next())

                configuration = configurations.Item(1)

                Dim configList As Object = New String() { configuration.Name }

                Dim missing As Object = System.Reflection.Missing.Value

                ' NOTE: Not sure why but this is causing Solid Edge ST6 to crash.
                ' Add the new configuration.
                configuration = configurations.AddDerivedConfig(1, 0, 0, configList, missing, missing, configName)
            Else
                Throw New System.Exception("No active document")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
