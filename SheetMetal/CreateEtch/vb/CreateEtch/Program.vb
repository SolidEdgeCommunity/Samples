Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateEtch
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
            Dim models As SolidEdgePart.Models = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim sketchs As SolidEdgePart.Sketchs = Nothing
            Dim sketch As SolidEdgePart.Sketch = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim profile As SolidEdgePart.Profile = Nothing
            Dim etches As SolidEdgePart.Etches = Nothing
            Dim etch As SolidEdgePart.Etch = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the documents collection.
                documents = application.Documents

                ' Add a new sheet metal document.
                sheetMetalDocument = documents.AddSheetMetalDocument()

                ' Invoke existing sample to create geometry.
                SheetMetalHelper.CreateHolesWithUserDefinedPattern(sheetMetalDocument)

                ' Get a reference to the Sketches collection.
                sketchs = sheetMetalDocument.Sketches

                ' Get the 1st Sketch.
                sketch = sketchs.Item(1)

                ' Get a reference to the Profiles collection.
                profiles = sketch.Profiles

                ' Get the 1st Profile.
                profile = profiles.Item(1)

                ' Get a reference to the Models collection.
                models = sheetMetalDocument.Models

                ' Get the 1st Model.
                model = models.Item(1)

                ' Get a reference to the Etches collection.
                etches = model.Etches

                ' Add the Etch.
                etch = etches.Add(profile)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new Dimple to ActiveSelectSet.
                selectSet.Add(etch)

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
