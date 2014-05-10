Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.FileProperties
	''' <summary>
	''' Reports file properties of a file.
	''' </summary>
	Friend Class ReportProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim propertySets As SolidEdgeFileProperties.PropertySets = Nothing
			Dim properties As SolidEdgeFileProperties.Properties = Nothing
			Dim [property] As SolidEdgeFileProperties.Property = Nothing

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

				' Open the file in readonly mode.
				propertySets.Open(fileName, True)

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
	End Class
End Namespace
