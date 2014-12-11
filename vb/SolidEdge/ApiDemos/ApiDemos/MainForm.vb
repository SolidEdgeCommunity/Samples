Imports ClariusLabs.NuDoc
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Drawing
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms

Partial Friend Class MainForm
	Inherits Form

	Private _samplesList As New List(Of MethodInfo)()
	Private _textBoxConsole As TextBoxConsole
	Private _assemblyMembers As AssemblyMembers

	Public Sub New()
		Me.Font = SystemFonts.MessageBoxFont
		InitializeComponent()
	End Sub

	Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
		Close()
	End Sub

	Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		_textBoxConsole = New TextBoxConsole(outputTextBox)
		_assemblyMembers = DocReader.Read(System.Reflection.Assembly.GetExecutingAssembly())

		LoadTreeView()
	End Sub

	Private Sub closeAllDocumentsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles closeAllDocumentsToolStripMenuItem.Click
        Dim type As Type = GetType(ApiDemos.Application.CloseAllDocuments)
        Dim method As MethodInfo = type.GetMethod("RunSample", BindingFlags.Static Or BindingFlags.NonPublic)

        Dim parameters As New List(Of Object)()

        ' Set breakOnStart parameter.
        parameters.Add(buttonBreakpoint.Checked)

        Dim arguments() As Object = {method, parameters.ToArray()}
        backgroundWorker.RunWorkerAsync(arguments)

        'InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
    End Sub

    Private Sub closeAllDocumentssilentToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles closeAllDocumentssilentToolStripMenuItem.Click
        Dim type As Type = GetType(ApiDemos.Application.CloseAllDocumentsSilent)
        Dim method As MethodInfo = type.GetMethod("RunSample", BindingFlags.Static Or BindingFlags.NonPublic)

        Dim parameters As New List(Of Object)()

        ' Set breakOnStart parameter.
        parameters.Add(buttonBreakpoint.Checked)

        Dim arguments() As Object = {method, parameters.ToArray()}
        backgroundWorker.RunWorkerAsync(arguments)

        'InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
    End Sub

	Private Sub buttonRun_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonRun.Click
		If listView.SelectedItems.Count = 1 Then
			Dim listViewItem As ListViewItem = listView.SelectedItems(0)
			Dim method As MethodInfo = TryCast(listViewItem.Tag, MethodInfo)

			If method IsNot Nothing Then
				Dim parameters As New List(Of Object)()

				' Set breakOnStart parameter.
				parameters.Add(buttonBreakpoint.Checked)

				Dim arguments() As Object = { method, parameters.ToArray() }
				backgroundWorker.RunWorkerAsync(arguments)

				'InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
			End If
		End If
	End Sub

	Private Sub buttonBreakpoint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonBreakpoint.Click
		buttonBreakpoint.Checked = Not buttonBreakpoint.Checked
	End Sub

	Private Sub solidEdgeST6SDKToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles solidEdgeST6SDKToolStripMenuItem.Click
		Try
			Process.Start(Resources.SolidEdgeSdkUrl)
		Catch
		End Try
	End Sub

	Private Sub samplesForSolidEdgeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles samplesForSolidEdgeToolStripMenuItem.Click
		Try
			Process.Start(Resources.SamplesForSolidEdgeUrl)
		Catch
		End Try
	End Sub

	Private Sub interopForSolidEdgeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles interopForSolidEdgeToolStripMenuItem.Click
		Try
			Process.Start(Resources.InteropForSolidEdgeUrl)
		Catch
		End Try
	End Sub

	Private Sub spyForSolidEdgeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles spyForSolidEdgeToolStripMenuItem.Click
		Try
			Process.Start(Resources.SpyForSolidEdgeUrl)
		Catch
		End Try
	End Sub

	Private Sub LoadTreeView()
		Dim assembly = System.Reflection.Assembly.GetExecutingAssembly()

		For Each type As Type In assembly.GetTypes()
			Dim methodInfo As MethodInfo = type.GetMethod("RunSample", BindingFlags.Static Or BindingFlags.NonPublic)

			If methodInfo IsNot Nothing Then
				_samplesList.Add(methodInfo)
			End If
		Next type

		_samplesList.Sort(Function(x As MethodInfo, y As MethodInfo)
			If x.DeclaringType.Namespace.Equals(y.DeclaringType.Namespace) Then
				Return (x.DeclaringType.Name.CompareTo(y.DeclaringType.Name))
			Else
				Return (x.DeclaringType.Namespace.CompareTo(y.DeclaringType.Namespace))
			End If
		End Function)

		Dim groups = _samplesList.GroupBy(Function(x) x.DeclaringType.Namespace)

		For Each group As IGrouping(Of String, MethodInfo) In groups
			Dim keys() As String = group.Key.Split("."c)
			Dim treeNode As TreeNode = Nothing

			For i As Integer = 1 To keys.Length - 1
				Dim key As String = keys(i).Replace("_", String.Empty)

				Dim Nodes As TreeNodeCollection

				If treeNode Is Nothing Then
					Nodes = treeView.Nodes
				Else
					Nodes = treeNode.Nodes
				End If

				treeNode = Nodes(key)
				If treeNode Is Nothing Then
					treeNode = Nodes.Add(key, key)
				End If

			Next i

			treeNode.Tag = group.ToArray()
		Next group
	End Sub

	Private Sub treeView_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles treeView.AfterSelect
		listView.Items.Clear()
		sampleRichTextBox.Clear()

		If e.Node IsNot Nothing Then
			Dim methods() As MethodInfo = TryCast(e.Node.Tag, MethodInfo())

			If methods IsNot Nothing Then
				For Each method As MethodInfo In methods
					Dim listViewItem As New ListViewItem(method.DeclaringType.Name)
					listViewItem.ImageIndex = 2
					listViewItem.Tag = method
					listView.Items.Add(listViewItem)
				Next method
			End If
		End If

		If listView.Items.Count > 0 Then
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
		Else
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
		End If
	End Sub

	Private Sub listView_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles listView.SelectedIndexChanged
		sampleRichTextBox.Clear()

		If listView.SelectedItems.Count = 1 Then
			Dim listViewtiem As ListViewItem = listView.SelectedItems(0)
			Dim method As MethodInfo = TryCast(listViewtiem.Tag, MethodInfo)

			If method IsNot Nothing Then
				sampleRichTextBox.AppendText(method.DeclaringType.FullName, FontStyle.Bold)
				sampleRichTextBox.AppendText(Environment.NewLine)

				Dim id As String = _assemblyMembers.IdMap.FindId(method.DeclaringType)
				Dim elements = _assemblyMembers.Elements.ToArray().OfType(Of ClariusLabs.NuDoc.Class)()
				Dim classElement = elements.Where(Function(x) x.Id.Equals(id)).FirstOrDefault()

				If classElement IsNot Nothing Then
					Dim summary = classElement.Elements.OfType(Of ClariusLabs.NuDoc.Summary)().FirstOrDefault()
					Dim remarks = classElement.Elements.OfType(Of ClariusLabs.NuDoc.Remarks)().FirstOrDefault()

					If summary IsNot Nothing Then
						sampleRichTextBox.AppendText(Environment.NewLine)
						sampleRichTextBox.AppendText("Summary:", FontStyle.Bold)
						sampleRichTextBox.AppendText(Environment.NewLine)
						sampleRichTextBox.AppendText(summary.ToText())
						sampleRichTextBox.AppendText(Environment.NewLine)
					End If

					If remarks IsNot Nothing Then
						sampleRichTextBox.AppendText(Environment.NewLine)
						sampleRichTextBox.AppendText("Remarks:", FontStyle.Bold)
						sampleRichTextBox.AppendText(Environment.NewLine)
						sampleRichTextBox.AppendText(remarks.ToText())
						sampleRichTextBox.AppendText(Environment.NewLine)
					End If
				End If
			End If
		End If
	End Sub

	'private void InvokeExampleInSeparateAppDomain(MethodInfo method, object[] parameters)
	'{
	'    if (method != null)
	'    {
	'        outputTextBox.Clear();

	'        Type type = typeof(SampleProxy);

	'        AppDomain interopDomain = null;

	'        try
	'        {
	'            // Define thread.
	'            var thread = new System.Threading.Thread(() =>
	'            {
	'                // Thread specific try\catch.
	'                try
	'                {
	'                    // Create a custom AppDomain to do COM Interop.
	'                    interopDomain = AppDomain.CreateDomain(method.DeclaringType.Name);

	'                    // Create a new instance of InteropProxy in the isolated application domain.
	'                    var sampleProxy = interopDomain.CreateInstanceAndUnwrap(
	'                        type.Assembly.FullName,
	'                        type.FullName) as SampleProxy;

	'                    _textBoxConsole.WriteLine("Begin '{0}'", method.DeclaringType.Name);
	'                    _textBoxConsole.WriteLine();

	'                    // Invoke sample.
	'                    sampleProxy.RunSample(method, parameters, _textBoxConsole);

	'                    _textBoxConsole.WriteLine();
	'                    _textBoxConsole.WriteLine("End '{0}'", method.DeclaringType.Name);
	'                }
	'                catch (System.Exception ex)
	'                {
	'                    Console.WriteLine(ex.Message);
	'                }
	'            });

	'            // Important! Set thread apartment state to STA.
	'            thread.SetApartmentState(System.Threading.ApartmentState.STA);

	'            // Start the thread.
	'            thread.Start();

	'            // Wait for the thead to finish.
	'            thread.Join();
	'        }
	'        catch (System.Exception ex)
	'        {
	'            Console.WriteLine(ex.Message);
	'        }
	'        finally
	'        {
	'            if (interopDomain != null)
	'            {
	'                // Unload the Interop AppDomain. This will automatically free up any COM references.
	'                AppDomain.Unload(interopDomain);
	'            }
	'        }
	'    }
	'}

	Private Sub backgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker.DoWork
		' Signal start of background worker.
		backgroundWorker.ReportProgress(0)

		' Unwrap arguments.
		Dim arguments() As Object = DirectCast(e.Argument, Object())
		Dim method As MethodInfo = DirectCast(arguments(0), MethodInfo)
		Dim parameters() As Object = DirectCast(arguments(1), Object())

		Dim type As Type = GetType(SampleProxy)

		Dim interopDomain As AppDomain = Nothing

		Try
			' Define thread.
			Dim thread = New System.Threading.Thread(Sub()
				' Thread specific try\catch.
					' Create a custom AppDomain to do COM Interop.
					' Create a new instance of InteropProxy in the isolated application domain.
					' Invoke sample.
				Try
					interopDomain = AppDomain.CreateDomain(method.DeclaringType.Name)
					Dim sampleProxy = TryCast(interopDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName), SampleProxy)
					_textBoxConsole.WriteLine("Begin '{0}'", method.DeclaringType.Name)
					_textBoxConsole.WriteLine()
					sampleProxy.RunSample(method, parameters, _textBoxConsole)
					_textBoxConsole.WriteLine()
					_textBoxConsole.WriteLine("End '{0}'", method.DeclaringType.Name)
				Catch ex As System.Exception
					Console.WriteLine(ex.Message)
				End Try
			End Sub)

			' Important! Set thread apartment state to STA.
			thread.SetApartmentState(System.Threading.ApartmentState.STA)

			' Start the thread.
			thread.Start()

			' Wait for the thead to finish.
			thread.Join()
		Catch ex As System.Exception
			Console.WriteLine(ex.Message)
		Finally
			If interopDomain IsNot Nothing Then
				' Unload the Interop AppDomain. This will automatically free up any COM references.
				AppDomain.Unload(interopDomain)
			End If
		End Try
	End Sub

	Private Sub backgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles backgroundWorker.ProgressChanged
		If e.ProgressPercentage = 0 Then
			fileToolStripMenuItem.Enabled = False
			buttonRun.Enabled = False
			buttonBreakpoint.Enabled = False
			outputTextBox.Clear()
		End If
	End Sub

	Private Sub backgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles backgroundWorker.RunWorkerCompleted
		fileToolStripMenuItem.Enabled = True
		buttonRun.Enabled = True
		buttonBreakpoint.Enabled = True
	End Sub
End Class
