Imports System
Imports System.Runtime.InteropServices
Imports System.Threading

Namespace ApiSamples.Samples
	''' <summary>
	''' Class that implements the OLE IMessageFilter interface.
	''' </summary>
	Friend Class OleMessageFilter
		Implements IMessageFilter

		<DllImport("Ole32.dll")> _
		Shared Function CoRegisterMessageFilter(ByVal newFilter As IMessageFilter, ByRef oldFilter As IMessageFilter) As Integer
		End Function

		''' <summary>
		''' Private constructor.
		''' </summary>
		''' <remarks>
		''' Instance of this class is only created by Register().
		''' </remarks>
		Private Sub New()
		End Sub

		''' <summary>
		''' Destructor.
		''' </summary>
		Protected Overrides Sub Finalize()
			' Call Unregister() for good measure. It's fine if this gets called twice.
			Unregister()
		End Sub

		''' <summary>
		''' Registers this instance of IMessageFilter with OLE to handle concurrency issues on the current thread. 
		''' </summary>
		''' <remarks>
		''' Only one message filter can be registered for each thread.
		''' Threads in multithreaded apartments cannot have message filters.
		''' Thread.CurrentThread.GetApartmentState() must equal ApartmentState.STA. In console applications, this can
		''' be achieved by applying the STAThreadAttribute to the Main() method. In WinForm applications, it is default.
		''' </remarks>
		Public Shared Sub Register()
			Dim newFilter As IMessageFilter = New OleMessageFilter()
			Dim oldFilter As IMessageFilter = Nothing

			' 1st check the current thread's apartment state. It must be STA!
			If Thread.CurrentThread.GetApartmentState() = ApartmentState.STA Then
				' Call CoRegisterMessageFilter().
				Marshal.ThrowExceptionForHR(CoRegisterMessageFilter(newFilter:= newFilter, oldFilter:= oldFilter))
			Else
				Throw New System.Exception("The current thread's apartment state must be STA.")
			End If
		End Sub

		''' <summary>
		''' Unregisters a previous instance of IMessageFilter with OLE on the current thread. 
		''' </summary>
		''' <remarks>
		''' It is not necessary to call Unregister() unless you need to explicitly do so as it is handled
		''' in the destructor.
		''' </remarks>
		Public Shared Sub Unregister()
			Dim oldFilter As IMessageFilter = Nothing

			' Call CoRegisterMessageFilter().
			CoRegisterMessageFilter(newFilter:= Nothing, oldFilter:= oldFilter)
		End Sub

		#Region "IMessageFilter"

		Public Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer Implements IMessageFilter.HandleInComingCall
			Return CInt(SERVERCALL.ISHANDLED)
		End Function

		Public Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer Implements IMessageFilter.MessagePending
			' Cancel the outgoing call. This should be returned only under extreme conditions. Canceling a call that
			' has not replied or been rejected can create orphan transactions and lose resources. COM fails the original
			' call and returns RPC_E_CALL_CANCELLED.
			'return (int)NativeMethods.PENDINGMSG.PENDINGMSG_CANCELCALL;

			' Continue waiting for the reply, and do not dispatch the message unless it is a task-switching or
			' window-activation message. A subsequent message will trigger another call to MessagePending.
			'return (int)NativeMethods.PENDINGMSG.PENDINGMSG_WAITNOPROCESS;

			' Keyboard and mouse messages are no longer dispatched. However there are some cases where mouse and
			' keyboard messages could cause the system to deadlock, and in these cases, mouse and keyboard messages
			' are discarded. WM_PAINT messages are dispatched. Task-switching and activation messages are handled as before.
			Return CInt(PENDINGMSG.WAITDEFPROCESS)
		End Function

		Public Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer Implements IMessageFilter.RetryRejectedCall
			If dwRejectType = CInt(SERVERCALL.RETRYLATER) Then
				' 0 ≤ value < 100
				' The call is to be retried immediately.
				Return 99

				' 100 ≤ value
				' COM will wait for this many milliseconds and then retry the call.
				' return 1000; // Wait 1 second before retrying the call.
			End If

			' The call should be canceled. COM then returns RPC_E_CALL_REJECTED from the original method call.
			Return -1
		End Function

		#End Region
	End Class

	''' <summary>
	''' Provides COM servers and applications with the ability to selectively handle incoming and outgoing COM
	''' messages while waiting for responses from synchronous calls.
	''' </summary>
	''' <remarks>http://msdn.microsoft.com/library/windows/desktop/ms693740.aspx</remarks>
	<ComImport, Guid("00000016-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)> _
	Friend Interface IMessageFilter
		''' <summary>
		''' Provides a single entry point for incoming calls.
		''' </summary>
		''' <remarks>http://msdn.microsoft.com/library/windows/desktop/ms687237.aspx</remarks>
		<PreserveSig> _
		Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer

		''' <summary>
		''' Indicates that a message has arrived while COM is waiting to respond to a remote call.
		''' </summary>
		''' <remarks>http://msdn.microsoft.com/library/windows/desktop/ms694352.aspx</remarks>
		<PreserveSig> _
		Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer

		''' <summary>
		''' Provides applications with an opportunity to display a dialog box offering retry, cancel, or task-switching options.
		''' </summary>
		''' <remarks>http://msdn.microsoft.comlibrary/windows/desktop/ms680739.aspx</remarks>
		<PreserveSig> _
		Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer
	End Interface

	Friend Enum SERVERCALL
		ISHANDLED = 0
		REJECTED = 1
		RETRYLATER = 2
	End Enum

	Friend Enum PENDINGMSG
		CANCELCALL = 0
		WAITNOPROCESS = 1
		WAITDEFPROCESS = 2
	End Enum
End Namespace
