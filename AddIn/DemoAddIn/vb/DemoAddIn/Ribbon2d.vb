Imports SolidEdgeCommunity.AddIn
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Ribbon2d
    Inherits SolidEdgeCommunity.AddIn.Ribbon

    Private Const _embeddedResourceName As String = "DemoAddIn.Ribbon2d.xml"

    Public Sub New()
        ' Get a reference to the current assembly. This is where the ribbon XML is embedded.
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly()

        ' In this example, XML file must have a build action of "Embedded Resource".
        Me.LoadXml(assembly, _embeddedResourceName)
    End Sub

    Public Overrides Sub OnControlClick(ByVal control As RibbonControl)
    End Sub
End Class
