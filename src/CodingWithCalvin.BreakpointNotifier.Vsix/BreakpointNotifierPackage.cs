using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace CodingWithCalvin.BreakpointNotifier.Vsix
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    public sealed class BreakpointNotifierPackage : AsyncPackage
    {
        protected override Task OnAfterPackageLoadedAsync(CancellationToken cancellationToken)
        {
            DebuggerEvents.Initialize();

            return Task.CompletedTask;
        }

    }
}
