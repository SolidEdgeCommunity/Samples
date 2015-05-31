Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect()

            ' Get a reference to the active select set.
            selectSet = application.ActiveSelectSet

            If selectSet.Count > 0 Then
                ' Loop through the items and report each one.
                For i As Integer = 1 To selectSet.Count
                    ' Get a reference to the item.
                    Dim item As Object = selectSet.Item(i)

                    ' Get the managed type.
                    Dim type = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(item)

                    Console.WriteLine("Item({0}) is of type '{1}'", i, type)

                    ReportItem(item, type)

                    Console.WriteLine()
                Next i
            Else
                Console.WriteLine("SelectSet is empty.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Sub ReportItem(ByVal item As Object, ByVal type As Type)
        ' Once we know the type, we can cast accordingly.
        ' Obviously not all types are handled here. Here are some examples.

        If type.Equals(GetType(SolidEdgeAssembly.Occurrence)) Then
            ReportItem(DirectCast(item, SolidEdgeAssembly.Occurrence))
        ElseIf type.Equals(GetType(SolidEdgeDraft.DrawingView)) Then
            ReportItem(DirectCast(item, SolidEdgeDraft.DrawingView))
        ElseIf type.Equals(GetType(SolidEdgePart.ExtrudedCutout)) Then
            ReportItem(DirectCast(item, SolidEdgePart.ExtrudedCutout))
        ElseIf type.Equals(GetType(SolidEdgePart.ExtrudedProtrusion)) Then
            ReportItem(DirectCast(item, SolidEdgePart.ExtrudedProtrusion))
        ElseIf type.Equals(GetType(SolidEdgePart.FeatureGroup)) Then
            ReportItem(DirectCast(item, SolidEdgePart.FeatureGroup))
        ElseIf type.Equals(GetType(SolidEdgePart.Flange)) Then
            ReportItem(DirectCast(item, SolidEdgePart.Flange))
        ElseIf type.Equals(GetType(SolidEdgePart.Hole)) Then
            ReportItem(DirectCast(item, SolidEdgePart.Hole))
        ElseIf type.Equals(GetType(SolidEdgePart.Round)) Then
            ReportItem(DirectCast(item, SolidEdgePart.Round))
        ElseIf type.Equals(GetType(SolidEdgePart.Sketch)) Then
            ReportItem(DirectCast(item, SolidEdgePart.Sketch))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.Arc2d)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.Arc2d))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.CenterMark)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.CenterMark))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.Circle2d)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.Circle2d))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.DatumFrame)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.DatumFrame))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.Dimension)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.Dimension))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.Line2d)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.Line2d))
        ElseIf type.Equals(GetType(SolidEdgeFrameworkSupport.TextBox)) Then
            ReportItem(DirectCast(item, SolidEdgeFrameworkSupport.TextBox))
        End If
    End Sub

    Private Shared Sub ReportItem(ByVal occurrence As SolidEdgeAssembly.Occurrence)
        Console.WriteLine("Name: {0}", occurrence.Name)
        Console.WriteLine("OccurrenceFileName: {0}", occurrence.OccurrenceFileName)
    End Sub

    Private Shared Sub ReportItem(ByVal drawingView As SolidEdgeDraft.DrawingView)
        Console.WriteLine("Name: {0}", drawingView.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal extrudedCutout As SolidEdgePart.ExtrudedCutout)
        Console.WriteLine("DisplayName: {0}", extrudedCutout.DisplayName)
        Console.WriteLine("EdgebarName: {0}", extrudedCutout.EdgebarName)
        Console.WriteLine("Name: {0}", extrudedCutout.Name)
        Console.WriteLine("SystemName: {0}", extrudedCutout.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal extrudedProtrusion As SolidEdgePart.ExtrudedProtrusion)
        Console.WriteLine("DisplayName: {0}", extrudedProtrusion.DisplayName)
        Console.WriteLine("EdgebarName: {0}", extrudedProtrusion.EdgebarName)
        Console.WriteLine("Name: {0}", extrudedProtrusion.Name)
        Console.WriteLine("SystemName: {0}", extrudedProtrusion.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal featureGroup As SolidEdgePart.FeatureGroup)
        Console.WriteLine("DisplayName: {0}", featureGroup.DisplayName)
        Console.WriteLine("EdgebarName: {0}", featureGroup.EdgebarName)
        Console.WriteLine("Name: {0}", featureGroup.Name)
        Console.WriteLine("SystemName: {0}", featureGroup.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal flange As SolidEdgePart.Flange)
        Console.WriteLine("DisplayName: {0}", flange.DisplayName)
        Console.WriteLine("EdgebarName: {0}", flange.EdgebarName)
        Console.WriteLine("Name: {0}", flange.Name)
        Console.WriteLine("SystemName: {0}", flange.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal hole As SolidEdgePart.Hole)
        Console.WriteLine("DisplayName: {0}", hole.DisplayName)
        Console.WriteLine("EdgebarName: {0}", hole.EdgebarName)
        Console.WriteLine("Name: {0}", hole.Name)
        Console.WriteLine("SystemName: {0}", hole.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal round As SolidEdgePart.Round)
        Console.WriteLine("DisplayName: {0}", round.DisplayName)
        Console.WriteLine("EdgebarName: {0}", round.EdgebarName)
        Console.WriteLine("Name: {0}", round.Name)
        Console.WriteLine("SystemName: {0}", round.SystemName)
    End Sub

    Private Shared Sub ReportItem(ByVal sketch As SolidEdgePart.Sketch)
        Console.WriteLine("Name: {0}", sketch.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal arc2d As SolidEdgeFrameworkSupport.Arc2d)
        Console.WriteLine("Name: {0}", arc2d.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal centerMark As SolidEdgeFrameworkSupport.CenterMark)
        Console.WriteLine("Name: {0}", centerMark.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal circle2d As SolidEdgeFrameworkSupport.Circle2d)
        Console.WriteLine("Name: {0}", circle2d.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal datumFrame As SolidEdgeFrameworkSupport.DatumFrame)
        Console.WriteLine("Name: {0}", datumFrame.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal dimension As SolidEdgeFrameworkSupport.Dimension)
        Console.WriteLine("DisplayName: {0}", dimension.DisplayName)
        Console.WriteLine("ExposeName: {0}", dimension.ExposeName)
        Console.WriteLine("Name: {0}", dimension.Name)
        Console.WriteLine("SystemName: {0}", dimension.SystemName)
        Console.WriteLine("VariableTableName: {0}", dimension.VariableTableName)
    End Sub

    Private Shared Sub ReportItem(ByVal line2d As SolidEdgeFrameworkSupport.Line2d)
        Console.WriteLine("Name: {0}", line2d.Name)
    End Sub

    Private Shared Sub ReportItem(ByVal textBox As SolidEdgeFrameworkSupport.TextBox)
        Console.WriteLine("Name: {0}", textBox.Name)
    End Sub
End Class
