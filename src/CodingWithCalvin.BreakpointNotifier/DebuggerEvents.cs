using System;
using System.Windows.Forms;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodingWithCalvin.BreakpointNotifier
{
    public sealed class DebuggerEvents : IVsDebuggerEvents, IDebugEventCallback2
    {
        private DebuggerEvents()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var debugger = (IVsDebugger)
                ServiceProvider.GlobalProvider.GetService(typeof(IVsDebugger));
            Assumes.Present(debugger);
            debugger.AdviseDebuggerEvents(this, out _);
            debugger.AdviseDebugEventCallback(this);
        }

        public static DebuggerEvents Initialize()
        {
            return new DebuggerEvents();
        }

        public int OnModeChange(DBGMODE dbgmodeNew)
        {
            // No longer showing message here - we use IDebugEventCallback2 instead
            // to specifically detect breakpoint hits vs. step operations
            return VSConstants.S_OK;
        }

        public int Event(
            IDebugEngine2 pEngine,
            IDebugProcess2 pProcess,
            IDebugProgram2 pProgram,
            IDebugThread2 pThread,
            IDebugEvent2 pEvent,
            ref Guid riidEvent,
            uint dwAttrib)
        {
            if (pEvent is IDebugBreakpointEvent2)
            {
                MessageBox.Show("Breakpoint Hit!");
            }

            return VSConstants.S_OK;
        }
    }
}
