using Android.App;
using Android.Content.PM;
using Android.OS;

namespace CapstoneProject2025
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Request notification permissions for Android 13+
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                RequestPermissions(new[] { Android.Manifest.Permission.PostNotifications }, 0);
            }

            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    "pantry_alerts",
                    "Pantry Alerts",
                    NotificationImportance.High)
                {
                    Description = "Notifications for expiring and expired pantry items"
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager?.CreateNotificationChannel(channel);

                // Background tasks channel
                var bgChannel = new NotificationChannel(
                    "background_tasks",
                    "Background Tasks",
                    NotificationImportance.Low)
                {
                    Description = "Background notification checks"
                };
                notificationManager?.CreateNotificationChannel(bgChannel);
            }
        }
    }
}