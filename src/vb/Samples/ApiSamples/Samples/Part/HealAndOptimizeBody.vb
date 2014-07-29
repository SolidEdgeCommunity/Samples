Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Part
	''' <summary>
	''' Heals and optimizes all models in the active part.
	''' </summary>
	Friend Class HealAndOptimizeBody
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim models As SolidEdgePart.Models = Nothing
			Dim model As SolidEdgePart.Model = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active part document.
				partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

				If partDocument IsNot Nothing Then
					models = partDocument.Models

					For i As Integer = 1 To models.Count
						model = models.Item(i)
						model.HealAndOptimizeBody(True, True)
					Next i
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
