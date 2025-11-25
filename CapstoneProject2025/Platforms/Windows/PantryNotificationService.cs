namespace CapstoneProject2025.Services
{
    public partial class PantryNotificationService
    {
        partial void InitializePlatform()
        {
            // Windows implementation would go here
        }

        partial void ShowNotification(int id, string title, string message)
        {
            // Windows implementation would go here
        }

        public Task RequestPermissionsAsync()
        {
            // Windows implementation would go here
            return Task.CompletedTask;
        }
    }
}