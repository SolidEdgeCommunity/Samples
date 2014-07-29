Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.SheetMetal
	''' <summary>
	''' Creates a new sheetmetal with a family member containing suppressed features.
	''' </summary>
	Friend Class CreateFamilyMemberWithSuppressedFeature
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
			Dim familyMembers As SolidEdgePart.FamilyMembers = Nothing
			Dim familyMember As SolidEdgePart.FamilyMember = Nothing
			Dim edgebarFeatures As SolidEdgePart.EdgebarFeatures = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				' Add a new sheetmetal document.
				sheetMetalDocument = documents.AddSheetMetalDocument()

				' Invoke existing sample to create geometry.
				SheetMetalHelper.CreateHolesWithUserDefinedPattern(sheetMetalDocument)

				' Get a reference to the FamilyMembers collection.
				familyMembers = sheetMetalDocument.FamilyMembers

				' Add a new FamilyMember.
				familyMember = familyMembers.Add("Member 1")

				' Get a reference to the DesignEdgebarFeatures collection.
				edgebarFeatures = sheetMetalDocument.DesignEdgebarFeatures

				' Iterate through the DesignEdgebarFeatures.
				For i As Integer = 1 To edgebarFeatures.Count
					' Get the EdgebarFeature at the current index.
					Dim edgebarFeature As Object = edgebarFeatures.Item(i)

					' Use ReflectionHelper class to get the feature type.
					Dim featureType As SolidEdgePart.FeatureTypeConstants = ReflectionHelper.GetPartFeatureType(edgebarFeature)

					' Looking for a Hole pattern to suppress.
					If featureType = SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject Then
						' Suppress the feature.
						familyMember.AddSuppressedFeature(edgebarFeature)
					End If
				Next i

				' Apply the FamilyMember.
				familyMember.Apply()
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
