Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Part
	''' <summary>
	''' Reports the body range information of the active part.
	''' </summary>
	Friend Class ReportBodyRange
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim models As SolidEdgePart.Models = Nothing
			Dim model As SolidEdgePart.Model = Nothing
			Dim body As SolidEdgeGeometry.Body = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True)

				' Make sure user can see the GUI.
				application.Visible = True

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active part document.
				partDocument = application.TryActiveDocumentAs(Of SolidEdgePart.PartDocument)()

				If partDocument IsNot Nothing Then
					models = partDocument.Models

					If models.Count = 0 Then
						Throw New System.Exception(Resources.NoGeometryDefined)
					End If

					model = models.Item(1)
					body = DirectCast(model.Body, SolidEdgeGeometry.Body)

					Dim MinRangePoint As Array = Array.CreateInstance(GetType(Double), 0)
					Dim MaxRangePoint As Array = Array.CreateInstance(GetType(Double), 0)

					body.GetRange(MinRangePoint, MaxRangePoint)

				Else
					Throw New System.Exception(Resources.NoActivePartDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
