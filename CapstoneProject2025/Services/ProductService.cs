using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneProject2025.Models;

namespace CapstoneProject2025.Services
{
    public class ProductService : IProductService
    {
        private SQLiteAsyncConnection _database;

        public ProductService()
        {
            _database = new SQLiteAsyncConnection(GetDatabasePath(), SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache);
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
            if (!string.IsNullOrEmpty(product.ItemId) && await GetProductAsync(product.ItemId) != null)
            {
                // Update existing product
                return await _database.UpdateAsync(product);
            }
            else
            {
                // Insert new product
                if (string.IsNullOrEmpty(product.ItemId))
                    product.ItemId = Guid.NewGuid().ToString();

                return await _database.InsertAsync(product);
            }
        }

        public async Task<int> DeleteProductAsync(Product product)
        {
            return await _database.DeleteAsync(product);
        }
    }
}