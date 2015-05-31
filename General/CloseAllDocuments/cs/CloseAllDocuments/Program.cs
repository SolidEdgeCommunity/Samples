using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloseAllDocuments
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            var bSilent = false;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(false);

                if (application != null)
                {
                    if (bSilent)
                    {
                        // Disable alerts. This will prevent the Save dialog from showing.
                        application.DisplayAlerts = false;
                    }

                    // Get a reference to the documents collection.
                    documents = application.Documents;

                    // Close all documents.
                    documents.Close();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (application != null)
                {
                    // Re-enable alerts.
                    application.DisplayAlerts = true;
                }

                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
