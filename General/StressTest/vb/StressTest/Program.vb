Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace StressTest
    Friend Class Program
        Shared Sub Main(ByVal args() As String)
            ' Start Solid Edge.
            Dim application = SolidEdgeUtils.Connect(True, True)

            ' Disable (most) prompts.
            application.DisplayAlerts = False

            Dim process = System.Diagnostics.Process.GetProcessById(application.ProcessID)
            Dim initialHandleCount = process.HandleCount
            Dim initialWorkingSet = process.WorkingSet64
            Dim trainingFolderPath = SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath()

            For Each fileName In Directory.EnumerateFiles(trainingFolderPath, "*.*", SearchOption.AllDirectories)
                Dim extension = Path.GetExtension(fileName)

                If (extension.Equals(".asm", StringComparison.OrdinalIgnoreCase)) OrElse (extension.Equals(".dft", StringComparison.OrdinalIgnoreCase)) OrElse (extension.Equals(".par", StringComparison.OrdinalIgnoreCase)) OrElse (extension.Equals(".psm", StringComparison.OrdinalIgnoreCase)) OrElse (extension.Equals(".pwd", StringComparison.OrdinalIgnoreCase)) Then
                    Console.WriteLine("Opening & closing '{0}'", fileName)

                    Dim startHandleCount = process.HandleCount
                    Dim startWorkingSet = process.WorkingSet64

                    Using task = New IsolatedTask(Of OpenCloseTask)()
                        task.Proxy.Application = application
                        task.Proxy.DoOpenClose(fileName)
                    End Using

                    process.Refresh()

                    Dim endHandleCount = process.HandleCount
                    Dim endWorkingSet = process.WorkingSet64

                    Console.WriteLine("Handle count start" & ControlChars.Tab & ControlChars.Tab & "'{0}'", startHandleCount)
                    Console.WriteLine("Handle count end" & ControlChars.Tab & ControlChars.Tab & "'{0}'", endHandleCount)
                    Console.WriteLine("Handle count change" & ControlChars.Tab & ControlChars.Tab & "'{0}'", endHandleCount - startHandleCount)
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Handle count total change" & ControlChars.Tab & "'{0}'", endHandleCount - initialHandleCount)
                    Console.ForegroundColor = ConsoleColor.Gray
                    Console.WriteLine("Working set start" & ControlChars.Tab & ControlChars.Tab & "'{0}'", startWorkingSet)
                    Console.WriteLine("Working set end" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & "'{0}'", endWorkingSet)
                    Console.WriteLine("Working set change" & ControlChars.Tab & ControlChars.Tab & "'{0}'", endWorkingSet - startWorkingSet)
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine("Working set total change" & ControlChars.Tab & "'{0}'", endWorkingSet - initialWorkingSet)
                    Console.ForegroundColor = ConsoleColor.Gray


                    Console.WriteLine()
                End If
            Next fileName

            If application IsNot Nothing Then
                application.DisplayAlerts = True
            End If

#If DEBUG Then
            System.Diagnostics.Debugger.Break()
#End If
        End Sub
    End Class
End Namespace
