Attribute VB_Name = "Module1"
Option Explicit

Sub Main()
    Dim objApplication As SolidEdgeFramework.Application
    Dim objDocument As SolidEdgeFramework.SolidEdgeDocument
    Dim objPropertySets As SolidEdgeFramework.PropertySets
    
On Error GoTo GetObjectError
    Set objApplication = GetObject(, "SolidEdge.Application")

On Error GoTo ActiveDocumentError
    Set objDocument = objApplication.ActiveDocument
    Set objPropertySets = objDocument.Properties

On Error GoTo GlobalError
    Call ModifyDocumentNumber(objPropertySets)
    Call ModifyRevisionNumber(objPropertySets)
    Call ModifyProjectName(objPropertySets)
    Call AddOrModifyCustomProperties(objPropertySets)
    Call ReportAllFileProperties(objPropertySets)
    
    ' Unremark to delete previously added custom properties.
    'Call DeleteCustomProperties(objPropertySets)
    
    GoTo ExitSub
    
GetObjectError:
    MsgBox "Solid Edge is not running.", vbCritical, "Error"
    GoTo ExitSub
    
ActiveDocumentError:
    MsgBox "No active document.", vbCritical, "Error"
    GoTo ExitSub

GlobalError:
    MsgBox Err.Description, vbCritical, "Error"
    GoTo ExitSub

ExitSub:
    Set objDocument = Nothing
    Set objApplication = Nothing
End Sub

Sub ModifyDocumentNumber(objPropertySets As SolidEdgeFramework.PropertySets)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    
    ' Get reference to the ProjectInformation property set.
    Set objProperties = objPropertySets.Item("ProjectInformation")
    Set objProperty = objProperties.Item("Document Number")
    objProperty.Value = "My Document Number"
    
    Set objProperty = Nothing
    Set objProperties = Nothing
End Sub

Sub ModifyRevisionNumber(objPropertySets As SolidEdgeFramework.PropertySets)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    
    ' Get reference to the ProjectInformation property set.
    Set objProperties = objPropertySets.Item("ProjectInformation")
    Set objProperty = objProperties.Item("Revision")
    objProperty.Value = "My Revision"
    
    Set objProperty = Nothing
    Set objProperties = Nothing
End Sub

Sub ModifyProjectName(objPropertySets As SolidEdgeFramework.PropertySets)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    
    ' Get reference to the ProjectInformation property set.
    Set objProperties = objPropertySets.Item("ProjectInformation")
    Set objProperty = objProperties.Item("Project Name")
    objProperty.Value = "My Project Name"
    
    Set objProperty = Nothing
    Set objProperties = Nothing
End Sub

Sub AddOrModifyCustomProperties(objPropertySets As SolidEdgeFramework.PropertySets)
    Call AddOrModifyCustomProperty(objPropertySets, "My Text", MonthName(Month(Date)))
    Call AddOrModifyCustomProperty(objPropertySets, "My Date", Date)
    Call AddOrModifyCustomProperty(objPropertySets, "My Long", 123456)
    Call AddOrModifyCustomProperty(objPropertySets, "My Double", CDbl(123456.789))
End Sub

Sub AddOrModifyCustomProperty(objPropertySets As SolidEdgeFramework.PropertySets, strName As String, vValue As Variant)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    
    ' Get reference to the Custom property set.
    Set objProperties = objPropertySets.Item("Custom")
    
    On Error Resume Next
    
    ' Check to see if the property exists.
    Set objProperty = objProperties.Item(strName)
    
    On Error GoTo 0
    
    If Not objProperty Is Nothing Then
        ' Property exists. Update it.
        objProperty.Value = vValue
    Else
        ' Property does not exist. Add it.
        Set objProperty = objProperties.Add(strName, vValue)
    End If
    
    Set objProperty = Nothing
    Set objProperties = Nothing
End Sub

Sub DeleteCustomProperties(objPropertySets As SolidEdgeFramework.PropertySets)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    Dim aryProperties() As SolidEdgeFramework.Property
    
    Set objProperties = objPropertySets.Item("Custom")
    
    For Each objProperty In objProperties
        If Left(objProperty.Name, 2) = "My" Then
            If Not (Not aryProperties) Then
                ReDim Preserve aryProperties(0 To UBound(aryProperties) + 1)
            Else
                ReDim aryProperties(0)
            End If
            
            Set aryProperties(UBound(aryProperties)) = objProperty
        End If
    Next
    
    Dim lngPosition As Integer
    For lngPosition = LBound(aryProperties) To UBound(aryProperties)
        Call aryProperties(lngPosition).Delete
    Next lngPosition
End Sub

Sub ReportAllFileProperties(objPropertySets As SolidEdgeFramework.PropertySets)
    Dim objProperties As SolidEdgeFramework.Properties
    Dim objProperty As SolidEdgeFramework.Property
    
    For Each objProperties In objPropertySets
        For Each objProperty In objProperties
            Call ReportProperty(objProperties, objProperty)
        Next
    Next
    
    Set objProperty = Nothing
    Set objProperties = Nothing
End Sub

Sub ReportProperty(objProperties As SolidEdgeFramework.Properties, objProperty As SolidEdgeFramework.Property)
    Dim strPropertySetName As String
    Dim strPropertyName As String
    Dim objPropertyType As Variant
    Dim objPropertyValue As Variant
    
    strPropertySetName = objProperties.Name
    strPropertyName = objProperty.Name
    objPropertyType = objProperty.Type
    objPropertyValue = objProperty.Value

    Select Case VarType(objPropertyValue)
        Case vbArray
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbArray)"
        Case vbBoolean
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbBoolean)"
        Case vbByte
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbByte)"
        Case vbCurrency
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbCurrency)"
        Case vbDataObject
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbDataObject)"
        Case vbDate
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbDate)"
        Case vbDecimal
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbDecimal)"
        Case vbDouble
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbDouble)"
        Case vbEmpty
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbEmpty)"
        Case vbError
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbError)"
        Case vbInteger
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbInteger)"
        Case vbLong
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbLong)"
        Case vbNull
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbNull)"
        Case vbObject
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbObject)"
        Case vbSingle
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbSingle)"
        Case vbString
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbString)"
        Case vbUserDefinedType
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbString)"
        Case vbVariant
            Debug.Print strPropertySetName + " | " + strPropertyName + " = '" + CStr(objPropertyValue) + "' (vbVariant)"
    End Select
End Sub

