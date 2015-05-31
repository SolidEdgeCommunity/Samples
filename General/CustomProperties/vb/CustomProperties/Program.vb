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
            Dim document = application.GetActiveDocument()

            If document IsNot Nothing Then
                Dim propertySets = DirectCast(document.Properties, SolidEdgeFramework.PropertySets)

                AddCustomProperties(propertySets)
                ReportCustomProperties(propertySets)
                DeleteCustomProperties(propertySets)
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Sub AddCustomProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
        Dim properties = DirectCast(propertySets.Item("Custom"), SolidEdgeFramework.Properties)
        Dim propertyValues = New Object() { "My text", Integer.MaxValue, 1.23, True, Date.Now }

        For Each propertyValue In propertyValues
            Dim propertyType = propertyValue.GetType()
            Dim propertyName = String.Format("My {0}", propertyType)
            Dim [property] = properties.Add(propertyName, propertyValue)

            Console.WriteLine("Added {0} - {1}.", [property].Name, propertyValue)
        Next propertyValue
    End Sub

    Private Shared Sub DeleteCustomProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
        Dim properties = DirectCast(propertySets.Item("Custom"), SolidEdgeFramework.Properties)

        ' Query for custom properties that start with "My".
        Dim query = properties.OfType(Of SolidEdgeFramework.Property)().Where(Function(x) x.Name.StartsWith("My"))

        ' Force ToArray() so that Delete() doesn't interfere with the enumeration.
        For Each [property] In query.ToArray()
            Dim propertyName = [property].Name
            [property].Delete()
            Console.WriteLine("Deleted {0}.", propertyName)
        Next [property]
    End Sub

    Private Shared Sub ReportCustomProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
        Dim properties = DirectCast(propertySets.Item("Custom"), SolidEdgeFramework.Properties)

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
    End Sub
End Class
