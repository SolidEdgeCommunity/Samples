Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text

Namespace SolidEdge.MouseEvents
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
End Namespace
