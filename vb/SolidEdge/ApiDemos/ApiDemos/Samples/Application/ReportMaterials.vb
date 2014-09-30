Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Application
	''' <summary>
	''' Reports all of the materials defined in the material table.
	''' </summary>
	Friend Class ReportMaterials
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim matTable As SolidEdgeFramework.MatTable = Nothing
			Dim varMatLibName As Object = Nothing
			Dim plNumMaterials As Integer = 0
			Dim listOfMaterials As Object = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the material table.
				matTable = application.GetMaterialTable()

				' Get the path to Material.mtl.
				matTable.GetMatLibFileName(varMatLibName)

				matTable.GetMaterialList(plNumMaterials, listOfMaterials)

				Dim materialArray() As Object = DirectCast(listOfMaterials, Object())

				Console.WriteLine("Material Table: '{0}'.", varMatLibName)

				' Loop through and report each material.
				For Each material As String In materialArray
					Console.WriteLine(material)
				Next material
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
