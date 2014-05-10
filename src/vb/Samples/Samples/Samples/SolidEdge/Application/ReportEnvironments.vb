Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	''' <summary>
	''' Reports all of the Solid Edge environments.
	''' </summary>
	Friend Class ReportEnvironments
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim environments As SolidEdgeFramework.Environments = Nothing
			Dim environment As SolidEdgeFramework.Environment = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the Environments collection.
				environments = application.Environments

				' Loop through each addin.
				For i As Integer = 1 To environments.Count
					environment = environments.Item(i)

					Console.WriteLine("Caption: {0}", environment.Caption)
					Console.WriteLine("CATID: {0}", environment.CATID)
					Console.WriteLine("Index: {0}", environment.Index)
					Console.WriteLine("Name: {0}", environment.Name)
					Console.WriteLine("SubTypeName: {0}", environment.SubTypeName)
					Console.WriteLine()
				Next i
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
