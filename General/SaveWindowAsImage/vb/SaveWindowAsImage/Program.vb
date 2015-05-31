Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
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
                Dim extensions() As String = { ".jpg", ".bmp", ".tif" }

                Dim view As SolidEdgeFramework.View = Nothing
                Dim guid As Guid = System.Guid.NewGuid()
                Dim folder As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)

                If window Is Nothing Then
                    Throw New ArgumentNullException("window")
                End If

                ' Get a reference to the 3D view.
                view = window.View

                ' Save each extension.
                For Each extension As String In extensions
                    ' File saved to desktop.
                    Dim filename As String = System.IO.Path.ChangeExtension(guid.ToString(), extension)
                    filename = System.IO.Path.Combine(folder, filename)

                    Dim resolution As Double = 1.0 ' DPI - Larger values have better quality but also lead to larger file.
                    Dim colorDepth As Integer = 24
                    Dim width As Integer = window.UsableWidth
                    Dim height As Integer = window.UsableHeight

                    ' You can specify .bmp (Windows Bitmap), .tif (TIFF), or .jpg (JPEG).
                    view.SaveAsImage(Filename:= filename, Width:= width, Height:= height, AltViewStyle:= Nothing, Resolution:= resolution, ColorDepth:= colorDepth, ImageQuality:= SolidEdgeFramework.SeImageQualityType.seImageQualityHigh, Invert:= False)

                    Console.WriteLine("Saved '{0}'.", filename)
                Next extension
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
