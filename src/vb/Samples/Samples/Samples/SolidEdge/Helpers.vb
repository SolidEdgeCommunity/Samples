Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	Friend Class ApplicationHelper
		''' <summary>
		''' Creates and returns a new instance of Solid Edge.
		''' </summary>
		''' <returns>
		''' An object of type SolidEdgeFramework.Application.
		''' </returns>
		Public Shared Function Start() As SolidEdgeFramework.Application
			' On a system where Solid Edge is installed, the COM ProgID will be
			' defined in registry: HKEY_CLASSES_ROOT\SolidEdge.Application
			Dim t As Type = Type.GetTypeFromProgID(ProgId.Application, throwOnError:= True)

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
				Return DirectCast(Marshal.GetActiveObject(ProgId.Application), SolidEdgeFramework.Application)
			Catch ex As System.Runtime.InteropServices.COMException
				Select Case ex.ErrorCode
					' Solid Edge is not running.
					Case HRESULT.MK_E_UNAVAILABLE
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
				application = DirectCast(Marshal.GetActiveObject(ProgId.Application), SolidEdgeFramework.Application)
			Catch ex As System.Runtime.InteropServices.COMException
				Select Case ex.ErrorCode
					' Solid Edge is not running.
					Case HRESULT.MK_E_UNAVAILABLE
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

	Friend Class InstallDataHelper
		Friend Shared Function GetInstalledPath() As String
			' Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. 
			Dim programDirectory As New DirectoryInfo(GetProgramFolderPath())

			' Get path to Solid Edge installation directory.  Typically, 'C:\Program Files\Solid Edge XXX'. 
			Dim installationDirectory As DirectoryInfo = programDirectory.Parent

			Return installationDirectory.FullName
		End Function

		Friend Shared Function GetProgramFolderPath() As String
			Dim installData As New SEInstallDataLib.SEInstallData()

			' Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. 
			Return installData.GetInstalledPath()
		End Function

		Friend Shared Function GetTrainingFolderPath() As String
			' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'. 
			Dim trainingDirectory As New DirectoryInfo(Path.Combine(GetInstalledPath(), "Training"))

			Return trainingDirectory.FullName
		End Function
	End Class

	Friend NotInheritable Class ReflectionHelper

		Private Sub New()
		End Sub

		''' <summary>
		''' Returns the Solid Edge object type by invoking the 'Name' property.
		''' </summary>
		''' <param name="o"></param>
		''' <param name="propertyName"></param>
		''' <returns></returns>
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

	Friend Class SolidEdgeDocumentHelper
		Public Shared Sub SaveAsJT(ByVal document As SolidEdgeAssembly.AssemblyDocument)
			SaveAsJT(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub SaveAsJT(ByVal document As SolidEdgePart.PartDocument)
			SaveAsJT(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub SaveAsJT(ByVal document As SolidEdgePart.SheetMetalDocument)
			SaveAsJT(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub SaveAsJT(ByVal document As SolidEdgePart.WeldmentDocument)
			SaveAsJT(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub SaveAsJT(ByVal document As SolidEdgeFramework.SolidEdgeDocument)
			' Note: Some of the parameters are obvious by their name but we need to work on getting better descriptions for some.
			Dim NewName = String.Empty
			Dim Include_PreciseGeom = 0
			Dim Prod_Structure_Option = 1
			Dim Export_PMI = 0
			Dim Export_CoordinateSystem = 0
			Dim Export_3DBodies = 0
			Dim NumberofLODs = 1
			Dim JTFileUnit = 0
			Dim Write_Which_Files = 1
			Dim Use_Simplified_TopAsm = 0
			Dim Use_Simplified_SubAsm = 0
			Dim Use_Simplified_Part = 0
			Dim EnableDefaultOutputPath = 0
			Dim IncludeSEProperties = 0
			Dim Export_VisiblePartsOnly = 0
			Dim Export_VisibleConstructionsOnly = 0
			Dim RemoveUnsafeCharacters = 0
			Dim ExportSEPartFileAsSingleJTFile = 0

			If document Is Nothing Then
				Throw New ArgumentNullException("document")
			End If

			Select Case document.Type
				Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument, SolidEdgeFramework.DocumentTypeConstants.igPartDocument, SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument, SolidEdgeFramework.DocumentTypeConstants.igWeldmentAssemblyDocument, SolidEdgeFramework.DocumentTypeConstants.igWeldmentDocument
					NewName = Path.ChangeExtension(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), document.Name), ".jt")
					document.SaveAsJT(NewName, Include_PreciseGeom, Prod_Structure_Option, Export_PMI, Export_CoordinateSystem, Export_3DBodies, NumberofLODs, JTFileUnit, Write_Which_Files, Use_Simplified_TopAsm, Use_Simplified_SubAsm, Use_Simplified_Part, EnableDefaultOutputPath, IncludeSEProperties, Export_VisiblePartsOnly, Export_VisibleConstructionsOnly, RemoveUnsafeCharacters, ExportSEPartFileAsSingleJTFile)
				Case Else
					Throw New System.Exception(String.Format("'{0}' cannot be converted to JT.", document.Type))
			End Select
		End Sub
	End Class

	Friend Class VariablesHelper
		Public Shared Sub ReportVariables(ByVal document As SolidEdgeAssembly.AssemblyDocument)
			ReportVariables(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub ReportVariables(ByVal document As SolidEdgeDraft.DraftDocument)
			ReportVariables(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub ReportVariables(ByVal document As SolidEdgePart.PartDocument)
			ReportVariables(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub ReportVariables(ByVal document As SolidEdgePart.SheetMetalDocument)
			ReportVariables(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub ReportVariables(ByVal document As SolidEdgePart.WeldmentDocument)
			ReportVariables(DirectCast(document, SolidEdgeFramework.SolidEdgeDocument))
		End Sub

		Public Shared Sub ReportVariables(ByVal document As SolidEdgeFramework.SolidEdgeDocument)
			Dim variables As SolidEdgeFramework.Variables = Nothing
			Dim variableList As SolidEdgeFramework.VariableList = Nothing
			Dim variable As SolidEdgeFramework.variable = Nothing
			Dim dimension As SolidEdgeFrameworkSupport.Dimension = Nothing
			Dim variableListItem As Object = Nothing

			If document Is Nothing Then
				Throw New ArgumentNullException("document")
			End If

			' Get a reference to the Variables collection.
			variables = DirectCast(document.Variables, SolidEdgeFramework.Variables)

			' Get a reference to the variablelist.
			variableList = DirectCast(variables.Query(pFindCriterium:= "*", NamedBy:= SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, VarType:= SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth), SolidEdgeFramework.VariableList)

			' Process variables.
			For i As Integer = 1 To variableList.Count
				' Get a reference to variable item.
				variableListItem = variableList.Item(i)

				' Use ReflectionHelper class to get the object type.
				Dim objectType As SolidEdgeFramework.ObjectType = ReflectionHelper.GetObjectType(variableListItem)

				' Process the specific variable item type.
				Select Case objectType
					Case SolidEdgeFramework.ObjectType.igDimension
						' Get a reference to the dimension.
						dimension = DirectCast(variableListItem, SolidEdgeFrameworkSupport.Dimension)
						Console.WriteLine("Dimension: '{0}' = '{1}' ({2})", dimension.DisplayName, dimension.Value, objectType)
					Case SolidEdgeFramework.ObjectType.igVariable
						variable = DirectCast(variableListItem, SolidEdgeFramework.variable)
						Console.WriteLine("Variable: '{0}' = '{1}' ({2})", variable.DisplayName, variable.Value, objectType)
					Case Else
						' Other SolidEdgeConstants.ObjectType's may exist.
				End Select
			Next i
		End Sub
	End Class

	Friend Class WindowHelper
		Public Const SRCCOPY As Integer = &HCC0020

		<DllImport("gdi32.dll")> _
		Shared Function BitBlt(ByVal hObject As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hObjectSource As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As Integer) As Boolean
		End Function

		<DllImport("gdi32.dll")> _
		Shared Function CreateCompatibleBitmap(ByVal hDC As IntPtr, ByVal nWidth As Integer, ByVal nHeight As Integer) As IntPtr
		End Function

		<DllImport("gdi32.dll")> _
		Shared Function CreateCompatibleDC(ByVal hDC As IntPtr) As IntPtr
		End Function

		<DllImport("gdi32.dll")> _
		Shared Function DeleteDC(ByVal hDC As IntPtr) As Boolean
		End Function

		<DllImport("gdi32.dll")> _
		Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
		End Function

		<DllImport("gdi32.dll")> _
		Shared Function SelectObject(ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
		End Function

		<DllImport("User32.dll")> _
		Shared Function GetDC(ByVal hWnd As IntPtr) As IntPtr
		End Function

		<DllImport("User32.dll")> _
		Shared Function ReleaseDC(ByVal hWnd As System.IntPtr, ByVal hDC As System.IntPtr) As Integer
		End Function

		<DllImport("user32.dll")> _
		Shared Function GetClientRect(ByVal hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
		End Function

        <StructLayout(LayoutKind.Sequential)> _
        Public Structure RECT
            Public left As Integer
            Public top As Integer
            Public right As Integer
            Public bottom As Integer
        End Structure

		Public Shared Sub SaveAsImage(ByVal window As SolidEdgeFramework.Window)
			Dim extensions() As String = { ".jpg", ".bmp", ".tif" }

			Dim view As SolidEdgeFramework.View = Nothing
			Dim guid As Guid = System.Guid.NewGuid()
			Dim folder As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)

			If window Is Nothing Then
				Throw New ArgumentNullException("window")
			End If

			' Get a reference to the 3D view.
			view = window.View

			' Save each extension.
			For Each extension As String In extensions
				' File saved to desktop.
				Dim filename As String = Path.ChangeExtension(guid.ToString(), extension)
				filename = Path.Combine(folder, filename)

				Dim resolution As Double = 1.0 ' DPI - Larger values have better quality but also lead to larger file.
				Dim colorDepth As Integer = 24
				Dim width As Integer = window.UsableWidth
				Dim height As Integer = window.UsableHeight

				' You can specify .bmp (Windows Bitmap), .tif (TIFF), or .jpg (JPEG).
				view.SaveAsImage(Filename:= filename, Width:= width, Height:= height, AltViewStyle:= Nothing, Resolution:= resolution, ColorDepth:= colorDepth, ImageQuality:= SolidEdgeFramework.SeImageQualityType.seImageQualityHigh, Invert:= False)

				Console.WriteLine("Saved '{0}'.", filename)
			Next extension
		End Sub

		Public Shared Sub SaveAsImageUsingBitBlt(ByVal window As SolidEdgeFramework.Window)
			Dim dialog As New System.Windows.Forms.SaveFileDialog()
			dialog.FileName = Path.ChangeExtension(window.Caption, ".bmp")
			dialog.Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpeg)|*.jpeg|PNG (.png)|*.png|TIFF (.tiff)|*.tiff|WMF Image (.wmf)|*.wmf"
			dialog.FilterIndex = 1

			If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				Dim handle As New IntPtr(window.DrawHwnd)

				' Capture the window to an Image object.
				Using image As Image = Capture(handle)
					Dim imageFormat As ImageFormat = Nothing

					' Determine the selected image format.
					' The index is 1-based.
					Select Case dialog.FilterIndex
						Case 1
							imageFormat = System.Drawing.Imaging.ImageFormat.Bmp
						Case 2
							imageFormat = System.Drawing.Imaging.ImageFormat.Gif
						Case 3
							imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg
						Case 4
							imageFormat = System.Drawing.Imaging.ImageFormat.Png
						Case 5
							imageFormat = System.Drawing.Imaging.ImageFormat.Tiff
						Case 6
							imageFormat = System.Drawing.Imaging.ImageFormat.Wmf
					End Select

					Console.WriteLine("Saving {0}.", dialog.FileName)

					image.Save(dialog.FileName, imageFormat)
				End Using
			End If
		End Sub

		Public Shared Function Capture(ByVal hWnd As IntPtr) As Image
			' Get the device context of the client.
			Dim hdcSrc As IntPtr = GetDC(hWnd)

			' Get the client rectangle.
			Dim windowRect As New RECT()
			GetClientRect(hWnd, windowRect)

			' Calculate the size of the window.
			Dim width As Integer = windowRect.right - windowRect.left
			Dim height As Integer = windowRect.bottom - windowRect.top

			' Create a new device context that is compatible with the source device context.
			Dim hdcDest As IntPtr = CreateCompatibleDC(hdcSrc)

			' Creates a bitmap compatible with the device that is associated with the specified device context.
			Dim hBitmap As IntPtr = CreateCompatibleBitmap(hdcSrc, width, height)

			' Select the new bitmap object into the destination device context.
			Dim hOld As IntPtr = SelectObject(hdcDest, hBitmap)

			' Perform a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
			BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY)

			' Restore selection.
			SelectObject(hdcDest, hOld)

			' Clean up device contexts.
			DeleteDC(hdcDest)
			ReleaseDC(hWnd, hdcSrc)

			' Create an Image from a handle to a GDI bitmap.
			Dim image As Image = System.Drawing.Image.FromHbitmap(hBitmap)

			' Free up the Bitmap object.
			DeleteObject(hBitmap)

			Return image
		End Function
	End Class
End Namespace
