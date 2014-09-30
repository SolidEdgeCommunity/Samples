Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Application
	''' <summary>
	''' Reports all addins registered with Solid Edge.
	''' </summary>
	Friend Class ReportAddIns
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim addins As SolidEdgeFramework.AddIns = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the addins collection.
				addins = application.AddIns

				' Loop through each addin.
				For Each addin In addins.OfType(Of SolidEdgeFramework.AddIn)()
					Console.WriteLine("Description: {0}", addin.Description)
					Console.WriteLine("GUID: {0}", addin.GUID)
					Console.WriteLine("GuiVersion: {0}", addin.GuiVersion)
					Console.WriteLine("ProgID: {0}", addin.ProgID)
					Console.WriteLine("Connect: {0}", addin.Connect)
					Console.WriteLine("Visible: {0}", addin.Visible)
					Console.WriteLine()
				Next addin
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
