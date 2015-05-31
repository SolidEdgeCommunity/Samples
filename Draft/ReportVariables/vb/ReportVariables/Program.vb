Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim variables As SolidEdgeFramework.Variables = Nothing
        Dim variableList As SolidEdgeFramework.VariableList = Nothing
        Dim variable As SolidEdgeFramework.variable = Nothing
        Dim dimension As SolidEdgeFrameworkSupport.Dimension = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect()

            ' Get a reference to the active document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            ' Make sure we have a document.
            If draftDocument IsNot Nothing Then
                ' Get a reference to the Variables collection.
                variables = DirectCast(draftDocument.Variables, SolidEdgeFramework.Variables)

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
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
