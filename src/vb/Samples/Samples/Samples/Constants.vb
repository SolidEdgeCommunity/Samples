Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples
	Friend Class HRESULT
		Private _hr As Integer

		Private Sub New(ByVal hr As Integer)
			_hr = hr
		End Sub

		Public Shared Widening Operator CType(ByVal hr As HRESULT) As Integer
			Return hr._hr
		End Operator

		Public Shared Widening Operator CType(ByVal hr As Integer) As HRESULT
			Return New HRESULT(hr)
		End Operator

		#Region "winerror.h"

'INSTANT VB TODO TASK: There is no VB equivalent to 'unchecked' in this context:
'ORIGINAL LINE: public const int MK_E_UNAVAILABLE = unchecked((int)&H800401E3);
		Public Const MK_E_UNAVAILABLE As Integer = CInt(&H800401E3)

		#End Region
	End Class
End Namespace
