Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Part
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
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active part document.
				partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

				If partDocument IsNot Nothing Then
					models = partDocument.Models

					If models.Count = 0 Then
						Throw New System.Exception(My.Resources.NoGeometryDefined)
					End If

					model = models.Item(1)
					body = DirectCast(model.Body, SolidEdgeGeometry.Body)

					Dim MinRangePoint As Array = Array.CreateInstance(GetType(Double), 0)
					Dim MaxRangePoint As Array = Array.CreateInstance(GetType(Double), 0)

					body.GetRange(MinRangePoint, MaxRangePoint)

				Else
					Throw New System.Exception(My.Resources.NoActivePartDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
