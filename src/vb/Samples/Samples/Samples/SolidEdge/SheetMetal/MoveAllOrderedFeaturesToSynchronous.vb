Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.SheetMetal
	''' <summary>
	''' Moves all existing ordered features of the active sheetmetal to synchronous.
	''' </summary>
	Friend Class MoveAllOrderedFeaturesToSynchronous
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
			Dim models As SolidEdgePart.Models = Nothing
			Dim model As SolidEdgePart.Model = Nothing
			Dim features As SolidEdgePart.Features = Nothing
			Dim bIgnoreWarnings As Boolean = True
			Dim bExtendSelection As Boolean = True

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
				sheetMetalDocument = application.TryActiveDocumentAs(Of SolidEdgePart.SheetMetalDocument)()

				If sheetMetalDocument IsNot Nothing Then
					' Get a reference to the Models collection.
					models = sheetMetalDocument.Models

					' Get a reference to the 1st model.
					model = models.Item(1)

					' Get a reference to the Features collection.
					features = model.Features

					' Iterate through the features.
					For Each feature As Object In features
						Dim featureEdgeBarName As String = ReflectionHelper.GetPropertyValueAsString(feature, "EdgeBarName")

						' Check to see if the feature is ordered.
						If ReflectionHelper.GetPartFeatureModelingMode(feature) = SolidEdgePart.ModelingModeConstants.seModelingModeOrdered Then
							Dim NumberOfFeaturesCausingError As Integer = 0
							Dim ErrorMessageArray As Array = Array.CreateInstance(GetType(String), 0)
							Dim NumberOfFeaturesCausingWarning As Integer = 0
							Dim WarningMessageArray As Array = Array.CreateInstance(GetType(String), 0)

							' Move the ordered feature to synchronous.
							sheetMetalDocument.MoveToSynchronous(pFeatureUnk:= feature, bIgnoreWarnings:= bIgnoreWarnings, bExtendSelection:= bExtendSelection, NumberOfFeaturesCausingError:= NumberOfFeaturesCausingError, ErrorMessageArray:= ErrorMessageArray, NumberOfFeaturesCausingWarning:= NumberOfFeaturesCausingWarning, WarningMessageArray:= WarningMessageArray)

							Console.WriteLine("Feature '{0}' results:", featureEdgeBarName)

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
					Throw New System.Exception(Resources.NoActiveSheetMetalDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
