Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Part
	''' <summary>
	''' Creates a new part with a finite revolved protrusion.
	''' </summary>
	Friend Class CreateFiniteRevolvedProtrusion
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim model As SolidEdgePart.Model = Nothing
			Dim revolvedProtrusions As SolidEdgePart.RevolvedProtrusions = Nothing
			Dim revolvedProtrusion As SolidEdgePart.RevolvedProtrusion = Nothing
			Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the Documents collection.
				documents = application.Documents

				' Create a new PartDocument.
				partDocument = documents.AddPartDocument()

				' Always a good idea to give SE a chance to breathe.
				application.DoIdle()

				' Call helper method to create the actual geometry.
				model = PartHelper.CreateFiniteRevolvedProtrusion(partDocument)

				' Get a reference to the RevolvedProtrusions collection.
				revolvedProtrusions = model.RevolvedProtrusions

				' Get a reference to the new RevolvedProtrusion.
				revolvedProtrusion = revolvedProtrusions.Item(1)

				' Get a reference to the ActiveSelectSet.
				selectSet = application.ActiveSelectSet

				' Empty ActiveSelectSet.
				selectSet.RemoveAll()

				' Add new RevolvedProtrusion to ActiveSelectSet.
				selectSet.Add(revolvedProtrusion)

				' Switch to ISO view.
				application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
