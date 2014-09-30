using ClariusLabs.NuDoc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ApiDemos
{
    partial class MainForm : Form
    {
        private List<MethodInfo> _samplesList = new List<MethodInfo>();
        private TextBoxConsole _textBoxConsole;
        private AssemblyMembers _assemblyMembers;

        public MainForm()
        {
            this.Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _textBoxConsole = new TextBoxConsole(outputTextBox);
            _assemblyMembers = DocReader.Read(System.Reflection.Assembly.GetExecutingAssembly());
            
            LoadTreeView();
        }

        private void closeAllDocumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type type = typeof(ApiDemos.Application.CloseAllDocuments);
            MethodInfo method = type.GetMethod("RunSample", BindingFlags.Static | BindingFlags.NonPublic);

            List<object> parameters = new List<object>();

            // Set breakOnStart parameter.
            parameters.Add(buttonBreakpoint.Checked);

            object[] arguments = { method, parameters.ToArray() };
            backgroundWorker.RunWorkerAsync(arguments);

            //InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
        }

        private void closeAllDocumentssilentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type type = typeof(ApiDemos.Application.CloseAllDocumentsSilent);
            MethodInfo method = type.GetMethod("RunSample", BindingFlags.Static | BindingFlags.NonPublic);

            List<object> parameters = new List<object>();

            // Set breakOnStart parameter.
            parameters.Add(buttonBreakpoint.Checked);

            object[] arguments = { method, parameters.ToArray() };
            backgroundWorker.RunWorkerAsync(arguments);

            //InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 1)
            {
                ListViewItem listViewItem = listView.SelectedItems[0];
                MethodInfo method = listViewItem.Tag as MethodInfo;

                if (method != null)
                {
                    List<object> parameters = new List<object>();

                    // Set breakOnStart parameter.
                    parameters.Add(buttonBreakpoint.Checked);

                    object[] arguments = { method, parameters.ToArray() };
                    backgroundWorker.RunWorkerAsync(arguments);

                    //InvokeExampleInSeparateAppDomain(method, parameters.ToArray());
                }
            }
        }

        private void buttonBreakpoint_Click(object sender, EventArgs e)
        {
            buttonBreakpoint.Checked = !buttonBreakpoint.Checked;
        }

        private void solidEdgeST6SDKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Resources.SolidEdgeSdkUrl);
            }
            catch
            {
            }
        }

        private void samplesForSolidEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Resources.SamplesForSolidEdgeUrl);
            }
            catch
            {
            }
        }

        private void interopForSolidEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Resources.InteropForSolidEdgeUrl);
            }
            catch
            {
            }
        }

        private void spyForSolidEdgeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Resources.SpyForSolidEdgeUrl);
            }
            catch
            {
            }
        }

        private void LoadTreeView()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                MethodInfo methodInfo = type.GetMethod("RunSample", BindingFlags.Static | BindingFlags.NonPublic);

                if (methodInfo != null)
                {
                    _samplesList.Add(methodInfo);
                }
            }

            _samplesList.Sort(delegate(MethodInfo x, MethodInfo y)
            {
                if (x.DeclaringType.Namespace.Equals(y.DeclaringType.Namespace))
                {
                    return (x.DeclaringType.Name.CompareTo(y.DeclaringType.Name));
                }
                else
                {
                    return (x.DeclaringType.Namespace.CompareTo(y.DeclaringType.Namespace));
                }
            });

            var groups = _samplesList.GroupBy(x => x.DeclaringType.Namespace);

            foreach (IGrouping<string, MethodInfo> group in groups)
            {
                string[] keys = group.Key.Split('.');
                TreeNode treeNode = null;

                for (int i = 1; i < keys.Length; i++)
                {
                    string key = keys[i].Replace("_", String.Empty);

                    TreeNodeCollection Nodes;

                    if (treeNode == null)
                    {
                        Nodes = treeView.Nodes;
                    }
                    else
                    {
                        Nodes = treeNode.Nodes;
                    }

                    treeNode = Nodes[key];
                    if (treeNode == null)
                    {
                        treeNode = Nodes.Add(key, key);
                    }

                }

                treeNode.Tag = group.ToArray();
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView.Items.Clear();
            sampleRichTextBox.Clear();

            if (e.Node != null)
            {
                MethodInfo[] methods = e.Node.Tag as MethodInfo[];

                if (methods != null)
                {
                    foreach (MethodInfo method in methods)
                    {
                        ListViewItem listViewItem = new ListViewItem(method.DeclaringType.Name);
                        listViewItem.ImageIndex = 2;
                        listViewItem.Tag = method;
                        listView.Items.Add(listViewItem);
                    }
                }
            }

            if (listView.Items.Count > 0)
            {
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            else
            {
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            sampleRichTextBox.Clear();

            if (listView.SelectedItems.Count == 1)
            {
                ListViewItem listViewtiem = listView.SelectedItems[0];
                MethodInfo method = listViewtiem.Tag as MethodInfo;

                if (method != null)
                {
                    sampleRichTextBox.AppendText(method.DeclaringType.FullName, FontStyle.Bold);
                    sampleRichTextBox.AppendText(Environment.NewLine);

                    string id = _assemblyMembers.IdMap.FindId(method.DeclaringType);
                    var elements = _assemblyMembers.Elements.ToArray().OfType<ClariusLabs.NuDoc.Class>();
                    var classElement = elements.Where(x => x.Id.Equals(id)).FirstOrDefault();

                    if (classElement != null)
                    {
                        var summary = classElement.Elements.OfType<ClariusLabs.NuDoc.Summary>().FirstOrDefault();
                        var remarks = classElement.Elements.OfType<ClariusLabs.NuDoc.Remarks>().FirstOrDefault();

                        if (summary != null)
                        {
                            sampleRichTextBox.AppendText(Environment.NewLine);
                            sampleRichTextBox.AppendText("Summary:", FontStyle.Bold);
                            sampleRichTextBox.AppendText(Environment.NewLine);
                            sampleRichTextBox.AppendText(summary.ToText());
                            sampleRichTextBox.AppendText(Environment.NewLine);
                        }

                        if (remarks != null)
                        {
                            sampleRichTextBox.AppendText(Environment.NewLine);
                            sampleRichTextBox.AppendText("Remarks:", FontStyle.Bold);
                            sampleRichTextBox.AppendText(Environment.NewLine);
                            sampleRichTextBox.AppendText(remarks.ToText());
                            sampleRichTextBox.AppendText(Environment.NewLine);
                        }
                    }
                }
            }
        }

        //private void InvokeExampleInSeparateAppDomain(MethodInfo method, object[] parameters)
        //{
        //    if (method != null)
        //    {
        //        outputTextBox.Clear();

        //        Type type = typeof(SampleProxy);

        //        AppDomain interopDomain = null;

        //        try
        //        {
        //            // Define thread.
        //            var thread = new System.Threading.Thread(() =>
        //            {
        //                // Thread specific try\catch.
        //                try
        //                {
        //                    // Create a custom AppDomain to do COM Interop.
        //                    interopDomain = AppDomain.CreateDomain(method.DeclaringType.Name);

        //                    // Create a new instance of InteropProxy in the isolated application domain.
        //                    var sampleProxy = interopDomain.CreateInstanceAndUnwrap(
        //                        type.Assembly.FullName,
        //                        type.FullName) as SampleProxy;

        //                    _textBoxConsole.WriteLine("Begin '{0}'", method.DeclaringType.Name);
        //                    _textBoxConsole.WriteLine();

        //                    // Invoke sample.
        //                    sampleProxy.RunSample(method, parameters, _textBoxConsole);

        //                    _textBoxConsole.WriteLine();
        //                    _textBoxConsole.WriteLine("End '{0}'", method.DeclaringType.Name);
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            });

        //            // Important! Set thread apartment state to STA.
        //            thread.SetApartmentState(System.Threading.ApartmentState.STA);

        //            // Start the thread.
        //            thread.Start();

        //            // Wait for the thead to finish.
        //            thread.Join();
        //        }
        //        catch (System.Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //        finally
        //        {
        //            if (interopDomain != null)
        //            {
        //                // Unload the Interop AppDomain. This will automatically free up any COM references.
        //                AppDomain.Unload(interopDomain);
        //            }
        //        }
        //    }
        //}

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Signal start of background worker.
            backgroundWorker.ReportProgress(0);

            // Unwrap arguments.
            object[] arguments = (object[])e.Argument;
            MethodInfo method = (MethodInfo)arguments[0];
            object[] parameters = (object[])arguments[1];

            Type type = typeof(SampleProxy);

            AppDomain interopDomain = null;

            try
            {
                // Define thread.
                var thread = new System.Threading.Thread(() =>
                {
                    // Thread specific try\catch.
                    try
                    {
                        // Create a custom AppDomain to do COM Interop.
                        interopDomain = AppDomain.CreateDomain(method.DeclaringType.Name);

                        // Create a new instance of InteropProxy in the isolated application domain.
                        var sampleProxy = interopDomain.CreateInstanceAndUnwrap(
                            type.Assembly.FullName,
                            type.FullName) as SampleProxy;

                        _textBoxConsole.WriteLine("Begin '{0}'", method.DeclaringType.Name);
                        _textBoxConsole.WriteLine();

                        // Invoke sample.
                        sampleProxy.RunSample(method, parameters, _textBoxConsole);

                        _textBoxConsole.WriteLine();
                        _textBoxConsole.WriteLine("End '{0}'", method.DeclaringType.Name);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });

                // Important! Set thread apartment state to STA.
                thread.SetApartmentState(System.Threading.ApartmentState.STA);

                // Start the thread.
                thread.Start();

                // Wait for the thead to finish.
                thread.Join();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (interopDomain != null)
                {
                    // Unload the Interop AppDomain. This will automatically free up any COM references.
                    AppDomain.Unload(interopDomain);
                }
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
            {
                fileToolStripMenuItem.Enabled = false;
                buttonRun.Enabled = false;
                buttonBreakpoint.Enabled = false;
                outputTextBox.Clear();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            fileToolStripMenuItem.Enabled = true;
            buttonRun.Enabled = true;
            buttonBreakpoint.Enabled = true;
        }
    }
}
