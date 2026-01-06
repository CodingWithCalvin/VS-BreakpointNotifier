using System;
using System.Runtime.InteropServices;
using System.Threading;
using CodingWithCalvin.Otel4Vsix;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace CodingWithCalvin.BreakpointNotifier
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [Guid("136f3004-4048-4dd9-bd6d-7ff910b2c900")]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    public sealed class BreakpointNotifierPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(
            CancellationToken cancellationToken,
            IProgress<ServiceProgressData> progress
        )
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var builder = VsixTelemetry.Configure()
                .WithServiceName(VsixInfo.DisplayName)
                .WithServiceVersion(VsixInfo.Version)
                .WithVisualStudioAttributes(this)
                .WithEnvironmentAttributes();

#if !DEBUG
            builder
                .WithOtlpHttp("https://api.honeycomb.io")
                .WithHeader("x-honeycomb-team", HoneycombConfig.ApiKey);
#endif

            builder.Initialize();

            DebuggerEvents.Initialize();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                VsixTelemetry.Shutdown();
            }

            base.Dispose(disposing);
        }
    }
}
