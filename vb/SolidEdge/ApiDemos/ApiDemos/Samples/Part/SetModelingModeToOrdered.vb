Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Part
	''' <summary>
	''' Sets the ModelingMode of the active part to ordered.
	''' </summary>
	Friend Class SetModelingModeToOrdered
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active sheetmetal document.
				partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

				If partDocument IsNot Nothing Then
					partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous
				Else
					Throw New System.Exception(Resources.NoActivePartDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
