using System.Windows.Forms;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodingWithCalvin.BreakpointNotifier
{
    public sealed class DebuggerEvents : IVsDebuggerEvents
    {
        private DebuggerEvents()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var debugger = (IVsDebugger)
                ServiceProvider.GlobalProvider.GetService(typeof(IVsDebugger));
            Assumes.Present(debugger);
            debugger.AdviseDebuggerEvents(this, out _);
        }

        public static DebuggerEvents Initialize()
        {
            return new DebuggerEvents();
        }

        public int OnModeChange(DBGMODE dbgmodeNew)
        {
            switch (dbgmodeNew)
            {
                case DBGMODE.DBGMODE_Break:
                    MessageBox.Show("Breakpoint Hit!");
                    break;
            }

            return VSConstants.S_OK;
        }
    }
}
