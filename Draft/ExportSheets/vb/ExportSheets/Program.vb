Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports SolidEdgeCommunity.Reader.Draft
Imports System
Imports System.Collections.Generic
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Text

' Example usage (open file): ExportSheets.exe --emf --bmp --jpg --png --tif
' Example usage (closed file): ExportSheets.exe -f "C:\Draft1.dft" --emf --bmp --jpg --png --tif
Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Try
            Dim options = New Options()

            ' Leverage CommandLineParser NuGet package for parsing the command line.
            If CommandLine.Parser.Default.ParseArguments(args, options) Then
                ' Default to EMF if no other option is specified.
                If options.IsRasterImageFormatSpecified = False Then
                    options.ExportEMF = True
                End If

                If String.IsNullOrEmpty(options.FileName) Then
                    ExportFromOpenFile(options)
                Else
                    ' Begin the export process from the file directly.
                    ExportFromClosedFile(options)
                End If
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Connects to a running instance of Solid Edge and extracts each sheet as an image.
    ''' </summary>
    Private Shared Sub ExportFromOpenFile(ByVal options As Options)
        Dim application = SolidEdgeUtils.Connect()
        Dim draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)()
        Dim sections = draftDocument.Sections
        Dim workingSection = sections.WorkingSection

        If File.Exists(draftDocument.FullName) Then
            ' Get the path to the file.
            Dim exportPath = Path.GetDirectoryName(draftDocument.FullName)

            ' Get the file name without the extension.
            Dim baseFileName = Path.GetFileNameWithoutExtension(draftDocument.FullName)

            ' Build the base path to the new file.
            baseFileName = Path.Combine(exportPath, baseFileName)

            For Each sheet As SolidEdgeDraft.Sheet In workingSection.Sheets
                ' Build the base path & filename of the image.
                Dim baseSheetFileName = String.Format("{0} ({1})", baseFileName, sheet.Index)

                ' Sheets native viewer format is EMF so they can be exported directly.
                If options.ExportEMF Then
                    ' Build full path to EMF.
                    Dim emfFileName = String.Format("{0}.emf", baseSheetFileName)

                    ' Save EMF.
                    ' Note: SaveAsEnhancedMetafile() is an extension method from SolidEdge.Community.dll.
                    sheet.SaveAsEnhancedMetafile(emfFileName)

                    Console.WriteLine("Extracted '{0}'.", emfFileName)
                End If

                ' Other formats must go through a vector to raster conversion process.
                ' This conversion process can be slow. The reason is that most drawings
                ' have large dimensions. You may consider resizing during the conversion.
                If options.IsRasterImageFormatSpecified Then
                    ' Get a new instance of Metafile from sheet.
                    Using metafile = sheet.GetEnhancedMetafile()
                        ExportMetafile(metafile, baseSheetFileName, options)
                    End Using
                End If
            Next sheet
        Else
            Console.WriteLine("Draft must be saved prior to exporting.")
        End If
    End Sub

    ''' <summary>
    ''' Opens a draft file directly and extracts each sheet as an image.
    ''' </summary>
    Private Shared Sub ExportFromClosedFile(ByVal options As Options)
        ' Make sure the file exists.
        If File.Exists(options.FileName) Then
            ' Open the file.
            Using draftDocument = SolidEdgeCommunity.Reader.Draft.DraftDocument.Open(options.FileName)
                ' Get the path to the file.
                Dim exportPath = Path.GetDirectoryName(options.FileName)

                ' Get the file name without the extension.
                Dim baseFileName = Path.GetFileNameWithoutExtension(options.FileName)

                ' Build the base path to the new file.
                baseFileName = Path.Combine(exportPath, baseFileName)

                ' Process each sheet.
                For Each sheet In draftDocument.Sheets
                    ' Build the base path & filename of the image.
                    Dim baseSheetFileName = String.Format("{0} ({1})", baseFileName, sheet.Index)

                    ' Sheets native viewer format is EMF so they can be exported directly.
                    If options.ExportEMF Then
                        ' Build full path to EMF.
                        Dim emfFileName = String.Format("{0}.emf", baseSheetFileName)

                        ' Save EMF.
                        sheet.SaveAsEmf(emfFileName)

                        Console.WriteLine("Extracted '{0}'.", emfFileName)
                    End If

                    ' Other formats must go through a vector to raster conversion process.
                    ' This conversion process can be slow. The reason is that most drawings
                    ' have large dimensions. You may consider resizing during the conversion.
                    If options.IsRasterImageFormatSpecified Then
                        ' Get a new instance of Metafile from sheet.
                        Using metafile = sheet.GetMetafile()
                            ExportMetafile(metafile, baseSheetFileName, options)
                        End Using
                    End If
                Next sheet
            End Using
        Else
            Throw New FileNotFoundException("File not found.", options.FileName)
        End If
    End Sub

    ''' <summary>
    ''' Converts a Metafile to specified image format(s).
    ''' </summary>
    Private Shared Sub ExportMetafile(ByVal metafile As System.Drawing.Imaging.Metafile, ByVal baseSheetFileName As String, ByVal options As Options)
        If options.ExportBMP Then
            ' Build full path to BMP.
            Dim bmpFileName = String.Format("{0}.bmp", baseSheetFileName)

            ' Convert EMF to BMP.
            metafile.Save(bmpFileName, ImageFormat.Bmp)

            Console.WriteLine("Extracted '{0}'.", bmpFileName)
        End If

        If options.ExportJPG Then
            ' Build full path to JPG.
            Dim jpgFileName = String.Format("{0}.jpg", baseSheetFileName)

            ' Convert EMF to JPG.
            metafile.Save(jpgFileName, ImageFormat.Jpeg)

            Console.WriteLine("Extracted '{0}'.", jpgFileName)
        End If

        If options.ExportPNG Then
            ' Build full path to PNG.
            Dim pngFileName = String.Format("{0}.png", baseSheetFileName)

            ' Convert EMF to PNG.
            metafile.Save(pngFileName, ImageFormat.Png)

            Console.WriteLine("Extracted '{0}'.", pngFileName)
        End If

        If options.ExportTIF Then
            ' Build full path to TIF.
            Dim tifFileName = String.Format("{0}.tif", baseSheetFileName)

            ' Convert EMF to TIF.
            metafile.Save(tifFileName, ImageFormat.Tiff)

            Console.WriteLine("Extracted '{0}'.", tifFileName)
        End If
    End Sub
End Class
