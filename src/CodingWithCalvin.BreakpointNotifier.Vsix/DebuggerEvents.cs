using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;
using Microsoft;

namespace CodingWithCalvin.BreakpointNotifier.Vsix
{
    public sealed class DebuggerEvents: IVsDebuggerEvents
    {
        private DebuggerEvents()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            var debugger = (IVsDebugger)ServiceProvider.GlobalProvider.GetService(typeof(IVsDebugger));
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