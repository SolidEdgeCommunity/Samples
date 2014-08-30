Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Assembly
	''' <summary>
	''' Creates and applies a new configuration to the active assembly.
	''' </summary>
	Friend Class AddNewConfigurationAndApply
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
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a refrence to the active assembly document.
				assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

				If assemblyDocument IsNot Nothing Then
					' Get a reference tot he Configurations collection.
					configurations = assemblyDocument.Configurations

					' Configuration name has to be unique so for demonstration
					' purposes, use a random number.
					Dim random As New Random()
					Dim configName As String = String.Format("Configuration {0}", random.Next())

					' Add the new configuration.
					configuration = configurations.Add(configName)

					' Make the new configuration the active configuration.
					configuration.Apply()
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
