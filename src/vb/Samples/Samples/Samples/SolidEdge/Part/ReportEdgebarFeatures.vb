Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Part
	''' <summary>
	''' Reports the edgebar features of the current part.
	''' </summary>
	Friend Class ReportEdgebarFeatures
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim edgebarFeatures As SolidEdgePart.EdgebarFeatures = Nothing

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
					' Get a reference to the DesignEdgebarFeatures collection.
					edgebarFeatures = partDocument.DesignEdgebarFeatures

					' Interate through the features.
					For i As Integer = 1 To edgebarFeatures.Count
						' Get the EdgebarFeature at current index.
						Dim edgebarFeature As Object = edgebarFeatures.Item(i)

						Dim type As Type = IDispatchHelper.GetManagedType(edgebarFeature)

						Console.WriteLine("Item({0}) is of type '{1}'", i, type)

					Next i
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
