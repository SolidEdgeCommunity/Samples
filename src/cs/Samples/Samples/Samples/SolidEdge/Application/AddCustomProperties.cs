using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge
{
    /// <summary>
    /// Creates an new document and adds multiple custom file properties.
    /// </summary>
    class AddCustomProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeFramework.SolidEdgeDocument document = null;
            SolidEdgeFramework.PropertySets propertySets = null;
            SolidEdgeFramework.Properties properties = null;
            SolidEdgeFramework.Property property = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the Documents collection.
                documents = application.Documents;

                // Add a new part document.
                document = (SolidEdgeFramework.SolidEdgeDocument)documents.AddPartDocument();

                // Get a reference to the Properties collection.
                propertySets = (SolidEdgeFramework.PropertySets)document.Properties;

                // Get a reference to the custom proprty set.
                properties = propertySets.Item("Custom");

                // Add string custom property.
                property = properties.Add("My String", "Hello world!");

                // Add integer custom property.
                property = properties.Add("My Integer", 338);

                // Add boolean custom property.
                property = properties.Add("My Boolean", true);

                // Add date custom property.
                property = properties.Add("My DateTime", DateTime.Now);

                // Show the file properties dialog.
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartFileProperties);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                OleMessageFilter.Unregister();
            }
        }
    }
}
