Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.Revision_Manager
	''' <summary>
	''' Reports file properties of a file.
	''' </summary>
	Friend Class ReportFileProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As RevisionManager.Application = Nothing
			Dim propertySets As RevisionManager.PropertySets = Nothing
			Dim properties As RevisionManager.Properties = Nothing
			Dim [property] As RevisionManager.Property = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = New RevisionManager.Application()

				' Build path to file.
				Dim fileName As String = Path.Combine(InstallDataHelper.GetTrainingFolderPath(), "Coffee Pot.par")

				Dim fileInfo As New FileInfo(fileName)

				Console.WriteLine("Processing '{0}'.", fileName)

				propertySets = DirectCast(application.PropertySets, RevisionManager.PropertySets)
				propertySets.Open(fileName, True)

				For i As Integer = 0 To propertySets.Count - 1
					properties = DirectCast(propertySets.Item(i), RevisionManager.Properties)

					Console.WriteLine(properties.Name)

					For j As Integer = 0 To properties.Count - 1
						[property] = CType(properties.Item(j), RevisionManager.Property)

						Dim propertyValue As Object = Nothing
						Dim typeName As String = "Unknown"

						' property.Value can throw an exception!
						Try
							' Attempt to get the property value.
							' In my testing, iCnt.exe would crash when accessing some properties.
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
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
