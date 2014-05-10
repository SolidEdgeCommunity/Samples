Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	Friend Partial NotInheritable Class ProgId

		Private Sub New()
		End Sub

		Public Const Application As String = "SolidEdge.Application"
		Public Const AssemblyDocument As String = "SolidEdge.AssemblyDocument"
		Public Const ConfigFileExtension As String = "SolidEdge.ConfigFileExtension"
		Public Const FamilyOfAssembliesDocument As String = "SolidEdge.FamilyOfAssembliesDocument"
		Public Const SynchronousAssemblyDocument As String = "SolidEdge.DMAssemblyDocument"
		Public Const DraftDocument As String = "SolidEdge.DraftDocument"
		Public Const MarkupDocument As String = "SmartView.SEMarkupDocument"
		Public Const PartDocument As String = "SolidEdge.PartDocument"
		Public Const SheetMetalDocument As String = "SolidEdge.SheetMetalDocument"
		Public Const SynchronousPartDocument As String = "SolidEdge.DMPartDocument"
		Public Const SynchronousSheetMetalDocument As String = "SolidEdge.DMSheetMetalDocument"
		Public Const WeldmentDocument As String = "SolidEdge.WeldmentDocument"
	End Class
End Namespace
