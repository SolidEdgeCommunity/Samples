Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Assembly
	''' <summary>
	''' Creates a new assembly and adds a structural frame.
	''' </summary>
	Friend Class CreateStructuralFrame
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim lineSegments As SolidEdgeAssembly.LineSegments = Nothing
			Dim lineSegment As SolidEdgeAssembly.LineSegment = Nothing
			Dim lineSegmentList As New List(Of SolidEdgeAssembly.LineSegment)()
			Dim structuralFrames As SolidEdgeAssembly.StructuralFrames = Nothing
			Dim structuralFrame As SolidEdgeAssembly.StructuralFrame = Nothing
			Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
			Dim startPointArray As Array = New Double() { 0.0, 0.0, 0.0 }
			Dim endPointArray As Array = New Double() { 0.0, 0.0, 0.5 }

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				' Create a new assembly document.
				assemblyDocument = documents.AddAssemblyDocument()

				' Always a good idea to give SE a chance to breathe.
				application.DoIdle()

				' Get a reference to the LineSegments collection.
				lineSegments = assemblyDocument.LineSegments

				' Add a new line segment.
				lineSegment = lineSegments.Add(StartPoint:= startPointArray, EndPoint:= endPointArray)

				' Store line segment in array.
				lineSegmentList.Add(lineSegment)

				' Get a reference to the StructuralFrames collection.
				structuralFrames = assemblyDocument.StructuralFrames

				' Build path to part file.  In this case, it is a .par from standard install.
				Dim filename As String = System.IO.Path.Combine(InstallDataHelper.GetInstalledPath(), "Frames\DIN\I-Beam\I-Beam 80x46.par")

				' Add new structural frame.
				structuralFrame = structuralFrames.Add(PartFileName:= filename, NumPaths:= lineSegmentList.Count, Path:= lineSegmentList.ToArray())

				' Close the Frame environment.
				application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyEnvironmentsExit)

				' Get a reference to the ActiveSelectSet.
				selectSet = application.ActiveSelectSet

				' Add the StructuralFrame to the select set.
				selectSet.Add(structuralFrame)

				' Switch to ISO view.
				application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyViewISOView)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
