Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class InteropProxy
	Inherits MarshalByRefObject

	Private _readOnly As Boolean
	Private _propertySets As SolidEdgeFileProperties.PropertySets

	Public Sub Close()
		If _propertySets IsNot Nothing Then
			_propertySets.Close()
		Else
			Throw New InvalidOperationException("File is not open.")
		End If
	End Sub

	Public Sub Close(ByVal saveChanges As Boolean)
		If _propertySets IsNot Nothing Then
			If saveChanges Then
				_propertySets.Save()
			End If

			_propertySets.Close()
		Else
			Throw New InvalidOperationException("File is not open.")
		End If
	End Sub

	Public Function GetProperties() As PropertyMetadata()
		Dim list As New List(Of PropertyMetadata)()

		' Loop through the properties and create a PropertyMetadata array to return.
		For i As Integer = 0 To _propertySets.Count - 1
			Dim properties = DirectCast(_propertySets(i), SolidEdgeFileProperties.Properties)

			For j As Integer = 0 To properties.Count - 1
				Dim [property] = DirectCast(properties(j), SolidEdgeFileProperties.Property)
				Dim metadata = PropertyMetadata.FromProperty(properties.Name, [property])
				list.Add(metadata)
			Next j
		Next i

		Return list.ToArray()
	End Function

	Public Sub Open(ByVal path As String, ByVal [readOnly] As Boolean)
		_readOnly = [readOnly]
		_propertySets = New SolidEdgeFileProperties.PropertySets()
		_propertySets.Open(path, _readOnly)
	End Sub

	Public Sub Save()
		If _propertySets IsNot Nothing Then
			If _readOnly = False Then
				_propertySets.Save()
			Else
				Throw New System.Exception("File is read only.")
			End If
		Else
			Throw New InvalidOperationException("File is not open.")
		End If
	End Sub

	Public Sub SetProperty(ByVal metadata As PropertyMetadata)
		Dim properties = DirectCast(_propertySets(0), SolidEdgeFileProperties.Properties)
		'var properties = (SolidEdgeFileProperties.Properties)_propertySets[metadata.PropertySetName];
		Dim [property] = DirectCast(properties(metadata.PropertyName), SolidEdgeFileProperties.Property)
		[property].Value = metadata.PropertyValue
	End Sub
End Class
