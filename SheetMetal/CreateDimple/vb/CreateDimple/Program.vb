Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateDimple
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
            Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
            Dim refPlane As SolidEdgePart.RefPlane = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim profileSets As SolidEdgePart.ProfileSets = Nothing
            Dim profileSet As SolidEdgePart.ProfileSet = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim profile As SolidEdgePart.Profile = Nothing
            Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
            Dim line2d As SolidEdgeFrameworkSupport.Line2d = Nothing
            Dim dimple As SolidEdgePart.Dimple = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the Documents collection.
                documents = application.Documents

                ' Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument()

                ' Always a good idea to give SE a chance to breathe.
                application.DoIdle()

                ' Call helper method to create the actual geometry.
                model = SheetMetalHelper.CreateBaseTabByCircle(sheetMetalDocument)

                ' Get a reference to the RefPlanes collection.
                refPlanes = sheetMetalDocument.RefPlanes

                ' Get a reference to front RefPlane.
                refPlane = refPlanes.GetFrontPlane()

                ' Get a reference to the ProfileSets collection.
                profileSets = sheetMetalDocument.ProfileSets

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                ' Add new Profile.
                profile = profiles.Add(refPlane)

                ' Draw a line to define the dimple point.
                lines2d = profile.Lines2d
                line2d = lines2d.AddBy2Points(0, 0, 0.01, 0)

                ' Hide the profile.
                profile.Visible = False

                Dim depth As Double = 0.01

                ' Add new dimple.
                dimple = model.Dimples.Add(Profile:= profile, Depth:= depth, ProfileSide:= SolidEdgePart.DimpleFeatureConstants.seDimpleDepthLeft, DepthSide:= SolidEdgePart.DimpleFeatureConstants.seDimpleDepthRight)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new Dimple to ActiveSelectSet.
                selectSet.Add(dimple)

                ' Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView)
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
