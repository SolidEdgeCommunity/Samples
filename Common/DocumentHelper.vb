Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class DocumentHelper
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
                NewName = System.IO.Path.ChangeExtension(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), document.Name), ".jt")
                document.SaveAsJT(NewName, Include_PreciseGeom, Prod_Structure_Option, Export_PMI, Export_CoordinateSystem, Export_3DBodies, NumberofLODs, JTFileUnit, Write_Which_Files, Use_Simplified_TopAsm, Use_Simplified_SubAsm, Use_Simplified_Part, EnableDefaultOutputPath, IncludeSEProperties, Export_VisiblePartsOnly, Export_VisibleConstructionsOnly, RemoveUnsafeCharacters, ExportSEPartFileAsSingleJTFile)
            Case Else
                Throw New System.Exception(String.Format("'{0}' cannot be converted to JT.", document.Type))
        End Select
    End Sub
End Class