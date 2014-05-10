' Credit: mav.northwind
'http://www.codeproject.com/Articles/9196/Links-with-arbitrary-text-in-a-RichTextBox

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Namespace ApiSamples.Forms
	Friend Class RichTextBoxEx
		Inherits RichTextBox

		#Region "Interop-Defines"
		<StructLayout(LayoutKind.Sequential)> _
		Private Structure CHARFORMAT2_STRUCT
			Public cbSize As UInt32
			Public dwMask As UInt32
			Public dwEffects As UInt32
			Public yHeight As Int32
			Public yOffset As Int32
			Public crTextColor As Int32
			Public bCharSet As Byte
			Public bPitchAndFamily As Byte
			<MarshalAs(UnmanagedType.ByValArray, SizeConst := 32)> _
			Public szFaceName() As Char
			Public wWeight As UInt16
			Public sSpacing As UInt16
			Public crBackColor As Integer ' Color.ToArgb() -> int
			Public lcid As Integer
			Public dwReserved As Integer
			Public sStyle As Int16
			Public wKerning As Int16
			Public bUnderlineType As Byte
			Public bAnimation As Byte
			Public bRevAuthor As Byte
			Public bReserved1 As Byte
		End Structure

		<DllImport("user32.dll", CharSet := CharSet.Auto)> _
		Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
		End Function

		Private Const WM_USER As Integer = &H400
		Private Const EM_GETCHARFORMAT As Integer = WM_USER + 58
		Private Const EM_SETCHARFORMAT As Integer = WM_USER + 68

		Private Const SCF_SELECTION As Integer = &H1
		Private Const SCF_WORD As Integer = &H2
		Private Const SCF_ALL As Integer = &H4

		#Region "CHARFORMAT2 Flags"
		Private Const CFE_BOLD As UInt32 = &H1
		Private Const CFE_ITALIC As UInt32 = &H2
		Private Const CFE_UNDERLINE As UInt32 = &H4
		Private Const CFE_STRIKEOUT As UInt32 = &H8
		Private Const CFE_PROTECTED As UInt32 = &H10
		Private Const CFE_LINK As UInt32 = &H20
		Private Const CFE_AUTOCOLOR As UInt32 = &H40000000
		Private Const CFE_SUBSCRIPT As UInt32 = &H10000 ' Superscript and subscript are
		Private Const CFE_SUPERSCRIPT As UInt32 = &H20000 '  mutually exclusive

		Private Const CFM_SMALLCAPS As Integer = &H40 ' (*)
		Private Const CFM_ALLCAPS As Integer = &H80 ' Displayed by 3.0
		Private Const CFM_HIDDEN As Integer = &H100 ' Hidden by 3.0
		Private Const CFM_OUTLINE As Integer = &H200 ' (*)
		Private Const CFM_SHADOW As Integer = &H400 ' (*)
		Private Const CFM_EMBOSS As Integer = &H800 ' (*)
		Private Const CFM_IMPRINT As Integer = &H1000 ' (*)
		Private Const CFM_DISABLED As Integer = &H2000
		Private Const CFM_REVISED As Integer = &H4000

		Private Const CFM_BACKCOLOR As Integer = &H4000000
		Private Const CFM_LCID As Integer = &H2000000
		Private Const CFM_UNDERLINETYPE As Integer = &H800000 ' Many displayed by 3.0
		Private Const CFM_WEIGHT As Integer = &H400000
		Private Const CFM_SPACING As Integer = &H200000 ' Displayed by 3.0
		Private Const CFM_KERNING As Integer = &H100000 ' (*)
		Private Const CFM_STYLE As Integer = &H80000 ' (*)
		Private Const CFM_ANIMATION As Integer = &H40000 ' (*)
		Private Const CFM_REVAUTHOR As Integer = &H8000


		Private Const CFM_BOLD As UInt32 = &H1
		Private Const CFM_ITALIC As UInt32 = &H2
		Private Const CFM_UNDERLINE As UInt32 = &H4
		Private Const CFM_STRIKEOUT As UInt32 = &H8
		Private Const CFM_PROTECTED As UInt32 = &H10
		Private Const CFM_LINK As UInt32 = &H20
		Private Const CFM_SIZE As UInt32 = &H80000000L
		Private Const CFM_COLOR As UInt32 = &H40000000
		Private Const CFM_FACE As UInt32 = &H20000000
		Private Const CFM_OFFSET As UInt32 = &H10000000
		Private Const CFM_CHARSET As UInt32 = &H8000000
		Private Const CFM_SUBSCRIPT As UInt32 = CFE_SUBSCRIPT Or CFE_SUPERSCRIPT
		Private Const CFM_SUPERSCRIPT As UInt32 = CFM_SUBSCRIPT

		Private Const CFU_UNDERLINENONE As Byte = &H0
		Private Const CFU_UNDERLINE As Byte = &H1
		Private Const CFU_UNDERLINEWORD As Byte = &H2 ' (*) displayed as ordinary underline
		Private Const CFU_UNDERLINEDOUBLE As Byte = &H3 ' (*) displayed as ordinary underline
		Private Const CFU_UNDERLINEDOTTED As Byte = &H4
		Private Const CFU_UNDERLINEDASH As Byte = &H5
		Private Const CFU_UNDERLINEDASHDOT As Byte = &H6
		Private Const CFU_UNDERLINEDASHDOTDOT As Byte = &H7
		Private Const CFU_UNDERLINEWAVE As Byte = &H8
		Private Const CFU_UNDERLINETHICK As Byte = &H9
		Private Const CFU_UNDERLINEHAIRLINE As Byte = &HA ' (*) displayed as ordinary underline

		#End Region

		#End Region

		Public Sub New()
			' Otherwise, non-standard links get lost when user starts typing
			' next to a non-standard link
			Me.DetectUrls = False
		End Sub

		<DefaultValue(False)> _
		Public Shadows Property DetectUrls() As Boolean
			Get
				Return MyBase.DetectUrls
			End Get
			Set(ByVal value As Boolean)
				MyBase.DetectUrls = value
			End Set
		End Property

		Public Overloads Sub AppendText(ByVal text As String, ByVal fontStyle As FontStyle)
			SelectionStart = TextLength
			SelectionLength = 0

			SelectionFont = New Font(Font, fontStyle)
			AppendText(text)
			SelectionFont = Font
		End Sub

		Public Overloads Sub AppendText(ByVal text As String, ByVal color As Color, ByVal fontStyle As FontStyle)
			SelectionStart = TextLength
			SelectionLength = 0

			SelectionColor = color
			SelectionFont = New Font(Font, fontStyle)
			AppendText(text)
			SelectionColor = ForeColor
			SelectionFont = Font
		End Sub

		''' <summary>
		''' Insert a given text as a link into the RichTextBox at the current insert position.
		''' </summary>
		''' <param name="text">Text to be inserted</param>
		Public Sub InsertLink(ByVal text As String)
			InsertLink(text, Me.SelectionStart)
		End Sub

		''' <summary>
		''' Insert a given text at a given position as a link. 
		''' </summary>
		''' <param name="text">Text to be inserted</param>
		''' <param name="position">Insert position</param>
		Public Sub InsertLink(ByVal text As String, ByVal position As Integer)
			If position < 0 OrElse position > Me.Text.Length Then
				Throw New ArgumentOutOfRangeException("position")
			End If

			Me.SelectionStart = position
			Me.SelectedText = text
			Me.Select(position, text.Length)
			Me.SetSelectionLink(True)
			Me.Select(position + text.Length, 0)
		End Sub

		''' <summary>
		''' Insert a given text at at the current input position as a link.
		''' The link text is followed by a hash (#) and the given hyperlink text, both of
		''' them invisible.
		''' When clicked on, the whole link text and hyperlink string are given in the
		''' LinkClickedEventArgs.
		''' </summary>
		''' <param name="text">Text to be inserted</param>
		''' <param name="hyperlink">Invisible hyperlink string to be inserted</param>
		Public Sub InsertLink(ByVal text As String, ByVal hyperlink As String)
			InsertLink(text, hyperlink, Me.SelectionStart)
		End Sub

		''' <summary>
		''' Insert a given text at a given position as a link. The link text is followed by
		''' a hash (#) and the given hyperlink text, both of them invisible.
		''' When clicked on, the whole link text and hyperlink string are given in the
		''' LinkClickedEventArgs.
		''' </summary>
		''' <param name="text">Text to be inserted</param>
		''' <param name="hyperlink">Invisible hyperlink string to be inserted</param>
		''' <param name="position">Insert position</param>
		Public Sub InsertLink(ByVal text As String, ByVal hyperlink As String, ByVal position As Integer)
			If position < 0 OrElse position > Me.Text.Length Then
				Throw New ArgumentOutOfRangeException("position")
			End If

			Me.SelectionStart = position
			Me.SelectedRtf = "{\rtf1\ansi " & text & "\v #" & hyperlink & "\v0}"
			Me.Select(position, text.Length + hyperlink.Length + 1)
			Me.SetSelectionLink(True)
			Me.Select(position + text.Length + hyperlink.Length + 1, 0)
		End Sub

		''' <summary>
		''' Set the current selection's link style
		''' </summary>
		''' <param name="link">true: set link style, false: clear link style</param>
		Public Sub SetSelectionLink(ByVal link As Boolean)
			SetSelectionStyle(CFM_LINK Or CFM_BOLD,If(link, CFE_LINK Or CFE_BOLD, 0))
		End Sub
		''' <summary>
		''' Get the link style for the current selection
		''' </summary>
		''' <returns>0: link style not set, 1: link style set, -1: mixed</returns>
		Public Function GetSelectionLink() As Integer
			Return GetSelectionStyle(CFM_LINK, CFE_LINK)
		End Function


		Private Sub SetSelectionStyle(ByVal mask As UInt32, ByVal effect As UInt32)
			Dim cf As New CHARFORMAT2_STRUCT()
			cf.cbSize = CUInt(Marshal.SizeOf(cf))
			cf.dwMask = mask
			cf.dwEffects = effect

			Dim wpar As New IntPtr(SCF_SELECTION)
			Dim lpar As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf))
			Marshal.StructureToPtr(cf, lpar, False)

			Dim res As IntPtr = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar)

			Marshal.FreeCoTaskMem(lpar)
		End Sub

		Private Function GetSelectionStyle(ByVal mask As UInt32, ByVal effect As UInt32) As Integer
			Dim cf As New CHARFORMAT2_STRUCT()
			cf.cbSize = CUInt(Marshal.SizeOf(cf))
			cf.szFaceName = New Char(31){}

			Dim wpar As New IntPtr(SCF_SELECTION)
			Dim lpar As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf))
			Marshal.StructureToPtr(cf, lpar, False)

			Dim res As IntPtr = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar)

			cf = DirectCast(Marshal.PtrToStructure(lpar, GetType(CHARFORMAT2_STRUCT)), CHARFORMAT2_STRUCT)

			Dim state As Integer
			' dwMask holds the information which properties are consistent throughout the selection:
			If (cf.dwMask And mask) = mask Then
				If (cf.dwEffects And effect) = effect Then
					state = 1
				Else
					state = 0
				End If
			Else
				state = -1
			End If

			Marshal.FreeCoTaskMem(lpar)
			Return state
		End Function
	End Class
End Namespace
