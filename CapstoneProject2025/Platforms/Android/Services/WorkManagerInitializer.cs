using Android.App;
using Android.Content;
using AndroidX.Work;
using Java.Util.Concurrent;
using Microsoft.Maui;
using AndroidApp = Android.App.Application;

namespace CapstoneProject2025.Platforms.Android
{
    [Service(Exported = false)]
    public class WorkManagerInitializer
    {
        private const string WORK_NAME = "PantryCheckWork";

        public static void Initialize()
        {
            // Use the alias
            var context = AndroidApp.Context;

            var workRequest =
                PeriodicWorkRequest.Builder
                .From<Services.PantryWorker>(24, TimeUnit.Hours)
                .Build();

            WorkManager
                .GetInstance(context)
                .EnqueueUniquePeriodicWork(
                    WORK_NAME,
                    ExistingPeriodicWorkPolicy.Keep,
                    workRequest
                );
        }
    }
}