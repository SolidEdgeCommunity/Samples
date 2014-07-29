Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Assembly
	''' <summary>
	''' Reports all configurations of the active assembly.
	''' </summary>
	Friend Class ReportConfigurations
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim configurations As SolidEdgeAssembly.Configurations = Nothing
			Dim configuration As SolidEdgeAssembly.Configuration = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to active assembly document.
				assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

				If assemblyDocument IsNot Nothing Then
					' Get a reference tot he Configurations collection.
					configurations = assemblyDocument.Configurations

					' Iterate through all of the configurations.
					For i As Integer = 1 To configurations.Count
						' Get the configuration at the specified index.
						configuration = configurations.Item(i)

						Console.WriteLine("Configuration Name: '{0}' | Configuration Type: {1}.", configuration.Name, configuration.ConfigurationType)
					Next i
				Else
					Throw New System.Exception(Resources.NoActiveAssemblyDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
