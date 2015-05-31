Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active assembly document.
            Dim document = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

            If document IsNot Nothing Then
                Dim propertySets = DirectCast(document.Properties, SolidEdgeFramework.PropertySets)

                For Each properties In propertySets.OfType(Of SolidEdgeFramework.Properties)()
                    Console.WriteLine("PropertSet '{0}'.", properties.Name)

                    For Each [property] In properties.OfType(Of SolidEdgeFramework.Property)()
                        Dim nativePropertyType As System.Runtime.InteropServices.VarEnum = System.Runtime.InteropServices.VarEnum.VT_EMPTY
                        Dim runtimePropertyType As Type = Nothing

                        Dim value As Object = Nothing

                        nativePropertyType = CType([property].Type, System.Runtime.InteropServices.VarEnum)

                        ' Accessing Value property may throw an exception...
                        Try
                            value = [property].get_Value()
                        Catch ex As System.Exception
                            value = ex.Message
                        End Try

                        If value IsNot Nothing Then
                            runtimePropertyType = value.GetType()
                        End If

                        Console.WriteLine(ControlChars.Tab & "{0} = '{1}' ({2} | {3}).", [property].Name, value, nativePropertyType, runtimePropertyType)
                    Next [property]

                    Console.WriteLine()
                Next properties
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
