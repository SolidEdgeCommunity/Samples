Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

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

        If document Is Nothing Then
            Throw New ArgumentNullException("document")
        End If

        ' Get a reference to the Variables collection.
        variables = DirectCast(document.Variables, SolidEdgeFramework.Variables)

        ' Get a reference to the variablelist.
        variableList = DirectCast(variables.Query(pFindCriterium:= "*", NamedBy:= SolidEdgeConstants.VariableNameBy.seVariableNameByBoth, VarType:= SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth), SolidEdgeFramework.VariableList)

        ' Process variables.
        For Each variableListItem In variableList.OfType(Of Object)()
            ' Not used in this sample but a good example of how to get the runtime type.
            Dim variableListItemType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(variableListItem)

            ' Use helper class to get the object type.
            Dim objectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgeFramework.ObjectType)(variableListItem, "Type", CType(0, SolidEdgeFramework.ObjectType))

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
        Next variableListItem
    End Sub
End Class