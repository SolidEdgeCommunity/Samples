Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.FileProperties
	''' <summary>
	''' Deletes specific custom file properties of a file.
	''' </summary>
	Friend Class DeleteCustomProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing
			Dim customPropertySet As SolidEdgeFileProperties.Properties = Nothing
			Dim [property] As SolidEdgeFileProperties.Property = Nothing
			Dim customProperties As New List(Of SolidEdgeFileProperties.Property)()

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
	End Class
End Namespace
