#if WINDOWS
using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace CapstoneProject2025.Services
{
    public partial class PantryNotificationService
    {
        partial void InitializePlatform()
        {
        }

        partial void ShowNotification(int id, string title, string message)
        {
            var toastContent = new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .GetToastContent();

            var toast = new ToastNotification(toastContent.GetXml())
            {
                ExpirationTime = DateTimeOffset.UtcNow.AddHours(1)
            };

            ToastNotificationManager.CreateToastNotifier("CapstoneProject2025").Show(toast);
        }

        public Task RequestPermissionsAsync()
        {
            return Task.CompletedTask;
        }
    }
}
#endif
