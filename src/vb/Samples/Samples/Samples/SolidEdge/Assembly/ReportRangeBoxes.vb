Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Assembly
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
			Dim occurrence As SolidEdgeAssembly.Occurrence = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get the active document.
				assemblyDocument = application.TryActiveDocumentAs(Of SolidEdgeAssembly.AssemblyDocument)()

				If assemblyDocument IsNot Nothing Then
					' Get a reference to the Occurrences collection.
					occurrences = assemblyDocument.Occurrences

					For i As Integer = 1 To occurrences.Count
						' Get a reference to the occurrence.
						occurrence = occurrences.Item(i)

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
					Next i
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
