Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Application
	''' <summary>
	''' Reports the Solid Edge global system information.
	''' </summary>
	Friend Class ReportGlobalSystemInformation
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim val As Object = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get the global system information global parameter.
				application.GetGlobalParameter(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo, val)

				Console.WriteLine(val)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
