using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Writes the 1st parts list of the active draft into Excel.
    /// </summary>
    class WritePartsListsToExcel
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.PartsLists partsLists = null;
            SolidEdgeDraft.PartsList partsList = null;
            Microsoft.Office.Interop.Excel.Application excelApplication = null;
            Microsoft.Office.Interop.Excel.Workbooks excelWorkbooks = null;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook = null;
            Microsoft.Office.Interop.Excel.Worksheet excelWorksheet = null;
            Microsoft.Office.Interop.Excel.Range excelCells = null;
            Microsoft.Office.Interop.Excel.Range excelRange = null;
            SolidEdgeDraft.TableColumns tableColumns = null;
            SolidEdgeDraft.TableColumn tableColumn = null;
            SolidEdgeDraft.TableRows tableRows = null;
            SolidEdgeDraft.TableRow tableRow = null;
            SolidEdgeDraft.TableCell tableCell = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(false);

                // Get a reference to the active draft document.
                draftDocument = application.TryActiveDocumentAs<SolidEdgeDraft.DraftDocument>();

                if (draftDocument != null)
                {
                    // Get a reference to the PartsLists collection.
                    partsLists = draftDocument.PartsLists;

                    if (partsLists.Count > 0)
                    {
                        // Get a reference to the 1st parts list.
                        partsList = partsLists.Item(1);

                        // Connect to or start Excel.
                        try
                        {
                            excelApplication = (Microsoft.Office.Interop.Excel.Application)Marshal.GetActiveObject("Excel.Application");
                        }
                        catch
                        {
                            excelApplication = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                        }

                        if (excelApplication != null)
                        {
                            excelApplication.Visible = true;
                            excelWorkbooks = excelApplication.Workbooks;
                            excelWorkbook = excelWorkbooks.Add();
                            excelWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)excelWorkbook.ActiveSheet;

                            // Get a reference to the Columns collection.
                            tableColumns = partsList.Columns;

                            // Get a reference to the Rows collection.
                            tableRows = partsList.Rows;

                            int visibleColumnCount = 0;
                            int visibleRowCount = 0;

                            // Get a reference to the Cells collection.
                            excelCells = excelWorksheet.Cells;

                            // Write headers.
                            for (int i = 1; i <= tableColumns.Count; i++)
                            {
                                tableColumn = tableColumns.Item(i);

                                if (tableColumn.Show)
                                {
                                    visibleColumnCount++;
                                    excelRange = (Microsoft.Office.Interop.Excel.Range)excelCells.Item[1, visibleColumnCount];
                                    excelRange.Value = tableColumn.HeaderRowValue;
                                }
                            }

                            // Write rows.
                            for (int i = 1; i <= tableRows.Count; i++)
                            {
                                tableRow = tableRows.Item(i);
                                for (int j = 1; j <= tableColumns.Count; j++)
                                {
                                    tableColumn = tableColumns.Item(j);

                                    if (tableColumn.Show)
                                    {
                                        visibleRowCount++;
                                        tableCell = partsList.Cell[i, j];
                                        excelRange = (Microsoft.Office.Interop.Excel.Range)excelCells.Item[i + 1, j];
                                        excelRange.Value = tableCell.value;
                                    }
                                }

                                visibleRowCount = 0;
                            }
                        }
                    }
                    else
                    {
                        throw new System.Exception(Resources.NoPartsListsInDraftDocument);
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveDraftDocument);
                }
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
