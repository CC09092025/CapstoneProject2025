using CapstoneProject2025.Models;

namespace CapstoneProject2025.Services
{
    public interface IPantryNotificationService
    {
        Task InitializeAsync();
        Task CheckAndScheduleNotificationsAsync();
        Task RequestPermissionsAsync();
    }

    // Platform-specific implementation will be in Platforms/Android
    public partial class PantryNotificationService : IPantryNotificationService
    {
        private readonly IProductService _productService;
        private const string LAST_NOTIFICATION_DATE_KEY = "last_notification_date";

        public PantryNotificationService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task InitializeAsync()
        {
            await RequestPermissionsAsync();
            InitializePlatform();
        }

        public async Task CheckAndScheduleNotificationsAsync()
        {
            var products = await _productService.GetProductsAsync();

            var upcomingItems = new List<Product>();
            var expiredItems = new List<Product>();

            foreach (var product in products)
            {
                if (product.IsExpiredNow())
                {
                    expiredItems.Add(product);
                }
                else if (product.IsExpiringSoon())
                {
                    upcomingItems.Add(product);
                }
            }

            // Check if we should send notifications today
            var lastNotificationDate = Preferences.Get(LAST_NOTIFICATION_DATE_KEY, string.Empty);
            var today = DateTime.Now.Date.ToString("yyyy-MM-dd");

            if (lastNotificationDate != today)
            {
                // Send notifications for today
                if (upcomingItems.Count > 0)
                {
                    ShowNotification(
                        1001,
                        "Items Expiring Soon",
                        upcomingItems.Count == 1
                            ? "1 item in your pantry is expiring soon"
                            : $"{upcomingItems.Count} items in your pantry are expiring soon"
                    );
                }

                if (expiredItems.Count > 0)
                {
                    ShowNotification(
                        1002,
                        "Expired Items",
                        expiredItems.Count == 1
                            ? "1 item in your pantry has expired"
                            : $"{expiredItems.Count} items in your pantry have expired"
                    );
                }

                // Update last notification date
                Preferences.Set(LAST_NOTIFICATION_DATE_KEY, today);
            }
        }

        // Platform-specific methods implemented in partial classes
        partial void InitializePlatform();
        partial void ShowNotification(int id, string title, string message);
    }
}