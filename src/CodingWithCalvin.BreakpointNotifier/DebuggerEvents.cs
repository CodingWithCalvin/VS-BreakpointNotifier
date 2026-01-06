using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodingWithCalvin.Otel4Vsix;
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
            VsixTelemetry.LogInformation("Debugger mode changed to {Mode}", dbgmodeNew.ToString());
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
                using var activity = VsixTelemetry.StartCommandActivity("BreakpointNotifier.BreakpointHit");

                try
                {
                    VsixTelemetry.LogInformation("Breakpoint hit detected");
                    MessageBox.Show("Breakpoint Hit!");
                }
                catch (Exception ex)
                {
                    activity?.RecordError(ex);
                    VsixTelemetry.TrackException(ex, new Dictionary<string, object>
                    {
                        { "operation.name", "BreakpointHit" }
                    });
                }
            }

            return VSConstants.S_OK;
        }
    }
}
