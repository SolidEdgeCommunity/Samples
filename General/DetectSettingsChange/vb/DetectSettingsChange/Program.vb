Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        ' Dictionary variables to hold before and after settings.
        Dim snapshot1 = New Dictionary(Of SolidEdgeFramework.ApplicationGlobalConstants, Object)()
        Dim snapshot2 = New Dictionary(Of SolidEdgeFramework.ApplicationGlobalConstants, Object)()

        ' Connect to Solid Edge.
        Dim application = SolidEdgeUtils.Connect()

        ' Begin snapshot 1 of application global constants.
        CaptureApplicationGlobalConstants(application, snapshot1)

        ' Force break-point. Change the Solid Edge setting in question.
        System.Diagnostics.Debugger.Break()

        ' Begin snapshot 2 of application global constants.
        CaptureApplicationGlobalConstants(application, snapshot2)

        ' Report the changes from snapshot 1 and snapshot 2.
        Dim enumerator = snapshot1.GetEnumerator()

        Do While enumerator.MoveNext()
            Dim enumConstant = enumerator.Current.Key
            Dim enumValue1 = enumerator.Current.Value
            Dim enumValue2 = snapshot2(enumConstant)

            ' We can safely ignore seApplicationGlobalSystemInfo. It's just noise for our purpose.
            If enumConstant.Equals(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo) Then
                Continue Do
            End If

            ' Check to see if the snapshot 1 and snapshot 2 value is equal.
            If Object.Equals(enumValue1, enumValue2) = False Then
                Console.WriteLine("{0}: '{1}' '{2}'", enumConstant, enumValue1, enumValue2)
            End If
        Loop

        ' This will pause the console window.
        Console.WriteLine()
        Console.WriteLine("Press enter to contine.")
        Console.ReadLine()
    End Sub

    Private Shared Sub CaptureApplicationGlobalConstants(ByVal application As SolidEdgeFramework.Application, ByVal dictionary As Dictionary(Of SolidEdgeFramework.ApplicationGlobalConstants, Object))
        ' Get list of SolidEdgeFramework.ApplicationGlobalConstants names and values.
        Dim enumConstants = System.Enum.GetNames(GetType(SolidEdgeFramework.ApplicationGlobalConstants)).ToArray()
        Dim enumValues = System.Enum.GetValues(GetType(SolidEdgeFramework.ApplicationGlobalConstants)).Cast(Of SolidEdgeFramework.ApplicationGlobalConstants)().ToArray()

        ' Populate the dictionary.
        For i As Integer = 0 To enumValues.Length - 1
            Dim enumConstant = enumConstants(i)
            Dim enumValue = enumValues(i)
            Dim value As Object = Nothing

            If enumValue.Equals(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalOpenAsReadOnly3DFile) Then
                Continue For
            End If

            ' We can safely ignore seApplicationGlobalSystemInfo. It's just noise for our purpose.
            If enumConstant.Equals(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo) Then
                Continue For
            End If

            application.GetGlobalParameter(enumValue, value)
            dictionary.Add(enumValue, value)
        Next i
    End Sub
End Class
