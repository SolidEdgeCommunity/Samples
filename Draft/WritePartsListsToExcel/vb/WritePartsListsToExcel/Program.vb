Option Strict Off
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim partsLists As SolidEdgeDraft.PartsLists = Nothing
        Dim partsList As SolidEdgeDraft.PartsList = Nothing
        Dim excelApplication As Object = Nothing
        Dim excelWorkbooks As Object = Nothing
        Dim excelWorkbook As Object = Nothing
        Dim excelWorksheet As Object = Nothing
        Dim excelCells As Object = Nothing
        Dim excelRange As Object = Nothing
        Dim tableColumns As SolidEdgeDraft.TableColumns = Nothing
        Dim tableRows As SolidEdgeDraft.TableRows = Nothing
        Dim tableCell As SolidEdgeDraft.TableCell = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            ' Get a reference to the active draft document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            If draftDocument IsNot Nothing Then
                ' Get a reference to the PartsLists collection.
                partsLists = draftDocument.PartsLists

                If partsLists.Count > 0 Then
                    ' Get a reference to the 1st parts list.
                    partsList = partsLists.Item(1)

                    ' Connect to or start Excel.
                    Try
                        excelApplication = Marshal.GetActiveObject("Excel.Application")
                    Catch
                        excelApplication = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"))
                    End Try

                    If excelApplication IsNot Nothing Then
                        excelApplication.Visible = True
                        excelWorkbooks = excelApplication.Workbooks
                        excelWorkbook = excelWorkbooks.Add()
                        excelWorksheet = excelWorkbook.ActiveSheet

                        ' Get a reference to the Columns collection.
                        tableColumns = partsList.Columns

                        ' Get a reference to the Rows collection.
                        tableRows = partsList.Rows

                        Dim visibleColumnCount As Integer = 0
                        Dim visibleRowCount As Integer = 0

                        ' Get a reference to the Cells collection.
                        excelCells = excelWorksheet.Cells

                        ' Write headers.
                        For Each tableColumn In tableColumns.OfType(Of SolidEdgeDraft.TableColumn)()
                            If tableColumn.Show Then
                                visibleColumnCount += 1
                                excelRange = excelCells.Item(1, visibleColumnCount)
                                excelRange.Value = tableColumn.HeaderRowValue
                            End If
                        Next tableColumn

                        ' Write rows.
                        For Each tableRow In tableRows.OfType(Of SolidEdgeDraft.TableRow)()
                            For Each tableColumn In tableColumns.OfType(Of SolidEdgeDraft.TableColumn)()
                                If tableColumn.Show Then
                                    visibleRowCount += 1
                                    tableCell = partsList.Cell(tableRow.Index, tableColumn.Index)
                                    excelRange = excelCells.Item(tableRow.Index + 1, tableColumn.Index)
                                    excelRange.Value = tableCell.value
                                End If
                            Next tableColumn

                            visibleRowCount = 0
                        Next tableRow
                    End If
                Else
                    Throw New System.Exception("No parts lists.")
                End If
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
