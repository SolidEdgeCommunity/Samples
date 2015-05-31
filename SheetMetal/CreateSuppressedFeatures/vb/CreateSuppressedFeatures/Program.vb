Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateSuppressedFeatures
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
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
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

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

                    ' Use helper class to get the feature type.
                    Dim featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgePart.FeatureTypeConstants)(edgebarFeature, "Type", CType(0, SolidEdgePart.FeatureTypeConstants))

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
