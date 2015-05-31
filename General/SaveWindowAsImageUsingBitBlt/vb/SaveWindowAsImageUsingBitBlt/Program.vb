Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim window As SolidEdgeFramework.Window = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Start()

                ' 3D windows are of type SolidEdgeFramework.Window.
                window = TryCast(application.ActiveWindow, SolidEdgeFramework.Window)

                If window IsNot Nothing Then
                    WindowHelper.SaveAsImageUsingBitBlt(window)
                Else
                    Throw New System.Exception("No active 3D window.")
                End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
