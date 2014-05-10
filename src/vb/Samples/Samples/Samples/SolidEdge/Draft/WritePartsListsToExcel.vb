Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Draft
	''' <summary>
	''' Writes the 1st parts list of the active draft into Excel.
	''' </summary>
	Friend Class WritePartsListsToExcel
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim partsLists As SolidEdgeDraft.PartsLists = Nothing
			Dim partsList As SolidEdgeDraft.PartsList = Nothing
			Dim excelApplication As Microsoft.Office.Interop.Excel.Application = Nothing
			Dim excelWorkbooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
			Dim excelWorkbook As Microsoft.Office.Interop.Excel.Workbook = Nothing
			Dim excelWorksheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
			Dim excelCells As Microsoft.Office.Interop.Excel.Range = Nothing
			Dim excelRange As Microsoft.Office.Interop.Excel.Range = Nothing
			Dim tableColumns As SolidEdgeDraft.TableColumns = Nothing
			Dim tableColumn As SolidEdgeDraft.TableColumn = Nothing
			Dim tableRows As SolidEdgeDraft.TableRows = Nothing
			Dim tableRow As SolidEdgeDraft.TableRow = Nothing
			Dim tableCell As SolidEdgeDraft.TableCell = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(False)

				' Get a reference to the active draft document.
				draftDocument = application.TryActiveDocumentAs(Of SolidEdgeDraft.DraftDocument)()

				If draftDocument IsNot Nothing Then
					' Get a reference to the PartsLists collection.
					partsLists = draftDocument.PartsLists

					If partsLists.Count > 0 Then
						' Get a reference to the 1st parts list.
						partsList = partsLists.Item(1)

						' Connect to or start Excel.
						Try
							excelApplication = DirectCast(Marshal.GetActiveObject("Excel.Application"), Microsoft.Office.Interop.Excel.Application)
						Catch
							excelApplication = DirectCast(Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application")), Microsoft.Office.Interop.Excel.Application)
						End Try

						If excelApplication IsNot Nothing Then
							excelApplication.Visible = True
							excelWorkbooks = excelApplication.Workbooks
							excelWorkbook = excelWorkbooks.Add()
							excelWorksheet = DirectCast(excelWorkbook.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)

							' Get a reference to the Columns collection.
							tableColumns = partsList.Columns

							' Get a reference to the Rows collection.
							tableRows = partsList.Rows

							Dim visibleColumnCount As Integer = 0
							Dim visibleRowCount As Integer = 0

							' Get a reference to the Cells collection.
							excelCells = excelWorksheet.Cells

							' Write headers.
							For i As Integer = 1 To tableColumns.Count
								tableColumn = tableColumns.Item(i)

								If tableColumn.Show Then
									visibleColumnCount += 1
									excelRange = DirectCast(excelCells.Item(1, visibleColumnCount), Microsoft.Office.Interop.Excel.Range)
									excelRange.Value = tableColumn.HeaderRowValue
								End If
							Next i

							' Write rows.
							For i As Integer = 1 To tableRows.Count
								tableRow = tableRows.Item(i)
								For j As Integer = 1 To tableColumns.Count
									tableColumn = tableColumns.Item(j)

									If tableColumn.Show Then
										visibleRowCount += 1
										tableCell = partsList.Cell(i, j)
										excelRange = DirectCast(excelCells.Item(i + 1, j), Microsoft.Office.Interop.Excel.Range)
										excelRange.Value = tableCell.value
									End If
								Next j

								visibleRowCount = 0
							Next i
						End If
					Else
						Throw New System.Exception(Resources.NoPartsListsInDraftDocument)
					End If
				Else
					Throw New System.Exception(Resources.NoActiveDraftDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
