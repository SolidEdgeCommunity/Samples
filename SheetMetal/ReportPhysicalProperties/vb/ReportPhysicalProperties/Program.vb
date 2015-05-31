Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim density As Double = 0
        Dim accuracy As Double = 0
        Dim volume As Double = 0
        Dim area As Double = 0
        Dim mass As Double = 0
        Dim cetnerOfGravity As Array = Array.CreateInstance(GetType(Double), 3)
        Dim centerOfVolumne As Array = Array.CreateInstance(GetType(Double), 3)
        Dim globalMomentsOfInteria As Array = Array.CreateInstance(GetType(Double), 6) ' Ixx, Iyy, Izz, Ixy, Ixz and Iyz
        Dim principalMomentsOfInteria As Array = Array.CreateInstance(GetType(Double), 3) ' Ixx, Iyy and Izz
        Dim principalAxes As Array = Array.CreateInstance(GetType(Double), 9) ' 3 axes x 3 coords
        Dim radiiOfGyration As Array = Array.CreateInstance(GetType(Double), 9) ' 3 axes x 3 coords
        Dim relativeAccuracyAchieved As Double = 0
        Dim status As Integer = 0

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active document.
            sheetMetalDocument = application.GetActiveDocument(Of SolidEdgePart.SheetMetalDocument)(False)

            If sheetMetalDocument IsNot Nothing Then
                density = 1
                accuracy = 0.05

                ' Get a reference to the Models collection.
                models = sheetMetalDocument.Models

                ' Get a reference to the Model.
                model = models.Item(1)

                ' Compute the physical properties.
                model.GetPhysicalProperties(Status:= status, Density:= density, Accuracy:= accuracy, Volume:= volume, Area:= area, Mass:= mass, CenterOfGravity:= cetnerOfGravity, CenterOfVolume:= centerOfVolumne, GlobalMomentsOfInteria:= globalMomentsOfInteria, PrincipalMomentsOfInteria:= principalMomentsOfInteria, PrincipalAxes:= principalAxes, RadiiOfGyration:= radiiOfGyration, RelativeAccuracyAchieved:= relativeAccuracyAchieved)

                Console.WriteLine("GetPhysicalProperties() results:")

                ' Write results to screen.

                Console.WriteLine("Density: {0}", density)
                Console.WriteLine("Accuracy: {0}", accuracy)
                Console.WriteLine("Volume: {0}", volume)
                Console.WriteLine("Area: {0}", area)
                Console.WriteLine("Mass: {0}", mass)

                ' Convert from System.Array to double[].  double[] is easier to work with.
                Dim m() As Double = cetnerOfGravity.OfType(Of Double)().ToArray()

                Console.WriteLine("CenterOfGravity:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))

                m = centerOfVolumne.OfType(Of Double)().ToArray()

                Console.WriteLine("CenterOfVolume:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))

                m = globalMomentsOfInteria.OfType(Of Double)().ToArray()

                Console.WriteLine("GlobalMomentsOfInteria:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(3), m(4), m(5))

                m = principalMomentsOfInteria.OfType(Of Double)().ToArray()

                Console.WriteLine("PrincipalMomentsOfInteria:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))

                m = principalAxes.OfType(Of Double)().ToArray()

                Console.WriteLine("PrincipalAxes:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(3), m(4), m(5))
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(6), m(7), m(8))

                m = radiiOfGyration.OfType(Of Double)().ToArray()

                Console.WriteLine("RadiiOfGyration:")
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(0), m(1), m(2))
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(3), m(4), m(5))
                Console.WriteLine(ControlChars.Tab & "|{0}, {1}, {2}|", m(6), m(7), m(8))

                Console.WriteLine("RelativeAccuracyAchieved: {0}", relativeAccuracyAchieved)
                Console.WriteLine("Status: {0}", status)
                Console.WriteLine()

                ' Show physical properties window.
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalToolsPhysicalProperties)
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
