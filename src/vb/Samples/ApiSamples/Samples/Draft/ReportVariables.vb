Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Draft
	''' <summary>
	''' Reports the variables of the active draft.
	''' </summary>
	Friend Class ReportVariables
		Friend Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the active document.
				draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

				' Make sure we have a document.
				If draftDocument IsNot Nothing Then
					VariablesHelper.ReportVariables(draftDocument)
				Else
					Throw New System.Exception(Resources.NoActiveDraftDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
