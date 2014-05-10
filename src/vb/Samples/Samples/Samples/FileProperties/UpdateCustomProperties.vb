Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.FileProperties
	''' <summary>
	''' Adds or updates custom file properties of a file.
	''' </summary>
	Friend Class UpdateCustomProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing

			' Build path to file.
			Dim fileName As String = Path.Combine(InstallDataHelper.GetTrainingFolderPath(), "Coffee Pot.par")

			Dim fileInfo As New FileInfo(fileName)

			Console.WriteLine("Processing '{0}'.", fileName)

			Try
				If fileInfo.Exists = False Then
					Throw New FileNotFoundException("File not found.", fileName)
				End If

				' Create a new instance of PropertySets.
				propertySets = New SolidEdgeFileProperties.PropertySets()

				' Open the file in edit mode.
				propertySets.Open(fileName, False)

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
