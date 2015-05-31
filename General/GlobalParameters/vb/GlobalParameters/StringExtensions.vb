Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

Public Module StringExtensions
    <System.Runtime.CompilerServices.Extension> _
    Public Function CamelCaseToWordString(ByVal str As String) As String
        Return Regex.Replace(Regex.Replace(str, "(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), "(\p{Ll})(\P{Ll})", "$1 $2")
    End Function
End Module
