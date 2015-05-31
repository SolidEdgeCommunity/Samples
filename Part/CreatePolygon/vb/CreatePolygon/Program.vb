Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim partDocument As SolidEdgePart.PartDocument = Nothing
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

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

            ' Get a reference to the ProfileSets collection.
            profileSets = partDocument.ProfileSets

            ' Add a new ProfileSet.
            profileSet = profileSets.Add()

            ' Get a reference to the Profiles collection.
            profiles = profileSet.Profiles

            ' Add a new Profile.
            profile = profiles.Add(refPlane)

            ' Get a reference to the Relations2d collection.
            relations2d = DirectCast(profile.Relations2d, SolidEdgeFrameworkSupport.Relations2d)

            ' Get a reference to the Lines2d collection.
            lines2d = profile.Lines2d

            Dim sides As Integer = 8
            Dim angle As Double = 360 \ sides
            angle = (angle * Math.PI) / 180

            Dim radius As Double =.05
            Dim lineLength As Double = 2 * radius * (Math.Tan(angle) / 2)

            ' x1, y1, x2, y2
            Dim points() As Double = { 0.0, 0.0, 0.0, 0.0 }

            Dim x As Double = 0.0
            Dim y As Double = 0.0

            points(2) = -((Math.Cos(angle / 2) * radius) - x)
            points(3) = -((lineLength / 2) - y)

            ' Draw each line.
            For i As Integer = 0 To sides - 1
                points(0) = points(2)
                points(1) = points(3)
                points(2) = points(0) + (Math.Sin(angle * i) * lineLength)
                points(3) = points(1) + (Math.Cos(angle * i) * lineLength)

                lines2d.AddBy2Points(points(0), points(1), points(2), points(3))
            Next i

            ' Create endpoint relationships.
            For i As Integer = 1 To lines2d.Count
                If i = lines2d.Count Then
                    relations2d.AddKeypoint(lines2d.Item(i), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                Else
                    relations2d.AddKeypoint(lines2d.Item(i), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(i + 1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                    relations2d.AddEqual(lines2d.Item(i), lines2d.Item(i + 1))
                End If
            Next i

            ' Get a reference to the ActiveSelectSet.
            selectSet = application.ActiveSelectSet

            ' Empty ActiveSelectSet.
            selectSet.RemoveAll()

            ' Add all lines to ActiveSelectSet.
            For i As Integer = 1 To lines2d.Count
                selectSet.Add(lines2d.Item(i))
            Next i

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
