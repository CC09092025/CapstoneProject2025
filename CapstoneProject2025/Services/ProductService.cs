using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneProject2025.Models;

namespace CapstoneProject2025.Services
{
    public class ProductService : IProductService
    {
        private SQLiteAsyncConnection _database;
        private IPantryNotificationService _notificationService;

        public ProductService()
        {
            _database = new SQLiteAsyncConnection(GetDatabasePath(), SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
        }

        public void SetNotificationService(IPantryNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        private string GetDatabasePath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, "products.db3");
        }

        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<Product>();
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _database.Table<Product>().ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _database.Table<Product>().Where(p => p.ItemId == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveProductAsync(Product product)
        {
            int result;

            if (!string.IsNullOrEmpty(product.ItemId) && await GetProductAsync(product.ItemId) != null)
            {
                // Update existing product
                result = await _database.UpdateAsync(product);
            }
            else
            {
                // Insert new product
                if (string.IsNullOrEmpty(product.ItemId))
                    product.ItemId = Guid.NewGuid().ToString();

                result = await _database.InsertAsync(product);
            }

            // Trigger notification check after save
            if (_notificationService != null)
            {
                await _notificationService.CheckAndScheduleNotificationsAsync();
            }

            return result;
        }

        public async Task<int> DeleteProductAsync(Product product)
        {
            var result = await _database.DeleteAsync(product);

            // Trigger notification check after delete
            if (_notificationService != null)
            {
                await _notificationService.CheckAndScheduleNotificationsAsync();
            }

            return result;
        }
    }
}