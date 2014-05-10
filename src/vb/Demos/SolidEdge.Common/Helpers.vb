Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text

Namespace SolidEdge.Common
	Public NotInheritable Class ApplicationHelper

		Private Sub New()
		End Sub

		''' <summary>
		''' Creates and returns a new instance of Solid Edge.
		''' </summary>
		''' <returns>
		''' An object of type SolidEdgeFramework.Application.
		''' </returns>
		Public Shared Function Start() As SolidEdgeFramework.Application
			' On a system where Solid Edge is installed, the COM ProgID will be
			' defined in registry: HKEY_CLASSES_ROOT\SolidEdge.Application
			Dim t As Type = Type.GetTypeFromProgID(progID:= "SolidEdge.Application", throwOnError:= True)

			' Using the discovered Type, create and return a new instance of Solid Edge.
			Return DirectCast(Activator.CreateInstance(type:= t), SolidEdgeFramework.Application)
		End Function

		''' <summary>
		''' Connects to a running instance of Solid Edge.
		''' </summary>
		''' <returns>
		''' An object of type SolidEdgeFramework.Application.
		''' </returns>
		Public Shared Function Connect() As SolidEdgeFramework.Application
			Return Connect(startIfNotRunning:= False)
		End Function

		''' <summary>
		''' Connects to or starts a new instance of Solid Edge.
		''' </summary>
		''' <param name="startIfNotRunning"></param>
		''' <returns>
		''' An object of type SolidEdgeFramework.Application.
		''' </returns>
		Public Shared Function Connect(ByVal startIfNotRunning As Boolean) As SolidEdgeFramework.Application
			Try
				' Attempt to connect to a running instance of Solid Edge.
				Return DirectCast(Marshal.GetActiveObject(progID:= "SolidEdge.Application"), SolidEdgeFramework.Application)
			Catch ex As System.Runtime.InteropServices.COMException
				Select Case ex.ErrorCode
					' Solid Edge is not running.
					Case NativeMethods.MK_E_UNAVAILABLE
						If startIfNotRunning Then
							' Start Solid Edge.
							Return Start()
						Else
							' Rethrow exception.
							Throw
						End If
					Case Else
						' Rethrow exception.
						Throw
				End Select
			Catch
				' Rethrow exception.
				Throw
			End Try
		End Function

		''' <summary>
		''' Connects to or starts a new instance of Solid Edge.
		''' </summary>
		''' <param name="startIfNotRunning"></param>
		''' <param name="ensureVisible"></param>
		''' <returns>
		''' An object of type SolidEdgeFramework.Application.
		''' </returns>
		Public Shared Function Connect(ByVal startIfNotRunning As Boolean, ByVal ensureVisible As Boolean) As SolidEdgeFramework.Application
			Dim application As SolidEdgeFramework.Application = Nothing

			Try
				' Attempt to connect to a running instance of Solid Edge.
				application = DirectCast(Marshal.GetActiveObject(progID:= "SolidEdge.Application"), SolidEdgeFramework.Application)
			Catch ex As System.Runtime.InteropServices.COMException
				Select Case ex.ErrorCode
					' Solid Edge is not running.
					Case NativeMethods.MK_E_UNAVAILABLE
						If startIfNotRunning Then
							' Start Solid Edge.
							application = Start()
							Exit Select
						Else
							' Rethrow exception.
							Throw
						End If
					Case Else
						' Rethrow exception.
						Throw
				End Select
			Catch
				' Rethrow exception.
				Throw
			End Try

			If (application IsNot Nothing) AndAlso (ensureVisible) Then
				application.Visible = True
			End If

			Return application
		End Function
	End Class

	Public Class IDispatchHelper
		Private Const LOCALE_SYSTEM_DEFAULT As Integer = 2048

		''' <summary>
		''' Using IDispatch, determine the managed type of the specified object.
		''' </summary>
		''' <param name="o"></param>
		''' <returns></returns>
		Public Shared Function GetManagedType(ByVal o As Object) As Type
			Dim type As Type = Nothing
			Dim dispatch As IDispatch = TryCast(o, IDispatch)
			Dim typeInfo As ITypeInfo = Nothing
			Dim pTypeAttr As IntPtr = IntPtr.Zero
			Dim typeAttr As System.Runtime.InteropServices.ComTypes.TYPEATTR = Nothing

			Try
				If dispatch IsNot Nothing Then
					typeInfo = dispatch.GetTypeInfo(0, LOCALE_SYSTEM_DEFAULT)
					typeInfo.GetTypeAttr(pTypeAttr)
					typeAttr = DirectCast(Marshal.PtrToStructure(pTypeAttr, GetType(System.Runtime.InteropServices.ComTypes.TYPEATTR)), System.Runtime.InteropServices.ComTypes.TYPEATTR)

					' Type can technically be defined in any loaded assembly.
					Dim assemblies() As System.Reflection.Assembly = AppDomain.CurrentDomain.GetAssemblies()

					' Scan each assembly for a type with a matching GUID.
					For Each assembly As System.Reflection.Assembly In assemblies
						type = assembly.GetTypes().Where(Function(x) x.GUID.Equals(typeAttr.guid)).FirstOrDefault()

						If type IsNot Nothing Then
							' Found what we're looking for so break out of the loop.
							Exit For
						End If
					Next assembly
				End If
			Catch
			Finally
				If typeInfo IsNot Nothing Then
					typeInfo.ReleaseTypeAttr(pTypeAttr)
					Marshal.ReleaseComObject(typeInfo)
				End If
			End Try

			Return type
		End Function
	End Class

	Public Class InstallDataHelper
		Public Shared Function GetInstalledPath() As String
			' Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. 
			Dim programDirectory As New DirectoryInfo(GetProgramFolderPath())

			' Get path to Solid Edge installation directory.  Typically, 'C:\Program Files\Solid Edge XXX'. 
			Dim installationDirectory As DirectoryInfo = programDirectory.Parent

			Return installationDirectory.FullName
		End Function

		Public Shared Function GetProgramFolderPath() As String
			Dim installData As New SEInstallDataLib.SEInstallData()

			' Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. 
			Return installData.GetInstalledPath()
		End Function

		Public Shared Function GetTrainingFolderPath() As String
			' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'. 
			Dim trainingDirectory As New DirectoryInfo(Path.Combine(GetInstalledPath(), "Training"))

			Return trainingDirectory.FullName
		End Function

		Public Shared Function GetVersion() As Version
			Dim installData As New SEInstallDataLib.SEInstallData()

			Return New Version(installData.GetMajorVersion(), installData.GetMinorVersion(), installData.GetServicePackVersion(), installData.GetBuildNumber())
		End Function
	End Class

	Public NotInheritable Class ReflectionHelper

		Private Sub New()
		End Sub

		''' <summary>
		''' Returns the Solid Edge object type by invoking the 'Name' property.
		''' </summary>
		''' <param name="o"></param>
		''' <returns>System.String</returns>
		Public Shared Function GetPropertyValueAsString(ByVal o As Object, ByVal propertyName As String) As String
			' Using .NET reflection, attempt to obtain the Name value.
			Dim val = o.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, Nothing, o, Nothing)

			Return val.ToString()
		End Function

		''' <summary>
		''' Returns the Solid Edge object type by invoking the 'Type' property.
		''' </summary>
		''' <param name="o"></param>
		''' <returns>SolidEdgeFramework.ObjectType</returns>
		Public Shared Function GetObjectType(ByVal o As Object) As SolidEdgeFramework.ObjectType
			' Using .NET reflection, attempt to obtain the Type value.
			Dim val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, Nothing, o, Nothing)

			Return CType(val, SolidEdgeFramework.ObjectType)
		End Function

		''' <summary>
		''' Returns the Solid Edge Part feature modeling mode by invoking the 'ModelingModeType' property.
		''' </summary>
		''' <param name="o"></param>
		''' <returns>SolidEdgePart.ModelingModeConstants</returns>
		Public Shared Function GetPartFeatureModelingMode(ByVal o As Object) As SolidEdgePart.ModelingModeConstants
			' Using .NET reflection, attempt to obtain the ModelingModeType value.
			Dim val = o.GetType().InvokeMember("ModelingModeType", BindingFlags.GetProperty, Nothing, o, Nothing)

			Return CType(val, SolidEdgePart.ModelingModeConstants)
		End Function

		''' <summary>
		''' Returns the Solid Edge Part feature type by invoking the 'Type' property.
		''' </summary>
		''' <param name="o"></param>
		''' <returns>SolidEdgePart.FeatureTypeConstants</returns>
		Public Shared Function GetPartFeatureType(ByVal o As Object) As SolidEdgePart.FeatureTypeConstants
			' Using .NET reflection, attempt to obtain the Type value.
			Dim val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, Nothing, o, Nothing)

			Return CType(val, SolidEdgePart.FeatureTypeConstants)
		End Function

		''' <summary>
		''' Returns the Solid Edge value of the object by invoking the 'Value' property.
		''' </summary>
		''' <param name="o"></param>
		''' <returns></returns>
		Public Shared Function GetObjectValue(ByVal o As Object) As Object
			' Using .NET reflection, attempt to obtain the Value value.
			Return o.GetType().InvokeMember("Value", BindingFlags.GetProperty, Nothing, o, Nothing)
		End Function
	End Class
End Namespace
