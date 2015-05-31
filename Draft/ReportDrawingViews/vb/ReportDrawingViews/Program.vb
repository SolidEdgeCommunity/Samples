Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim sections As SolidEdgeDraft.Sections = Nothing
        Dim section As SolidEdgeDraft.Section = Nothing
        Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
        Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
        Dim modelMembers As SolidEdgeDraft.ModelMembers = Nothing
        Dim graphicMembers As SolidEdgeDraft.GraphicMembers = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            If draftDocument IsNot Nothing Then
                ' Get a reference to the Sections collection.
                sections = draftDocument.Sections

                ' Get a reference to the WorkingSection.
                section = sections.WorkingSection

                ' Get a reference to the Sheets collection.
                sectionSheets = section.Sheets

                For Each sheet In sectionSheets.OfType(Of SolidEdgeDraft.Sheet)()
                    Console.WriteLine()
                    Console.WriteLine("Processing sheet '{0}'.", sheet.Name)

                    ' Get a reference to the DrawingViews collection.
                    drawingViews = sheet.DrawingViews

                    For Each drawingView In drawingViews.OfType(Of SolidEdgeDraft.DrawingView)()
                        Console.WriteLine()
                        Console.WriteLine("Processing drawing view '{0}'.", drawingView.Name)

                        Dim xOrigin As Double = 0
                        Dim yOrigin As Double = 0
                        drawingView.GetOrigin(xOrigin, yOrigin)

                        Console.WriteLine("Origin: x={0} y={1}.", xOrigin, yOrigin)

                        ' Get a reference to the ModelMembers collection.
                        modelMembers = drawingView.ModelMembers

                        For Each modelMember In modelMembers.OfType(Of SolidEdgeDraft.ModelMember)()
                            Console.WriteLine("Processing model member '{0}'.", modelMember.FileName)
                            Console.WriteLine("ComponentType: '{0}'.", modelMember.ComponentType)
                            Console.WriteLine("DisplayType: '{0}'.", modelMember.DisplayType)
                            Console.WriteLine("Type: '{0}'.", modelMember.Type)
                        Next modelMember

                        ' Get a reference to the ModelMembers collection.
                        graphicMembers = drawingView.GraphicMembers

                        ReportGraphicMembers(graphicMembers)
                    Next drawingView
                Next sheet
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Sub ReportGraphicMembers(ByVal graphicMembers As SolidEdgeDraft.GraphicMembers)
        ' This is a good example of how to deal with collections that contain different types.
        For Each graphicMember In graphicMembers.OfType(Of Object)()
            Console.WriteLine()
            Dim graphicMemberType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(graphicMember)

            Dim xKeypoint As Double = Nothing
            Dim yKeypoint As Double = Nothing
            Dim zKeypoint As Double = Nothing
            Dim keypointType As SolidEdgeFramework.KeyPointType = Nothing
            Dim handleType As Integer = Nothing

            If graphicMemberType.Equals(GetType(SolidEdgeDraft.DVLine2d)) Then
                Dim line2d = TryCast(graphicMember, SolidEdgeDraft.DVLine2d)
                Console.WriteLine("Processing graphic member '{0}' ({1}).", line2d.Name, graphicMemberType)
                Console.WriteLine("Angle: {0}.", line2d.Angle)
                Console.WriteLine("Length: {0}.", line2d.Length)

                For i As Integer = 1 To line2d.KeyPointCount
                    line2d.GetKeyPoint(i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                    Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                Next i
            ElseIf graphicMemberType.Equals(GetType(SolidEdgeDraft.DVArc2d)) Then
                Dim arc2d = TryCast(graphicMember, SolidEdgeDraft.DVArc2d)
                Console.WriteLine("Processing graphic member '{0}' ({1}).", arc2d.Name, graphicMemberType)
                Console.WriteLine("BendRadius: {0}.", arc2d.BendRadius)
                Console.WriteLine("Radius: {0}.", arc2d.Radius)
                Console.WriteLine("StartAngle: {0}.", arc2d.StartAngle)
                Console.WriteLine("SweepAngle: {0}.", arc2d.SweepAngle)

                For i As Integer = 1 To arc2d.KeyPointCount
                    arc2d.GetKeyPoint(i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                    Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                Next i
            ElseIf graphicMemberType.Equals(GetType(SolidEdgeDraft.DVCircle2d)) Then
                Dim circle2d = TryCast(graphicMember, SolidEdgeDraft.DVCircle2d)
                Console.WriteLine("Processing graphic member '{0}' ({1}).", circle2d.Name, graphicMemberType)
                Console.WriteLine("Area: {0}.", circle2d.Area)
                Console.WriteLine("BendAngle: {0}.", circle2d.BendAngle)
                Console.WriteLine("BendRadius: {0}.", circle2d.BendRadius)
                Console.WriteLine("Circumference: {0}.", circle2d.Circumference)
                Console.WriteLine("Diameter: {0}.", circle2d.Diameter)

                For i As Integer = 1 To circle2d.KeyPointCount
                    circle2d.GetKeyPoint(i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                    Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType)
                Next i
            End If
        Next graphicMember
    End Sub
End Class
