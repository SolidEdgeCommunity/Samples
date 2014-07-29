Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace FileProperties.WinForm
	Partial Public Class MainForm
		Inherits Form

		Private _openFileDialog As New OpenFileDialog()
		Private interopDomain As AppDomain = Nothing
		Private interopProxy As InteropProxy = Nothing

		Public Sub New()
			InitializeComponent()

			_openFileDialog.Filter = "Solid Edge Assembly (*.asm)|*.asm|Solid Edge Draft (*.dft)|*.dft|Solid Edge Part (*.par)|*.par|Solid Edge SheetMetal (*.psm)|*.psm"
			_openFileDialog.ShowReadOnly = True
		End Sub

		Private Sub openToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles openToolStripMenuItem.Click
			If _openFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
				toolStripStatusLabel1.Text = _openFileDialog.FileName
				saveToolStripMenuItem.Enabled = Not _openFileDialog.ReadOnlyChecked

				If interopDomain IsNot Nothing Then
					closeToolStripMenuItem_Click(sender, e)
				End If

				If interopDomain Is Nothing Then
					' Create a custom AppDomain to do COM Interop.
					interopDomain = AppDomain.CreateDomain("Interop Domain")

					Dim proxyType As Type = GetType(InteropProxy)

					' Create a new instance of InteropProxy in the isolated application domain.
					Dim interopProxy As InteropProxy = TryCast(interopDomain.CreateInstanceAndUnwrap(proxyType.Assembly.FullName, proxyType.FullName), InteropProxy)

					interopProxy.Open(_openFileDialog.FileName, _openFileDialog.ReadOnlyChecked)

					' Get the list of properties.
					Dim properties = interopProxy.GetProperties()

					Dim items As New List(Of ListViewItem)()

					' Populate the ListView.
					For Each [property] In properties
						If listView1.Groups([property].PropertySetName) Is Nothing Then
							listView1.Groups.Add([property].PropertySetName, [property].PropertySetName)
						End If

						Dim item = New ListViewItem(listView1.Groups([property].PropertySetName))
						item.ImageIndex = 0
						item.Text = [property].PropertyName
						item.SubItems.Add(String.Format("{0}", [property].PropertyValue))
						items.Add(item)
					Next [property]

					listView1.Items.AddRange(items.ToArray())
					listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
				End If
			End If
		End Sub

		Private Sub closeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles closeToolStripMenuItem.Click
			toolStripStatusLabel1.Text = String.Empty
			listView1.Items.Clear()

			If interopProxy IsNot Nothing Then
				interopProxy.Close()
			End If

			interopProxy = Nothing
			AppDomain.Unload(interopDomain)
			interopDomain = Nothing
		End Sub

		Private Sub saveToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles saveToolStripMenuItem.Click
			If interopProxy IsNot Nothing Then
				Try
					interopProxy.Save()
				Catch ex As System.Exception
					MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error)
				End Try
			End If
		End Sub
	End Class
End Namespace
