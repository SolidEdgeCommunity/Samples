using log4net;
using log4net.Config;
using SolidEdge.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SolidEdge.OpenSave
{
    public class InteropProxy : MarshalByRefObject
    {
        private static ILog _log = LogManager.GetLogger(typeof(InteropProxy));
        private OpenSaveSettings _openSaveSettings;

        public void DoOpenSave(string filePath, OpenSaveSettings openSaveSettings)
        {
            _openSaveSettings = openSaveSettings;

            // Register with OLE to handle concurrency issues on the current thread.
            OleMessageFilter.Register();

            try
            {
                SolidEdgeFramework.Application application = ApplicationHelper.Connect(true);
                SolidEdgeFramework.Documents documents = null;
                SolidEdgeFramework.SolidEdgeDocument document = null;

                application.Visible = _openSaveSettings.Application.Visible;

                if (_openSaveSettings.Application.DisableAddins == true)
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
                document = (SolidEdgeFramework.SolidEdgeDocument)documents.Open(filePath);

                application.DoIdle();

                if (document != null)
                {
                    // Environment specific routines.
                    if (document is SolidEdgeAssembly.AssemblyDocument)
                    {
                        DoOpenSave((SolidEdgeAssembly.AssemblyDocument)document, _openSaveSettings.Assembly);
                    }
                    else if (document is SolidEdgeDraft.DraftDocument)
                    {
                        DoOpenSave((SolidEdgeDraft.DraftDocument)document, _openSaveSettings.Draft);
                    }
                    else if (document is SolidEdgePart.PartDocument)
                    {
                        DoOpenSave((SolidEdgePart.PartDocument)document, _openSaveSettings.Part);
                    }
                    else if (document is SolidEdgePart.SheetMetalDocument)
                    {
                        DoOpenSave((SolidEdgePart.SheetMetalDocument)document, _openSaveSettings.SheetMetal);
                    }
                    else if (document is SolidEdgePart.WeldmentDocument)
                    {
                        DoOpenSave((SolidEdgePart.WeldmentDocument)document, _openSaveSettings.Weldment);
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
                OleMessageFilter.Unregister();
            }
        }

        private void DoOpenSave(SolidEdgeAssembly.AssemblyDocument assemblyDocument, AssemblySettings assemblySettings)
        {
        }

        private void DoOpenSave(SolidEdgeDraft.DraftDocument draftDocument, DraftSettings draftSettings)
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

        private void DoOpenSave(SolidEdgePart.PartDocument partDocument, PartSettings partSettings)
        {
        }

        private void DoOpenSave(SolidEdgePart.SheetMetalDocument sheetMetalDocument, SheetMetalSettings sheetMetalSettings)
        {
        }

        private void DoOpenSave(SolidEdgePart.WeldmentDocument weldmentDocument, WeldmentSettings weldmentSettings)
        {
        }

        private void DisableAddins(SolidEdgeFramework.Application application)
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
