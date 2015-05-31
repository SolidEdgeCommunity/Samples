using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WritePartsListsToExcel
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.PartsLists partsLists = null;
            SolidEdgeDraft.PartsList partsList = null;
            dynamic excelApplication = null;
            dynamic excelWorkbooks = null;
            dynamic excelWorkbook = null;
            dynamic excelWorksheet = null;
            dynamic excelCells = null;
            dynamic excelRange = null;
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
                            excelApplication = Marshal.GetActiveObject("Excel.Application");
                        }
                        catch
                        {
                            excelApplication = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));
                        }

                        if (excelApplication != null)
                        {
                            excelApplication.Visible = true;
                            excelWorkbooks = excelApplication.Workbooks;
                            excelWorkbook = excelWorkbooks.Add();
                            excelWorksheet = excelWorkbook.ActiveSheet;

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
                                    excelRange = excelCells.Item[1, visibleColumnCount];
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
                                        excelRange = excelCells.Item[tableRow.Index + 1, tableColumn.Index];
                                        excelRange.Value = tableCell.value;
                                    }
                                }

                                visibleRowCount = 0;
                            }
                        }
                    }
                    else
                    {
                        throw new System.Exception("No parts lists.");
                    }
                }
                else
                {
                    throw new System.Exception("No active document.");
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
