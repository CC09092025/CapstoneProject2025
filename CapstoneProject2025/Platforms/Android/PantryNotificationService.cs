using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace CapstoneProject2025.Services
{
    public partial class PantryNotificationService
    {
        private const string CHANNEL_ID = "pantry_alerts";
        private const string CHANNEL_NAME = "Pantry Alerts";

        partial void InitializePlatform()
        {
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.High)
                {
                    Description = "Notifications for expiring and expired pantry items"
                };

                var notificationManager = (NotificationManager)Android.App.Application.Context.GetSystemService(Context.NotificationService);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        partial void ShowNotification(int id, string title, string message)
        {
            var context = Android.App.Application.Context;

            var intent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName ?? string.Empty);
            intent?.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

            var pendingIntent = PendingIntent.GetActivity(
                context,
                0,
                intent,
                PendingIntentFlags.Immutable
            );

            var builder = new NotificationCompat.Builder(context, CHANNEL_ID)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.dotnet_bot)
                .SetPriority(NotificationCompat.PriorityHigh)
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);

            var notificationManager = NotificationManagerCompat.From(context);

            if (notificationManager != null && ActivityCompat.CheckSelfPermission(context, Android.Manifest.Permission.PostNotifications) == Android.Content.PM.Permission.Granted)
            {
                notificationManager.Notify(id, builder.Build());
            }
        }

        public Task RequestPermissionsAsync()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var activity = Platform.CurrentActivity;
                if (activity != null)
                {
                    ActivityCompat.RequestPermissions(activity, new[] { Android.Manifest.Permission.PostNotifications }, 0);
                }
            }
            return Task.CompletedTask;
        }
    }
}