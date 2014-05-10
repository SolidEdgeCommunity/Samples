Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Assembly
	''' <summary>
	''' Creates a new configuration in the active assembly.
	''' </summary>
	Friend Class AddDerivedConfiguration
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
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the active assembly document.
				assemblyDocument = application.TryActiveDocumentAs(Of SolidEdgeAssembly.AssemblyDocument)()

				If assemblyDocument IsNot Nothing Then
					' Get a reference tot he Configurations collection.
					configurations = assemblyDocument.Configurations

					' Configuration name has to be unique so for demonstration
					' purposes, use a random number.
					Dim random As New Random()
					Dim configName As String = String.Format("Configuration {0}", random.Next())

					configuration = configurations.Item(1)

					Dim configList As Object = New String() { configuration.Name }

					Dim missing As Object = System.Reflection.Missing.Value

					' NOTE: Not sure why but this is causing Solid Edge ST6 to crash.
					' Add the new configuration.
					configuration = configurations.AddDerivedConfig(1, 0, 0, configList, missing, missing, configName)
				Else
					Throw New System.Exception(Resources.NoActiveAssemblyDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
