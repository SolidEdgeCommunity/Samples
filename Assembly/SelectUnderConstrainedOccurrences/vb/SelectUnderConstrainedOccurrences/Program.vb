Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace SelectUnderConstrainedOccurrences
	Friend Class Program
		<STAThread>
		Shared Sub Main(ByVal args() As String)
			Dim application As SolidEdgeFramework.Application = Nothing
			Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect()

				' Get a reference to the active selectset.
				selectSet = application.ActiveSelectSet

				' Temporarily suspend selectset UI updates.
				selectSet.SuspendDisplay()

				' Clear the selectset.
				selectSet.RemoveAll()

				' Get a reference to the active document.
				assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

				If assemblyDocument IsNot Nothing Then
					' Get a reference to the occurrences collection.
					occurrences = assemblyDocument.Occurrences

					' Loop through the occurrences.
					For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
						' If status equals seOccurrenceStatusUnderDefined, add to selectset.
						If occurrence.Status = SolidEdgeAssembly.OccurrenceStatusConstants.seOccurrenceStatusUnderDefined Then
							selectSet.Add(occurrence)
						End If
					Next occurrence
				Else
					Throw New System.Exception("No active document")
				End If

				' Re-enable selectset UI display.
				selectSet.ResumeDisplay()

				' Manually refresh the selectset UI display.
				selectSet.RefreshDisplay()
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
