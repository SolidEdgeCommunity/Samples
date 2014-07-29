Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace FileProperties.ConsoleDemo
	Friend Class Program
		<STAThread> _
		Shared Sub Main(ByVal args() As String)
			' Build path to file.
			Dim filename As String = Path.Combine(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath(), "Coffee Pot.par")

			' Ideally, you want to run the Interop logic in a separate AppDomain.
			' If you don't, you will experience file locking. Even after Close().

			' Add or Update custom properties.
			UpdateCustomProperties(filename)

			' Report properties.
			ReportProperties(filename)

			' Delete previously added custom properties.
			DeleteCustomProperties(filename)
		End Sub

		Private Shared Sub DeleteCustomProperties(ByVal filename As String)
			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing
			Dim customPropertySet As SolidEdgeFileProperties.Properties = Nothing
			Dim [property] As SolidEdgeFileProperties.Property = Nothing
			Dim customProperties As New List(Of SolidEdgeFileProperties.Property)()

			Dim fileInfo As New FileInfo(filename)

			Console.WriteLine("Processing '{0}'.", fileInfo.FullName)

			Try
				If fileInfo.Exists = False Then
					Throw New FileNotFoundException("File not found.", fileInfo.FullName)
				End If

				' Create a new instance of PropertySets.
				propertySets = New SolidEdgeFileProperties.PropertySets()

				' Open the file in edit mode.
				propertySets.Open(fileInfo.FullName, False)

				' Get a reference to the custom property set.
				customPropertySet = DirectCast(propertySets("Custom"), SolidEdgeFileProperties.Properties)

				' Loop through the properties for the current PropertySet.
				For j As Integer = 0 To customPropertySet.Count - 1
					' Get a reference to the current PropertySet.
					[property] = DirectCast(customPropertySet(j), SolidEdgeFileProperties.Property)

					If [property].Name.StartsWith("My") Then
						customProperties.Add([property])
					End If
				Next j

				' Loop through the custom properties in the List<>.
				For Each customProperty As SolidEdgeFileProperties.Property In customProperties
					Dim customPropertyName As String = customProperty.Name

					' Delete the custom property.
					customProperty.Delete()

					Console.WriteLine("Deleted '{0}' custom property.", customPropertyName)
				Next customProperty

				' Save the changes.
				propertySets.Save()
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				If propertySets IsNot Nothing Then
					propertySets.Close()
				End If
			End Try
		End Sub

		Private Shared Sub ReportProperties(ByVal filename As String)
			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing
			Dim properties As SolidEdgeFileProperties.Properties = Nothing
			Dim [property] As SolidEdgeFileProperties.Property = Nothing

			Dim fileInfo As New FileInfo(filename)

			Console.WriteLine("Processing '{0}'.", fileInfo.FullName)

			Try
				If fileInfo.Exists = False Then
					Throw New FileNotFoundException("File not found.", fileInfo.FullName)
				End If

				' Create a new instance of PropertySets.
				propertySets = New SolidEdgeFileProperties.PropertySets()

				' Open the file in readonly mode.
				propertySets.Open(fileInfo.FullName, True)

				' Loop through the PropertySets.
				For i As Integer = 0 To propertySets.Count - 1
					' Get a reference to the current PropertySet.
					properties = DirectCast(propertySets(i), SolidEdgeFileProperties.Properties)

					Console.WriteLine(properties.Name)

					' Loop through the properties for the current PropertySet.
					For j As Integer = 0 To properties.Count - 1
						' Get a reference to the current PropertySet.
						[property] = DirectCast(properties(j), SolidEdgeFileProperties.Property)

						Dim propertyValue As Object = Nothing
						Dim typeName As String = "Unknown"

						' property.Value can throw an exception!
						Try
							' Attempt to get the property value.
							propertyValue = [property].Value
							typeName = propertyValue.GetType().FullName
						Catch ex As System.Exception
							propertyValue = ex.Message
						End Try

						Console.WriteLine(ControlChars.Tab & "{0} ({1}) = '{2}'", [property].Name, typeName, propertyValue)
					Next j
				Next i
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				If propertySets IsNot Nothing Then
					propertySets.Close()
				End If
			End Try
		End Sub

		Private Shared Sub UpdateCustomProperties(ByVal filename As String)
			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing

			Dim fileInfo As New FileInfo(filename)

			Console.WriteLine("Processing '{0}'.", fileInfo.FullName)

			Try
				If fileInfo.Exists = False Then
					Throw New FileNotFoundException("File not found.", fileInfo.FullName)
				End If

				' Create a new instance of PropertySets.
				propertySets = New SolidEdgeFileProperties.PropertySets()

				' Open the file in edit mode.
				propertySets.Open(fileInfo.FullName, False)

				' Add a string.
				AddOrUpdateCustomProperty(propertySets, "My String", "My text")

				' Add a number.
				AddOrUpdateCustomProperty(propertySets, "My Integer", Integer.MaxValue)

				' Add a boolean (Yes\No).
				AddOrUpdateCustomProperty(propertySets, "My Boolean", True)

				' Add a DateTime.
				AddOrUpdateCustomProperty(propertySets, "My Date", Date.Now)

				' Save the changes.
				propertySets.Save()
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				If propertySets IsNot Nothing Then
					propertySets.Close()
				End If
			End Try
		End Sub

		''' <summary>
		''' This method will either add a custom property if it doesn't exist or update an existing custom property.
		''' </summary>
		Private Shared Sub AddOrUpdateCustomProperty(ByVal propertySets As SolidEdgeFileProperties.PropertySets, ByVal propertyName As String, ByVal propertyValue As Object)
			Dim customPropertySet As SolidEdgeFileProperties.Properties = Nothing
			Dim [property] As SolidEdgeFileProperties.Property = Nothing

			Try
				' Get a reference to the custom property set.
				customPropertySet = DirectCast(propertySets("Custom"), SolidEdgeFileProperties.Properties)

				' Attempt to get the custom property.
				[property] = DirectCast(customPropertySet(propertyName), SolidEdgeFileProperties.Property)

				' If we get here, the custom property exists so update the value.
				[property].Value = propertyValue

				Console.WriteLine("Updated '{0}' custom property.", propertyName)
			Catch e1 As System.Runtime.InteropServices.COMException
				' If we get here, the custom property does not exist so add it.
				[property] = DirectCast(customPropertySet.Add(propertyName, propertyValue), SolidEdgeFileProperties.Property)

				Console.WriteLine("Added '{0}' custom property.", propertyName)
			Catch
				Throw
			End Try
		End Sub
	End Class
End Namespace
