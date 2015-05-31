Imports CommandLine
Imports CommandLine.Text
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Friend Class Options
    Private _fileName As String

    <[Option]("f"c, "file", HelpText := "The .dft to process.")> _
    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = Path.GetFullPath(value)
        End Set
    End Property

    <[Option]("emf", DefaultValue := False, HelpText := "Export an EMF for each sheet.")> _
    Public Property ExportEMF() As Boolean

    <[Option]("bmp", DefaultValue := False, HelpText := "Export a BMP for each sheet.")> _
    Public Property ExportBMP() As Boolean

    <[Option]("jpg", DefaultValue := False, HelpText := "Export a JPG for each sheet.")> _
    Public Property ExportJPG() As Boolean

    <[Option]("png", DefaultValue := False, HelpText := "Export a PNG for each sheet.")> _
    Public Property ExportPNG() As Boolean

    <[Option]("tif", DefaultValue := False, HelpText := "Export a TIF for each sheet.")> _
    Public Property ExportTIF() As Boolean

    Public ReadOnly Property IsRasterImageFormatSpecified() As Boolean
        Get
            If (ExportBMP) OrElse (ExportJPG) OrElse (ExportPNG) OrElse (ExportTIF) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    <ParserState> _
    Public Property LastParserState() As IParserState

    <HelpOption> _
    Public Function GetUsage() As String
        Return HelpText.AutoBuild(Me)
    End Function
End Class
