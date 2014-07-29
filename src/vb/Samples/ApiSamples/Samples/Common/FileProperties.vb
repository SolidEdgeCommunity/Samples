Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Common
	Friend Class FileProperties
		Public Shared Sub AddCustomProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
			Dim properties = DirectCast(propertySets.Item("Custom"), SolidEdgeFramework.Properties)
			Dim propertyValues = New Object() { "My text", Integer.MaxValue, 1.23, True, Date.Now }

			For Each propertyValue In propertyValues
				Dim propertyType = propertyValue.GetType()
				Dim propertyName = String.Format("My {0}", propertyType)
				Dim [property] = properties.Add(propertyName, propertyValue)

				Console.WriteLine("Added {0} - {1}.", [property].Name, propertyValue)
			Next propertyValue
		End Sub

		Public Shared Sub DeleteCustomProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
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

		Public Shared Sub ReportAllProperties(ByVal propertySets As SolidEdgeFramework.PropertySets)
			For i As Integer = 1 To propertySets.Count
				Dim properties = propertySets.Item(i)

				Console.WriteLine("PropertSet '{0}'.", properties.Name)

				For j As Integer = 1 To properties.Count
					Dim nativePropertyType As System.Runtime.InteropServices.VarEnum = System.Runtime.InteropServices.VarEnum.VT_EMPTY
					Dim runtimePropertyType As Type = Nothing

					Dim value As Object = Nothing

                    Dim objProperty = properties.Item(j)
                    nativePropertyType = DirectCast(objProperty.Type, System.Runtime.InteropServices.VarEnum)

					' Accessing Value property may throw an exception...
					Try
                        value = objProperty.Value
					Catch ex As System.Exception
						value = ex.Message
					End Try

					If value IsNot Nothing Then
						runtimePropertyType = value.GetType()
					End If

                    Console.WriteLine(ControlChars.Tab & "{0} = '{1}' ({2} | {3}).", objProperty.Name, value, nativePropertyType, runtimePropertyType)
				Next j

				Console.WriteLine()
			Next i
		End Sub
	End Class
End Namespace
