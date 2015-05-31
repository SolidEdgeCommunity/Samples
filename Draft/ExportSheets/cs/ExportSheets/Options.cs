using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ExportSheets
{
    class Options
    {
        private string _fileName;

        [Option('f', "file", HelpText = "The .dft to process.")]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = Path.GetFullPath(value); }
        }

        [Option("emf", DefaultValue = false, HelpText = "Export an EMF for each sheet.")]
        public bool ExportEMF { get; set; }

        [Option("bmp", DefaultValue = false, HelpText = "Export a BMP for each sheet.")]
        public bool ExportBMP { get; set; }

        [Option("jpg", DefaultValue = false, HelpText = "Export a JPG for each sheet.")]
        public bool ExportJPG { get; set; }

        [Option("png", DefaultValue = false, HelpText = "Export a PNG for each sheet.")]
        public bool ExportPNG { get; set; }

        [Option("tif", DefaultValue = false, HelpText = "Export a TIF for each sheet.")]
        public bool ExportTIF { get; set; }

        public bool IsRasterImageFormatSpecified
        {
            get
            {
                if ((ExportBMP) || (ExportJPG) || (ExportPNG) || (ExportTIF))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }
    }
}
