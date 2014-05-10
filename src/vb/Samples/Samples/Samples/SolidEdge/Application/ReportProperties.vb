Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	''' <summary>
	''' Reports the file properties of the active document.
	''' </summary>
	Friend Class ReportProperties
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim document As SolidEdgeFramework.SolidEdgeDocument = Nothing
			Dim propertySets As SolidEdgeFramework.PropertySets = Nothing
			Dim properties As SolidEdgeFramework.Properties = Nothing
			Dim [property] As SolidEdgeFramework.Property = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(True)

				' Make sure user can see the GUI.
				application.Visible = True

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the active document.
				document = application.TryActiveDocumentAs(Of SolidEdgeFramework.SolidEdgeDocument)()

				If document IsNot Nothing Then
					' Get a reference to the Properties collection.
					propertySets = DirectCast(document.Properties, SolidEdgeFramework.PropertySets)

					For i As Integer = 1 To propertySets.Count
						properties = propertySets.Item(i)

						Console.WriteLine("PropertSet '{0}'.", properties.Name)

						For j As Integer = 1 To properties.Count
							Dim nativePropertyType As System.Runtime.InteropServices.VarEnum = System.Runtime.InteropServices.VarEnum.VT_EMPTY
							Dim runtimePropertyType As Type = Nothing

							Dim value As Object = Nothing

							[property] = properties.Item(j)
							nativePropertyType = CType([property].Type, System.Runtime.InteropServices.VarEnum)

							' Accessing Value property may throw an exception...
							Try
								value = [property].get_Value()
							Catch ex As System.Exception
								value = ex.Message
							End Try

							If value IsNot Nothing Then
								runtimePropertyType = value.GetType()
							End If

							Console.WriteLine(ControlChars.Tab & "{0} = '{1}' ({2} | {3}).", [property].Name, value, nativePropertyType, runtimePropertyType)
						Next j

						Console.WriteLine()
					Next i
				Else
					Throw New System.Exception(Resources.NoActiveDocument)
				End If

			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
