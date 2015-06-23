using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenClose
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Connect to Solid Edge.
            var application = SolidEdgeUtils.Connect(true, true);

            // Get a reference to the Documents collection.
            var documents = application.Documents;

            // Get a folder that has Solid Edge files.
            var folder = new DirectoryInfo(SolidEdgeUtils.GetTrainingFolderPath());

            // Get the installed version of Solid Edge.
            var solidEdgeVesion = application.GetVersion();

            // Disable prompts.
            application.DisplayAlerts = false;

            // Process the files.
            foreach (var file in folder.EnumerateFiles("*.par", SearchOption.AllDirectories))
            {
                Console.WriteLine(file.FullName);

                // Open the document.
                var document = (SolidEdgeFramework.SolidEdgeDocument)documents.Open(file.FullName);

                // Give Solid Edge a chance to do processing.
                application.DoIdle();

                // Prior to ST8, we needed a reference to a document to close it.
                // That meant that SE can't fully close the document because we're holding a reference.
                if (solidEdgeVesion.Major < 108)
                {
                    // Close the document.
                    document.Close();

                    // Release our reference to the document.
                    Marshal.FinalReleaseComObject(document);
                    document = null;

                    // Give SE a chance to do post processing (finish closing the document).
                    application.DoIdle();
                }
                else
                {
                    // Release our reference to the document.
                    Marshal.FinalReleaseComObject(document);
                    document = null;

                    // Starting with ST8, the Documents collection has a CloseDocument() method.
                    documents.CloseDocument(file.FullName, false, Missing.Value, Missing.Value, true);
                }
            }

            application.DisplayAlerts = true;

            // Additional cleanup.
            Marshal.FinalReleaseComObject(documents);
            Marshal.FinalReleaseComObject(application);
        }
    }
}
