using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ApiSamples.Samples
{
    /// <summary>
    /// Class that implements the OLE IMessageFilter interface.
    /// </summary>
    class OleMessageFilter : IMessageFilter
    {
        [DllImport("Ole32.dll")]
        static extern int CoRegisterMessageFilter(IMessageFilter newFilter, out IMessageFilter oldFilter);

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <remarks>
        /// Instance of this class is only created by Register().
        /// </remarks>
        private OleMessageFilter()
        {
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~OleMessageFilter()
        {
            // Call Unregister() for good measure. It's fine if this gets called twice.
            Unregister();
        }

        /// <summary>
        /// Registers this instance of IMessageFilter with OLE to handle concurrency issues on the current thread. 
        /// </summary>
        /// <remarks>
        /// Only one message filter can be registered for each thread.
        /// Threads in multithreaded apartments cannot have message filters.
        /// Thread.CurrentThread.GetApartmentState() must equal ApartmentState.STA. In console applications, this can
        /// be achieved by applying the STAThreadAttribute to the Main() method. In WinForm applications, it is default.
        /// </remarks>
        public static void Register()
        {
            IMessageFilter newFilter = new OleMessageFilter();
            IMessageFilter oldFilter = null;

            // 1st check the current thread's apartment state. It must be STA!
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                // Call CoRegisterMessageFilter().
                Marshal.ThrowExceptionForHR(CoRegisterMessageFilter(newFilter: newFilter, oldFilter: out oldFilter));
            }
            else
            {
                throw new System.Exception("The current thread's apartment state must be STA.");
            }
        }

        /// <summary>
        /// Unregisters a previous instance of IMessageFilter with OLE on the current thread. 
        /// </summary>
        /// <remarks>
        /// It is not necessary to call Unregister() unless you need to explicitly do so as it is handled
        /// in the destructor.
        /// </remarks>
        public static void Unregister()
        {
            IMessageFilter oldFilter = null;

            // Call CoRegisterMessageFilter().
            CoRegisterMessageFilter(newFilter: null, oldFilter: out oldFilter);
        }

        #region IMessageFilter

        public int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo)
        {
            return (int)SERVERCALL.ISHANDLED;
        }

        public int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType)
        {
            // Cancel the outgoing call. This should be returned only under extreme conditions. Canceling a call that
            // has not replied or been rejected can create orphan transactions and lose resources. COM fails the original
            // call and returns RPC_E_CALL_CANCELLED.
            //return (int)NativeMethods.PENDINGMSG.PENDINGMSG_CANCELCALL;

            // Continue waiting for the reply, and do not dispatch the message unless it is a task-switching or
            // window-activation message. A subsequent message will trigger another call to MessagePending.
            //return (int)NativeMethods.PENDINGMSG.PENDINGMSG_WAITNOPROCESS;

            // Keyboard and mouse messages are no longer dispatched. However there are some cases where mouse and
            // keyboard messages could cause the system to deadlock, and in these cases, mouse and keyboard messages
            // are discarded. WM_PAINT messages are dispatched. Task-switching and activation messages are handled as before.
            return (int)PENDINGMSG.WAITDEFPROCESS;
        }

        public int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType)
        {
            if (dwRejectType == (int)SERVERCALL.RETRYLATER)
            {
                // 0 ≤ value < 100
                // The call is to be retried immediately.
                return 99;

                // 100 ≤ value
                // COM will wait for this many milliseconds and then retry the call.
                // return 1000; // Wait 1 second before retrying the call.
            }

            // The call should be canceled. COM then returns RPC_E_CALL_REJECTED from the original method call.
            return -1;
        }

        #endregion
    }

    /// <summary>
    /// Provides COM servers and applications with the ability to selectively handle incoming and outgoing COM
    /// messages while waiting for responses from synchronous calls.
    /// </summary>
    /// <remarks>http://msdn.microsoft.com/library/windows/desktop/ms693740.aspx</remarks>
    [ComImport]
    [Guid("00000016-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IMessageFilter
    {
        /// <summary>
        /// Provides a single entry point for incoming calls.
        /// </summary>
        /// <remarks>http://msdn.microsoft.com/library/windows/desktop/ms687237.aspx</remarks>
        [PreserveSig]
        int HandleInComingCall(int dwCallType, IntPtr hTaskCaller, int dwTickCount, IntPtr lpInterfaceInfo);

        /// <summary>
        /// Indicates that a message has arrived while COM is waiting to respond to a remote call.
        /// </summary>
        /// <remarks>http://msdn.microsoft.com/library/windows/desktop/ms694352.aspx</remarks>
        [PreserveSig]
        int MessagePending(IntPtr hTaskCallee, int dwTickCount, int dwPendingType);

        /// <summary>
        /// Provides applications with an opportunity to display a dialog box offering retry, cancel, or task-switching options.
        /// </summary>
        /// <remarks>http://msdn.microsoft.comlibrary/windows/desktop/ms680739.aspx</remarks>
        [PreserveSig]
        int RetryRejectedCall(IntPtr hTaskCallee, int dwTickCount, int dwRejectType);
    }

    internal enum SERVERCALL
    {
        ISHANDLED = 0,
        REJECTED = 1,
        RETRYLATER = 2
    }

    internal enum PENDINGMSG
    {
        CANCELCALL = 0,
        WAITNOPROCESS = 1,
        WAITDEFPROCESS = 2
    }
}
