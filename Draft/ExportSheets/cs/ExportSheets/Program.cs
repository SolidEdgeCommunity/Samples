using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using SolidEdgeCommunity.Reader.Draft;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

// Example usage (open file): ExportSheets.exe --emf --bmp --jpg --png --tif
// Example usage (closed file): ExportSheets.exe -f "C:\Draft1.dft" --emf --bmp --jpg --png --tif
namespace ExportSheets
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();

                // Leverage CommandLineParser NuGet package for parsing the command line.
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    // Default to EMF if no other option is specified.
                    if (options.IsRasterImageFormatSpecified == false)
                    {
                        options.ExportEMF = true;
                    }

                    if (String.IsNullOrEmpty(options.FileName))
                    {
                        ExportFromOpenFile(options);
                    }
                    else
                    {
                        // Begin the export process from the file directly.
                        ExportFromClosedFile(options);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Connects to a running instance of Solid Edge and extracts each sheet as an image.
        /// </summary>
        static void ExportFromOpenFile(Options options)
        {
            var application = SolidEdgeUtils.Connect();
            var draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>();
            var sections = draftDocument.Sections;
            var workingSection = sections.WorkingSection;

            if (File.Exists(draftDocument.FullName))
            {
                // Get the path to the file.
                var exportPath = Path.GetDirectoryName(draftDocument.FullName);

                // Get the file name without the extension.
                var baseFileName = Path.GetFileNameWithoutExtension(draftDocument.FullName);

                // Build the base path to the new file.
                baseFileName = Path.Combine(exportPath, baseFileName);

                foreach (SolidEdgeDraft.Sheet sheet in workingSection.Sheets)
                {
                    // Build the base path & filename of the image.
                    var baseSheetFileName = String.Format("{0} ({1})", baseFileName, sheet.Index);

                    // Sheets native viewer format is EMF so they can be exported directly.
                    if (options.ExportEMF)
                    {
                        // Build full path to EMF.
                        var emfFileName = String.Format("{0}.emf", baseSheetFileName);

                        // Save EMF.
                        // Note: SaveAsEnhancedMetafile() is an extension method from SolidEdge.Community.dll.
                        sheet.SaveAsEnhancedMetafile(emfFileName);

                        Console.WriteLine("Extracted '{0}'.", emfFileName);
                    }

                    // Other formats must go through a vector to raster conversion process.
                    // This conversion process can be slow. The reason is that most drawings
                    // have large dimensions. You may consider resizing during the conversion.
                    if (options.IsRasterImageFormatSpecified)
                    {
                        // Get a new instance of Metafile from sheet.
                        using (var metafile = sheet.GetEnhancedMetafile())
                        {
                            ExportMetafile(metafile, baseSheetFileName, options);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Draft must be saved prior to exporting.");
            }
        }

        /// <summary>
        /// Opens a draft file directly and extracts each sheet as an image.
        /// </summary>
        static void ExportFromClosedFile(Options options)
        {
            // Make sure the file exists.
            if (File.Exists(options.FileName))
            {
                // Open the file.
                using (var draftDocument = DraftDocument.Open(options.FileName))
                {
                    // Get the path to the file.
                    var exportPath = Path.GetDirectoryName(options.FileName);
                    
                    // Get the file name without the extension.
                    var baseFileName = Path.GetFileNameWithoutExtension(options.FileName);

                    // Build the base path to the new file.
                    baseFileName = Path.Combine(exportPath, baseFileName);

                    // Process each sheet.
                    foreach (var sheet in draftDocument.Sheets)
                    {
                        // Build the base path & filename of the image.
                        var baseSheetFileName = String.Format("{0} ({1})", baseFileName, sheet.Index);

                        // Sheets native viewer format is EMF so they can be exported directly.
                        if (options.ExportEMF)
                        {
                            // Build full path to EMF.
                            var emfFileName = String.Format("{0}.emf", baseSheetFileName);

                            // Save EMF.
                            sheet.SaveAsEmf(emfFileName);

                            Console.WriteLine("Extracted '{0}'.", emfFileName);
                        }

                        // Other formats must go through a vector to raster conversion process.
                        // This conversion process can be slow. The reason is that most drawings
                        // have large dimensions. You may consider resizing during the conversion.
                        if (options.IsRasterImageFormatSpecified)
                        {
                            // Get a new instance of Metafile from sheet.
                            using (var metafile = sheet.GetMetafile())
                            {
                                ExportMetafile(metafile, baseSheetFileName, options);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("File not found.", options.FileName);
            }
        }

        /// <summary>
        /// Converts a Metafile to specified image format(s).
        /// </summary>
        static void ExportMetafile(System.Drawing.Imaging.Metafile metafile, string baseSheetFileName, Options options)
        {
            if (options.ExportBMP)
            {
                // Build full path to BMP.
                var bmpFileName = String.Format("{0}.bmp", baseSheetFileName);

                // Convert EMF to BMP.
                metafile.Save(bmpFileName, ImageFormat.Bmp);

                Console.WriteLine("Extracted '{0}'.", bmpFileName);
            }

            if (options.ExportJPG)
            {
                // Build full path to JPG.
                var jpgFileName = String.Format("{0}.jpg", baseSheetFileName);

                // Convert EMF to JPG.
                metafile.Save(jpgFileName, ImageFormat.Jpeg);

                Console.WriteLine("Extracted '{0}'.", jpgFileName);
            }

            if (options.ExportPNG)
            {
                // Build full path to PNG.
                var pngFileName = String.Format("{0}.png", baseSheetFileName);

                // Convert EMF to PNG.
                metafile.Save(pngFileName, ImageFormat.Png);

                Console.WriteLine("Extracted '{0}'.", pngFileName);
            }

            if (options.ExportTIF)
            {
                // Build full path to TIF.
                var tifFileName = String.Format("{0}.tif", baseSheetFileName);

                // Convert EMF to TIF.
                metafile.Save(tifFileName, ImageFormat.Tiff);

                Console.WriteLine("Extracted '{0}'.", tifFileName);
            }
        }
    }
}
