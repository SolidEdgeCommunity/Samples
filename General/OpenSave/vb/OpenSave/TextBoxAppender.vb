Imports log4net.Appender
Imports log4net.Core
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Public Class TextBoxAppender
    Inherits AppenderSkeleton

    Private _textBox As TextBox

    Public Sub New()
    End Sub

    Protected Overrides Sub Append(ByVal loggingEvent As LoggingEvent)
        If _textBox IsNot Nothing Then
            _textBox.Do(Sub(t) t.AppendText(String.Format("{0}{1}", loggingEvent.RenderedMessage, Environment.NewLine)))
        End If
    End Sub

    Public Property TextBox() As TextBox
        Get
            Return _textBox
        End Get
        Set(ByVal value As TextBox)
            _textBox = value
        End Set
    End Property
End Class
