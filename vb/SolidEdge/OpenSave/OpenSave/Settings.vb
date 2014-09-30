Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text

Public Class OpenSaveSettings
	Inherits MarshalByRefObject

	Private _applicationSettings As New ApplicationSettings()
	Private _assemblySettings As New AssemblySettings()
	Private _draftSettings As New DraftSettings()
	Private _partSettings As New PartSettings()
	Private _sheetMetalSettings As New SheetMetalSettings()
	Private _weldmentSettings As New WeldmentSettings()

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property Application() As ApplicationSettings
		Get
			Return _applicationSettings
		End Get
		Set(ByVal value As ApplicationSettings)
			_applicationSettings = value
		End Set
	End Property

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property Assembly() As AssemblySettings
		Get
			Return _assemblySettings
		End Get
		Set(ByVal value As AssemblySettings)
			_assemblySettings = value
		End Set
	End Property

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property Draft() As DraftSettings
		Get
			Return _draftSettings
		End Get
		Set(ByVal value As DraftSettings)
			_draftSettings = value
		End Set
	End Property

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property Part() As PartSettings
		Get
			Return _partSettings
		End Get
		Set(ByVal value As PartSettings)
			_partSettings = value
		End Set
	End Property

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property SheetMetal() As SheetMetalSettings
		Get
			Return _sheetMetalSettings
		End Get
		Set(ByVal value As SheetMetalSettings)
			_sheetMetalSettings = value
		End Set
	End Property

	<TypeConverter(GetType(ExpandableObjectConverter))> _
	Public Property Weldment() As WeldmentSettings
		Get
			Return _weldmentSettings
		End Get
		Set(ByVal value As WeldmentSettings)
			_weldmentSettings = value
		End Set
	End Property
End Class

Public Class ApplicationSettings
	Inherits MarshalByRefObject

	Private _disableAddins As Boolean
	Private _visible As Boolean = True

	Public Property DisableAddins() As Boolean
		Get
			Return _disableAddins
		End Get
		Set(ByVal value As Boolean)
			_disableAddins = value
		End Set
	End Property

	Public Property Visible() As Boolean
		Get
			Return _visible
		End Get
		Set(ByVal value As Boolean)
			_visible = value
		End Set
	End Property

	Public Overrides Function ToString() As String
		Return "Application Settings"
	End Function
End Class

Public Class AssemblySettings
	Inherits MarshalByRefObject

	Public Overrides Function ToString() As String
		Return "Assembly Settings"
	End Function
End Class

Public Class DraftSettings
	Inherits MarshalByRefObject

	Private _updateDrawingViews As Boolean

	Public Property UpdateDrawingViews() As Boolean
		Get
			Return _updateDrawingViews
		End Get
		Set(ByVal value As Boolean)
			_updateDrawingViews = value
		End Set
	End Property

	Public Overrides Function ToString() As String
		Return "Draft Settings"
	End Function
End Class

Public Class PartSettings
	Inherits MarshalByRefObject

	Public Overrides Function ToString() As String
		Return "Part Settings"
	End Function
End Class

Public Class SheetMetalSettings
	Inherits MarshalByRefObject

	Public Overrides Function ToString() As String
		Return "SheetMetal Settings"
	End Function
End Class

Public Class WeldmentSettings
	Inherits MarshalByRefObject

	Public Overrides Function ToString() As String
		Return "Weldment Settings"
	End Function
End Class
