Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Reflection
Imports System.Text

Public Class BatchPrintTask
    Inherits IsolatedTaskProxy

    Public Sub Print(ByVal filename As String, ByVal options As DraftPrintUtilityOptions)
        InvokeSTAThread(Of String, DraftPrintUtilityOptions)(AddressOf PrintInternal, filename, options)
    End Sub

    Private Sub PrintInternal(ByVal filename As String, ByVal options As DraftPrintUtilityOptions)
'INSTANT VB NOTE: The variable application was renamed since Visual Basic does not handle local variables named the same as class members well:
        Dim application_Renamed As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim draftPrintUtility As SolidEdgeDraft.DraftPrintUtility = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application_Renamed = SolidEdgeCommunity.SolidEdgeUtils.Connect(True)

            ' Make sure Solid Edge is visible.
            application_Renamed.Visible = True

            ' Get a reference to the Documents collection.
            documents = application_Renamed.Documents

            ' Get a reference to the DraftPrintUtility.
            draftPrintUtility = DirectCast(application_Renamed.GetDraftPrintUtility(), SolidEdgeDraft.DraftPrintUtility)

            ' Copy all of the settings from DraftPrintUtilityOptions to the DraftPrintUtility object.
            CopyOptions(draftPrintUtility, options)

            ' Open the document.
            draftDocument = DirectCast(documents.Open(filename), SolidEdgeDraft.DraftDocument)

            ' Give Solid Edge time to process.
            application_Renamed.DoIdle()

            ' Add the draft document to the queue.
            draftPrintUtility.AddDocument(draftDocument)

            ' Print out.
            draftPrintUtility.PrintOut()

            ' Cleanup queue.
            draftPrintUtility.RemoveAllDocuments()
        Catch
            Throw
        Finally
            ' Make sure we close the document.
            If draftDocument IsNot Nothing Then
                draftDocument.Close()
            End If

            SolidEdgeCommunity.OleMessageFilter.Register()
        End Try
    End Sub

    Private Sub CopyOptions(ByVal draftPrintUtility As SolidEdgeDraft.DraftPrintUtility, ByVal options As DraftPrintUtilityOptions)
        Dim fromType As Type = GetType(DraftPrintUtilityOptions)
        Dim toType As Type = GetType(SolidEdgeDraft.DraftPrintUtility)
        Dim properties() As PropertyInfo = toType.GetProperties().Where(Function(x) x.CanWrite).ToArray()

        ' Copy all of the properties from DraftPrintUtility to this object.
        For Each toProperty As PropertyInfo In properties
            ' Some properties may throw an exception if options are incompatible.
            ' For instance, if PrintToFile = false, setting PrintToFileName = "" will cause an exception.
            ' Mostly irrelevant but handle it as you see fit.
            Try
                Dim fromProperty As PropertyInfo = fromType.GetProperty(toProperty.Name)
                If fromProperty IsNot Nothing Then
                    Dim val As Object = fromProperty.GetValue(options, Nothing)

                    toType.InvokeMember(toProperty.Name, BindingFlags.SetProperty, Nothing, draftPrintUtility, New Object() { val })

                End If
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            End Try
        Next toProperty
    End Sub
End Class
