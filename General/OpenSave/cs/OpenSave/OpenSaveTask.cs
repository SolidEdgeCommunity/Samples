using log4net;
using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdge.OpenSave
{
    public class OpenSaveTask : IsolatedTaskProxy
    {
        private static ILog _log = LogManager.GetLogger(typeof(OpenSaveTask));

        public void DoOpenSave(string filePath, OpenSaveSettings openSaveSettings)
        {
            InvokeSTAThread<string, OpenSaveSettings>(DoOpenSaveInternal, filePath, openSaveSettings);
        }

        void DoOpenSaveInternal(string filePath, OpenSaveSettings openSaveSettings)
        {
            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();

            try
            {
                SolidEdgeFramework.Application application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true);
                SolidEdgeFramework.Documents documents = null;
                SolidEdgeFramework.SolidEdgeDocument document = null;

                application.DisplayAlerts = openSaveSettings.Application.DisplayAlerts;
                application.Visible = openSaveSettings.Application.Visible;

                if (openSaveSettings.Application.DisableAddins == true)
                {
                    DisableAddins(application);
                }

                // Disable (most) prompts.
                application.DisplayAlerts = false;

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Close any documents that may be left open for whatever reason.
                documents.Close();

                // Open the file.
                document = documents.Open<SolidEdgeFramework.SolidEdgeDocument>(filePath);

                application.DoIdle();

                if (document != null)
                {
                    // Environment specific routines.
                    if (document is SolidEdgeAssembly.AssemblyDocument)
                    {
                        DoOpenSave((SolidEdgeAssembly.AssemblyDocument)document, openSaveSettings.Assembly);
                    }
                    else if (document is SolidEdgeDraft.DraftDocument)
                    {
                        DoOpenSave((SolidEdgeDraft.DraftDocument)document, openSaveSettings.Draft);
                    }
                    else if (document is SolidEdgePart.PartDocument)
                    {
                        DoOpenSave((SolidEdgePart.PartDocument)document, openSaveSettings.Part);
                    }
                    else if (document is SolidEdgePart.SheetMetalDocument)
                    {
                        DoOpenSave((SolidEdgePart.SheetMetalDocument)document, openSaveSettings.SheetMetal);
                    }
                    else if (document is SolidEdgePart.WeldmentDocument)
                    {
                        DoOpenSave((SolidEdgePart.WeldmentDocument)document, openSaveSettings.Weldment);
                    }

                    // Save document.
                    document.Save();

                    // Close document.
                    document.Close();

                    application.DoIdle();
                }
            }
            finally
            {
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }

        void DoOpenSave(SolidEdgeAssembly.AssemblyDocument assemblyDocument, AssemblySettings assemblySettings)
        {
        }

        void DoOpenSave(SolidEdgeDraft.DraftDocument draftDocument, DraftSettings draftSettings)
        {
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.Section section = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;
            SolidEdgeDraft.DrawingView drawingView = null;

            if (draftSettings.UpdateDrawingViews)
            {
                sections = draftDocument.Sections;
                section = sections.WorkingSection;

                sectionSheets = section.Sheets;

                for (int i = 1; i <= sectionSheets.Count; i++)
                {
                    sheet = sectionSheets.Item(i);
                    drawingViews = sheet.DrawingViews;

                    for (int j = 1; j <= drawingViews.Count; j++)
                    {
                        drawingView = drawingViews.Item(j);
                        drawingView.Update();
                    }
                }
            }
        }

        void DoOpenSave(SolidEdgePart.PartDocument partDocument, PartSettings partSettings)
        {
        }

        void DoOpenSave(SolidEdgePart.SheetMetalDocument sheetMetalDocument, SheetMetalSettings sheetMetalSettings)
        {
        }

        void DoOpenSave(SolidEdgePart.WeldmentDocument weldmentDocument, WeldmentSettings weldmentSettings)
        {
        }

        void DisableAddins(SolidEdgeFramework.Application application)
        {
            SolidEdgeFramework.AddIns addins = null;
            SolidEdgeFramework.AddIn addin = null;

            addins = application.AddIns;

            for (int i = 1; i <= addins.Count; i++)
            {
                addin = addins.Item(i);
                addin.Connect = false;
            }
        }
    }
}
