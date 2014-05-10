Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading

Namespace ApiSamples
	Friend Class SampleProxy
		Inherits MarshalByRefObject

		Public Sub RunSample(ByVal method As MethodInfo, ByVal parameters() As Object, ByVal consoleOut As TextWriter)
			If consoleOut IsNot Nothing Then
				Console.SetOut(consoleOut)
			End If

			method.Invoke(Nothing, parameters)
		End Sub
	End Class
End Namespace
