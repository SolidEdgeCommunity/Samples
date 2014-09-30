Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

<Serializable> _
Public Structure PropertyMetadata
	Public Sub New(ByVal propertySetName As String, ByVal propertyName As String, ByVal propertyValue As Boolean)
		Me.PropertySetName = propertySetName
		Me.PropertyName = propertyName
		Me.PropertyValue = propertyValue
	End Sub

	Public Sub New(ByVal propertySetName As String, ByVal propertyName As String, ByVal propertyValue As Date)
		Me.PropertySetName = propertySetName
		Me.PropertyName = propertyName
		Me.PropertyValue = propertyValue
	End Sub

	Public Sub New(ByVal propertySetName As String, ByVal propertyName As String, ByVal propertyValue As Double)
		Me.PropertySetName = propertySetName
		Me.PropertyName = propertyName
		Me.PropertyValue = propertyValue
	End Sub

	Public Sub New(ByVal propertySetName As String, ByVal propertyName As String, ByVal propertyValue As Integer)
		Me.PropertySetName = propertySetName
		Me.PropertyName = propertyName
		Me.PropertyValue = propertyValue
	End Sub

	Public Sub New(ByVal propertySetName As String, ByVal propertyName As String, ByVal propertyValue As String)
		Me.PropertySetName = propertySetName
		Me.PropertyName = propertyName
		Me.PropertyValue = propertyValue
	End Sub

	Public Shared Function FromProperty(ByVal propertySetName As String, ByVal [property] As SolidEdgeFileProperties.Property) As PropertyMetadata
'INSTANT VB NOTE: The variable propertyValue was renamed since Visual Basic does not handle local variables named the same as class members well:
		Dim propertyValue_Renamed As Object = Nothing

		Try
			propertyValue_Renamed = [property].Value
		Catch
		End Try

		If propertyValue_Renamed IsNot Nothing Then
			Dim propertyType = [property].Value.GetType()

			If propertyType.Equals(GetType(Boolean)) Then
				Return New PropertyMetadata(propertySetName, [property].Name, CBool([property].Value))
			ElseIf propertyType.Equals(GetType(Date)) Then
				Return New PropertyMetadata(propertySetName, [property].Name, CDate([property].Value))
			ElseIf propertyType.Equals(GetType(Double)) Then
				Return New PropertyMetadata(propertySetName, [property].Name, CDbl([property].Value))
			ElseIf propertyType.Equals(GetType(Integer)) Then
				Return New PropertyMetadata(propertySetName, [property].Name, CInt(Math.Truncate([property].Value)))
			ElseIf propertyType.Equals(GetType(String)) Then
				Return New PropertyMetadata(propertySetName, [property].Name, CStr([property].Value))
			Else
				Throw New System.Exception("Unknown property type.")
			End If
		Else
			Return New PropertyMetadata(propertySetName, [property].Name, String.Empty)
		End If
	End Function

	Public ReadOnly PropertySetName As String
	Public ReadOnly PropertyName As String
	Public PropertyValue As Object
End Structure
