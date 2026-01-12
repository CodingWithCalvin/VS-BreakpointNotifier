using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace CodingWithCalvin.BreakpointNotifier.Options
{
    public enum NotificationStyle
    {
        [Description("Message Box")]
        MessageBox,

        [Description("Toast Notification")]
        Toast,

        [Description("Both")]
        Both
    }

    [ComVisible(true)]
    public class GeneralOptions : DialogPage
    {
        [Category("Notification")]
        [DisplayName("Notification Style")]
        [Description("Choose how breakpoint notifications are displayed. Toast notifications are less intrusive and don't steal focus.")]
        public NotificationStyle Style { get; set; } = NotificationStyle.Toast;
    }
}
