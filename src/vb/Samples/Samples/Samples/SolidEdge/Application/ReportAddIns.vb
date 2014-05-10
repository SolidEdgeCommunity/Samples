Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
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
			Dim addin As SolidEdgeFramework.AddIn = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the addins collection.
				addins = application.AddIns

				' Loop through each addin.
				For i As Integer = 1 To addins.Count
					addin = addins.Item(i)

					Console.WriteLine("Description: {0}", addin.Description)
					Console.WriteLine("GUID: {0}", addin.GUID)
					Console.WriteLine("GuiVersion: {0}", addin.GuiVersion)
					Console.WriteLine("ProgID: {0}", addin.ProgID)
					Console.WriteLine("Connect: {0}", addin.Connect)
					Console.WriteLine("Visible: {0}", addin.Visible)
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
