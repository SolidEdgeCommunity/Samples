Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	''' <summary>
	''' Creates an new document and adds multiple custom file properties.
	''' </summary>
	Friend Class AddCustomProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim document As SolidEdgeFramework.SolidEdgeDocument = Nothing
			Dim propertySets As SolidEdgeFramework.PropertySets = Nothing
			Dim properties As SolidEdgeFramework.Properties = Nothing
			Dim [property] As SolidEdgeFramework.Property = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the Documents collection.
				documents = application.Documents

				' Add a new part document.
				document = DirectCast(documents.AddPartDocument(), SolidEdgeFramework.SolidEdgeDocument)

				' Get a reference to the Properties collection.
				propertySets = DirectCast(document.Properties, SolidEdgeFramework.PropertySets)

				' Get a reference to the custom proprty set.
				properties = propertySets.Item("Custom")

				' Add string custom property.
				[property] = properties.Add("My String", "Hello world!")

				' Add integer custom property.
				[property] = properties.Add("My Integer", 338)

				' Add boolean custom property.
				[property] = properties.Add("My Boolean", True)

				' Add date custom property.
				[property] = properties.Add("My DateTime", Date.Now)

				' Show the file properties dialog.
				application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartFileProperties)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
