using SolidEdgeCommunity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StressTest
{
    public class OpenCloseTask : IsolatedTaskProxy
    {
        public void DoOpenClose(string fileName)
        {
            InvokeSTAThread<string>(DoOpenCloseInternal, fileName);
        }

        void DoOpenCloseInternal(string fileName)
        {
            //SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeFramework.SolidEdgeDocument document = null;

            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();

            try
            {
                // Get reference to application object.
                var application = this.Application;

                // Get reference to documents collection.
                documents = application.Documents;

                // Open the document.
                document = (SolidEdgeFramework.SolidEdgeDocument)documents.Open(fileName);

                // Not sure why but as of ST8, I had to add a Sleep() call to prevent crashing...
                System.Threading.Thread.Sleep(500);

                // Do idle processing.
                application.DoIdle();

                // Close the document.
                document.Close(false);

                //System.Threading.Thread.Sleep(500);

                // Do idle processing.
                application.DoIdle();

                documents.Close();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
