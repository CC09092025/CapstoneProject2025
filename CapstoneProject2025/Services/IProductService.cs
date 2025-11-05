using System.Collections.Generic;
using System.Threading.Tasks;
using CapstoneProject2025.Models;
namespace CapstoneProject2025.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string id);
        Task<int> SaveProductAsync(Product product);
        Task<int> DeleteProductAsync(Product product);
        Task InitializeAsync();
    }
}