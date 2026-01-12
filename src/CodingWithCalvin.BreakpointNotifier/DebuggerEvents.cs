using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CodingWithCalvin.BreakpointNotifier.Options;
using CodingWithCalvin.Otel4Vsix;
using Microsoft;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace CodingWithCalvin.BreakpointNotifier
{
    public sealed class DebuggerEvents : IVsDebuggerEvents, IDebugEventCallback2
    {
        private readonly BreakpointNotifierPackage _package;

        private DebuggerEvents(BreakpointNotifierPackage package)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _package = package;

            var debugger = (IVsDebugger)
                ServiceProvider.GlobalProvider.GetService(typeof(IVsDebugger));
            Assumes.Present(debugger);
            debugger.AdviseDebuggerEvents(this, out _);
            debugger.AdviseDebugEventCallback(this);
        }

        public static DebuggerEvents Initialize(BreakpointNotifierPackage package)
        {
            return new DebuggerEvents(package);
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
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pEvent is IDebugBreakpointEvent2)
            {
                using var activity = VsixTelemetry.StartCommandActivity("BreakpointNotifier.BreakpointHit");

                try
                {
                    VsixTelemetry.LogInformation("Breakpoint hit detected");
                    ShowNotification();
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

        private void ShowNotification()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var style = GetNotificationStyle();

            if (style == NotificationStyle.MessageBox || style == NotificationStyle.Both)
            {
                MessageBox.Show("Breakpoint Hit!");
            }

            if (style == NotificationStyle.Toast || style == NotificationStyle.Both)
            {
                new ToastContentBuilder()
                    .AddText("Breakpoint Hit!")
                    .Show();
            }
        }

        private NotificationStyle GetNotificationStyle()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var options = _package.GetDialogPage(typeof(GeneralOptions)) as GeneralOptions;
            return options?.Style ?? NotificationStyle.Toast;
        }
    }
}
