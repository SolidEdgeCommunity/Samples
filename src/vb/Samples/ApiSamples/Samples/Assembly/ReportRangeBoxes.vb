Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Assembly
	''' <summary>
	''' Reports all occurrences range boxes of the active assembly.
	''' </summary>
	Friend Class ReportRangeBoxes
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get the active document.
				assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

				If assemblyDocument IsNot Nothing Then
					' Get a reference to the Occurrences collection.
					occurrences = assemblyDocument.Occurrences

					For Each occurrence In occurrences.OfType(Of SolidEdgeAssembly.Occurrence)()
						Dim MinRangePoint As Array = Array.CreateInstance(GetType(Double), 0)
						Dim MaxRangePoint As Array = Array.CreateInstance(GetType(Double), 0)
						occurrence.GetRangeBox(MinRangePoint, MaxRangePoint)

						' Convert from System.Array to double[].  double[] is easier to work with.
						Dim a1() As Double = MinRangePoint.OfType(Of Double)().ToArray()
						Dim a2() As Double = MaxRangePoint.OfType(Of Double)().ToArray()

						' Report the occurrence matrix.
						Console.WriteLine("{0} range box:", occurrence.Name)
						Console.WriteLine("|MinRangePoint: {0}, {1}, {2}|", a1(0), a1(1), a1(2))
						Console.WriteLine("|MaxRangePoint: {0}, {1}, {2}|", a2(0), a2(1), a2(2))
					Next occurrence
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
