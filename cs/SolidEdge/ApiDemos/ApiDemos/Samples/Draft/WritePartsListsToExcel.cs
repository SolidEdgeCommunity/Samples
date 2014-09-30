using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ApiDemos.Draft
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
            SolidEdgeDraft.TableRows tableRows = null;
            SolidEdgeDraft.TableCell tableCell = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(false);

                // Get a reference to the active draft document.
                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

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
                            foreach (var tableColumn in tableColumns.OfType<SolidEdgeDraft.TableColumn>())
                            {
                                if (tableColumn.Show)
                                {
                                    visibleColumnCount++;
                                    excelRange = (Microsoft.Office.Interop.Excel.Range)excelCells.Item[1, visibleColumnCount];
                                    excelRange.Value = tableColumn.HeaderRowValue;
                                }
                            }

                            // Write rows.
                            foreach (var tableRow in tableRows.OfType<SolidEdgeDraft.TableRow>())
                            {
                                foreach (var tableColumn in tableColumns.OfType<SolidEdgeDraft.TableColumn>())
                                {
                                    if (tableColumn.Show)
                                    {
                                        visibleRowCount++;
                                        tableCell = partsList.Cell[tableRow.Index, tableColumn.Index];
                                        excelRange = (Microsoft.Office.Interop.Excel.Range)excelCells.Item[tableRow.Index + 1, tableColumn.Index];
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
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
