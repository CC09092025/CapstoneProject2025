using Android.Content;
using AndroidX.Work;
using Microsoft.Maui;
using CapstoneProject2025.Services;

namespace CapstoneProject2025.Platforms.Android.Services
{
    public class PantryWorker : Worker
    {
        public PantryWorker(Context context, WorkerParameters workerParams)
            : base(context, workerParams)
        {
        }

        public override Result DoWork()
        {
            try
            {
                // Resolve the shared service
                var service = MauiApplication.Current.Services.GetService<IPantryNotificationService>();

                if (service != null)
                {
                    // Run the notification check
                    service.CheckAndScheduleNotificationsAsync().Wait();
                }

                return Result.InvokeSuccess();
            }
            catch
            {
                return Result.InvokeRetry();
            }
        }
    }
}