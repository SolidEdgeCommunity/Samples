Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ReportVariables
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim document As SolidEdgePart.PartDocument = Nothing
            Dim variables As SolidEdgeFramework.Variables = Nothing
            Dim variableList As SolidEdgeFramework.VariableList = Nothing
            Dim variable As SolidEdgeFramework.variable = Nothing
            Dim dimension As SolidEdgeFrameworkSupport.Dimension = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect()

                ' Get a reference to the active document.
                document = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

                ' Make sure we have a document.
                If document IsNot Nothing Then
                    VariablesHelper.ReportVariables(document)
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
End Namespace
