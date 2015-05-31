Imports System
Imports System.Collections.Generic
Imports System.Drawing.Imaging
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Friend Class WindowHelper
    Public Const SRCCOPY As Integer = &HCC0020

    <DllImport("gdi32.dll")> _
    Shared Function BitBlt(ByVal hObject As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hObjectSource As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As Integer) As Boolean
    End Function

    <DllImport("gdi32.dll")> _
    Shared Function CreateCompatibleBitmap(ByVal hDC As IntPtr, ByVal nWidth As Integer, ByVal nHeight As Integer) As IntPtr
    End Function

    <DllImport("gdi32.dll")> _
    Shared Function CreateCompatibleDC(ByVal hDC As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")> _
    Shared Function DeleteDC(ByVal hDC As IntPtr) As Boolean
    End Function

    <DllImport("gdi32.dll")> _
    Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
    End Function

    <DllImport("gdi32.dll")> _
    Shared Function SelectObject(ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
    End Function

    <DllImport("User32.dll")> _
    Shared Function GetDC(ByVal hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("User32.dll")> _
    Shared Function ReleaseDC(ByVal hWnd As System.IntPtr, ByVal hDC As System.IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential)> _
    Friend Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Public Shared Sub SaveAsImageUsingBitBlt(ByVal window As SolidEdgeFramework.Window)
        Dim dialog As New System.Windows.Forms.SaveFileDialog()
        dialog.FileName = System.IO.Path.ChangeExtension(window.Caption, ".bmp")
        dialog.Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpeg)|*.jpeg|PNG (.png)|*.png|TIFF (.tiff)|*.tiff|WMF Image (.wmf)|*.wmf"
        dialog.FilterIndex = 1

        If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim handle As New IntPtr(window.DrawHwnd)

            ' Capture the window to an Image object.
            Using image As System.Drawing.Image = Capture(handle)
                Dim imageFormat As ImageFormat = Nothing

                ' Determine the selected image format.
                ' The index is 1-based.
                Select Case dialog.FilterIndex
                    Case 1
                        imageFormat = System.Drawing.Imaging.ImageFormat.Bmp
                    Case 2
                        imageFormat = System.Drawing.Imaging.ImageFormat.Gif
                    Case 3
                        imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
                    Case 4
                        imageFormat = System.Drawing.Imaging.ImageFormat.Png
                    Case 5
                        imageFormat = System.Drawing.Imaging.ImageFormat.Tiff
                    Case 6
                        imageFormat = System.Drawing.Imaging.ImageFormat.Wmf
                End Select

                Console.WriteLine("Saving {0}.", dialog.FileName)

                image.Save(dialog.FileName, imageFormat)
            End Using
        End If
    End Sub

    Public Shared Function Capture(ByVal hWnd As IntPtr) As System.Drawing.Image
        ' Get the device context of the client.
        Dim hdcSrc As IntPtr = GetDC(hWnd)

        ' Get the client rectangle.
        Dim windowRect As New RECT()
        GetClientRect(hWnd, windowRect)

        ' Calculate the size of the window.
        Dim width As Integer = windowRect.right - windowRect.left
        Dim height As Integer = windowRect.bottom - windowRect.top

        ' Create a new device context that is compatible with the source device context.
        Dim hdcDest As IntPtr = CreateCompatibleDC(hdcSrc)

        ' Creates a bitmap compatible with the device that is associated with the specified device context.
        Dim hBitmap As IntPtr = CreateCompatibleBitmap(hdcSrc, width, height)

        ' Select the new bitmap object into the destination device context.
        Dim hOld As IntPtr = SelectObject(hdcDest, hBitmap)

        ' Perform a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
        BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY)

        ' Restore selection.
        SelectObject(hdcDest, hOld)

        ' Clean up device contexts.
        DeleteDC(hdcDest)
        ReleaseDC(hWnd, hdcSrc)

        ' Create an Image from a handle to a GDI bitmap.
        Dim image As System.Drawing.Image = System.Drawing.Image.FromHbitmap(hBitmap)

        ' Free up the Bitmap object.
        DeleteObject(hBitmap)

        Return image
    End Function
End Class
