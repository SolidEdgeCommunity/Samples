Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	''' <summary>
	''' Reports information about the Material.mtl XML file.
	''' </summary>
	Friend Class ReportMaterialLibraryFile
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim matTable As SolidEdgeFramework.MatTable = Nothing
			Dim varMatLibName As Object = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the material table.
				matTable = application.GetMaterialTable()

				' Get the path to Material.mtl.
				matTable.GetMatLibFileName(varMatLibName)

				' Convert raw path string into FileInfo object.
				Dim fileInfo As New FileInfo(varMatLibName.ToString())

				If fileInfo.Exists Then
					Console.WriteLine("'{0}' found.", fileInfo.FullName)
				Else
					Console.WriteLine("'{0}' does not exist.", fileInfo.FullName)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
