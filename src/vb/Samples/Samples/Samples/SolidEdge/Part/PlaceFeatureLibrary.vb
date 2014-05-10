Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Part
	''' <summary>
	''' Creates a new part and places features from an existing document.
	''' </summary>
	Friend Class PlaceFeatureLibrary
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
			Dim refPlane As SolidEdgePart.RefPlane = Nothing
			Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
			Dim features As Array = Array.CreateInstance(GetType(Object), 0)
			Dim extrudedProtrustion As SolidEdgePart.ExtrudedProtrusion = Nothing
			Dim round As SolidEdgePart.Round = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				' Create a new part document.
				partDocument = documents.AddPartDocument()

				' Always a good idea to give SE a chance to breathe.
				application.DoIdle()

				' Get a reference to the RefPlanes collection.
				refPlanes = partDocument.RefPlanes

				' Get a reference to the top RefPlane using extension method.
				refPlane = refPlanes.GetTopPlane()

				' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
				Dim trainingDirectory As New DirectoryInfo(InstallDataHelper.GetTrainingFolderPath())

				' Build path to source part document.
				Dim LibName As String = Path.Combine(trainingDirectory.FullName, "block.par")

				' This method will take all features from block.par and place them into the new part document.
				partDocument.PlaceFeatureLibrary(LibName, refPlane, 0.0, 0.0, 0.0, features)

				' Optionally, iterate through all of the added features.
				For Each feature As Object In features
					' Use ReflectionHelper class to get the feature type.
					Dim featureType As SolidEdgePart.FeatureTypeConstants = ReflectionHelper.GetPartFeatureType(feature)

					' Depending on the feature type, we can cast the weakly typed feature to a strongly typed feature.
					Select Case featureType
						Case SolidEdgePart.FeatureTypeConstants.igExtrudedProtrusionFeatureObject
							extrudedProtrustion = DirectCast(feature, SolidEdgePart.ExtrudedProtrusion)
						Case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject
							round = DirectCast(feature, SolidEdgePart.Round)
					End Select
				Next feature

				' Get a reference to the ActiveSelectSet.
				selectSet = application.ActiveSelectSet

				' Empty ActiveSelectSet.
				selectSet.RemoveAll()

				' Add all features to ActiveSelectSet.
				For Each feature As Object In features
					selectSet.Add(feature)
				Next feature

				' Switch to ISO view.
				application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
