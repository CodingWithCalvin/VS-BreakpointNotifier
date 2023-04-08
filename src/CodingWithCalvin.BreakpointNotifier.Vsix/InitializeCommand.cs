using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using System;

namespace CodingWithCalvin.BreakpointNotifier.Vsix
{
    internal class InitializeCommand
    {
        private readonly Package _package;
        private static DebuggerEvents _debuggerEvents;

        private InitializeCommand(Package package)
        {
            _package = package;

            var commandService = (OleMenuCommandService)ServiceProvider.GetService(typeof(IMenuCommandService));

            if (commandService == null)
            {
                return;
            }

            var menuCommandId = new CommandID(PackageGuids.CommandSetGuid, PackageIds.InitializeCommandId);
            var menuItem = new MenuCommand(InitializeDebuggerEvents, menuCommandId);
            commandService.AddCommand(menuItem);
        }
        
        private IServiceProvider ServiceProvider => _package;

        public static void Initialize(Package package)
        {
            _ = new InitializeCommand(package);
        }

        private static void InitializeDebuggerEvents(object sender, EventArgs e)
        {
            _debuggerEvents = DebuggerEvents.Initialize();
        }
    }
}