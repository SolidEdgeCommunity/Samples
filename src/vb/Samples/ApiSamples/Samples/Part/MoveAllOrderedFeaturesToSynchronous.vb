Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Part
	''' <summary>
	''' Moves all existing ordered features of the active part to synchronous.
	''' </summary>
	Friend Class MoveAllOrderedFeaturesToSynchronous
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim models As SolidEdgePart.Models = Nothing
			Dim model As SolidEdgePart.Model = Nothing
			Dim features As SolidEdgePart.Features = Nothing
			Dim bIgnoreWarnings As Boolean = True
			Dim bExtendSelection As Boolean = True

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
					' Get a reference to the Models collection.
					models = partDocument.Models

					' Get a reference to the 1st model.
					model = models.Item(1)

					' Get a reference to the Features collection.
					features = model.Features

					' Iterate through the features.
					For Each feature As Object In features
						Dim featureEdgeBarName As String = ReflectionHelper.GetPropertyValueAsString(feature, "EdgeBarName")

						Dim faceRotate As SolidEdgePart.FaceRotate = TryCast(feature, SolidEdgePart.FaceRotate)

						' Check to see if the feature is ordered.
						' NOTE: I've found that not all features have a ModelingModeType property. SolidEdgePart.FaceRotate is one of them.
						' This is a bit of a problem because I see no way to know if the FaceRotate is Ordered or Synchronous...
						If ReflectionHelper.GetPartFeatureModelingMode(feature) = SolidEdgePart.ModelingModeConstants.seModelingModeOrdered Then
							Dim NumberOfFeaturesCausingError As Integer = 0
							Dim ErrorMessageArray As Array = Array.CreateInstance(GetType(String), 0)
							Dim NumberOfFeaturesCausingWarning As Integer = 0
							Dim WarningMessageArray As Array = Array.CreateInstance(GetType(String), 0)
							Dim VolumeDifference As Double = 0.0

							' Move the ordered feature to synchronous.
							partDocument.MoveToSynchronous(pFeatureUnk:= feature, bIgnoreWarnings:= bIgnoreWarnings, bExtendSelection:= bExtendSelection, NumberOfFeaturesCausingError:= NumberOfFeaturesCausingError, ErrorMessageArray:= ErrorMessageArray, NumberOfFeaturesCausingWarning:= NumberOfFeaturesCausingWarning, WarningMessageArray:= WarningMessageArray, VolumeDifference:= VolumeDifference)

							Console.WriteLine("Feature '{0}' results:", featureEdgeBarName)
							Console.WriteLine("VolumeDifference: '{0}'", VolumeDifference)

							' Process error messages.
							For i As Integer = 0 To ErrorMessageArray.Length - 1
								Console.WriteLine("Error: '{0}'.", ErrorMessageArray.GetValue(i))
							Next i

							' Process warning messages.
							For i As Integer = 0 To WarningMessageArray.Length - 1
								Console.WriteLine("Warning: '{0}'.", WarningMessageArray.GetValue(i))
							Next i

							' If you get any error or warning messages, it's probably a good idea to stop.
							If (ErrorMessageArray.Length > 0) OrElse (WarningMessageArray.Length > 0) Then
								Exit For
							Else
								Console.WriteLine("Success")
							End If
						End If
					Next feature
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
