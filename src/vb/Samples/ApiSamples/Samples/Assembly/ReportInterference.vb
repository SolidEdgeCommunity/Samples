Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text

Namespace ApiSamples.Assembly
	''' <summary>
	''' Reports interference between all occurrences of the active assembly.
	''' </summary>
	Friend Class ReportInterference
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim occurrences As SolidEdgeAssembly.Occurrences = Nothing
			Dim occurrence As SolidEdgeAssembly.Occurrence = Nothing
			Dim interferenceStatus As SolidEdgeAssembly.InterferenceStatusConstants = Nothing
			Dim compare As SolidEdgeConstants.InterferenceComparisonConstants = SolidEdgeConstants.InterferenceComparisonConstants.seInterferenceComparisonSet1vsAllOther
			Dim reportType As SolidEdgeConstants.InterferenceReportConstants = SolidEdgeConstants.InterferenceReportConstants.seInterferenceReportPartNames

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to the active assembly document.
				assemblyDocument = application.GetActiveDocument(Of SolidEdgeAssembly.AssemblyDocument)(False)

				If assemblyDocument IsNot Nothing Then
					' Get a reference to the Occurrences collection.
					occurrences = assemblyDocument.Occurrences

					For i As Integer = 1 To occurrences.Count
						' Get a reference to the occurrence.
						occurrence = occurrences.Item(i)

						Dim set1 As Array = Array.CreateInstance(DirectCast(occurrence, Object).GetType(), 1)
						Dim numInterferences As Object = 0
						Dim retSet1 As Object = Array.CreateInstance(GetType(SolidEdgeAssembly.Occurrence), 0)
						Dim retSet2 As Object = Array.CreateInstance(GetType(SolidEdgeAssembly.Occurrence), 0)
						Dim confirmedInterference As Object = Nothing
						Dim interferenceOccurrence As Object = Nothing

						set1.SetValue(occurrence, 0)

						' Check interference.
						assemblyDocument.CheckInterference(NumElementsSet1:= set1.Length, Set1:= set1, Status:= interferenceStatus, ComparisonMethod:= compare, NumElementsSet2:= 0, Set2:= Missing.Value, AddInterferenceAsOccurrence:= False, ReportFilename:= Missing.Value, ReportType:= reportType, NumInterferences:= numInterferences, InterferingPartsSet1:= retSet1, InterferingPartsOtherSet:= retSet2, ConfirmedInterference:= confirmedInterference, InterferenceOccurrence:= interferenceOccurrence, IgnoreThreadInterferences:= Missing.Value)

						' Process status.
						Select Case interferenceStatus
							Case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusNoInterference
							Case SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedAndProbableInterference, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusConfirmedInterference, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusIncompleteAnalysis, SolidEdgeAssembly.InterferenceStatusConstants.seInterferenceStatusProbableInterference
								If retSet2 IsNot Nothing Then
									For j As Integer = 0 To (DirectCast(numInterferences, Integer)) - 1
										Dim obj1 As Object = DirectCast(retSet1, Array).GetValue(j)
										Dim obj2 As Object = DirectCast(retSet2, Array).GetValue(j)

										' Use ReflectionHelper class to get the object type.
										Dim objectType1 As SolidEdgeFramework.ObjectType = ReflectionHelper.GetObjectType(obj1)
										Dim objectType2 As SolidEdgeFramework.ObjectType = ReflectionHelper.GetObjectType(obj2)

										Dim reference1 As SolidEdgeFramework.Reference = Nothing
										Dim reference2 As SolidEdgeFramework.Reference = Nothing
										Dim occurrence1 As SolidEdgeAssembly.Occurrence = Nothing
										Dim occurrence2 As SolidEdgeAssembly.Occurrence = Nothing

										Select Case objectType1
											Case SolidEdgeFramework.ObjectType.igReference
												reference1 = DirectCast(obj1, SolidEdgeFramework.Reference)
											Case SolidEdgeFramework.ObjectType.igPart, SolidEdgeFramework.ObjectType.igOccurrence
												occurrence1 = DirectCast(obj1, SolidEdgeAssembly.Occurrence)
										End Select

										Select Case objectType2
											Case SolidEdgeFramework.ObjectType.igReference
												reference2 = DirectCast(obj2, SolidEdgeFramework.Reference)
											Case SolidEdgeFramework.ObjectType.igPart, SolidEdgeFramework.ObjectType.igOccurrence
												occurrence2 = DirectCast(obj2, SolidEdgeAssembly.Occurrence)
										End Select
									Next j
								End If
						End Select
					Next i
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
