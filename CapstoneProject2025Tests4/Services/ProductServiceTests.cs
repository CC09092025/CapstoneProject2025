using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapstoneProject2025.Services;
using CapstoneProject2025.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneProject2025.Services.Testing
{
    [TestClass]
    public class ProductServiceTests
    {
        private string _testDatabasePath = Path.Combine(Path.GetTempPath(), "test_products.db3");

        [TestInitialize]
        public void Setup()
        {
            // Clean up the test database file if it exists
            if (File.Exists(_testDatabasePath))
            {
                File.Delete(_testDatabasePath);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up the test database file after each test
            if (File.Exists(_testDatabasePath))
            {
                File.Delete(_testDatabasePath);
            }
        }

        [TestMethod]
        public async Task ProductService_Constructor_WithDatabasePath_ShouldInitializeDatabase()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);

            // Act
            await productService.InitializeAsync();

            // Assert
            // No direct access to _database, so we test indirectly by saving and retrieving a product
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);
            await productService.SaveProductAsync(product);
            var products = await productService.GetProductsAsync();
            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        public async Task InitializeAsync_ShouldCreateProductTable()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);

            // Act
            await productService.InitializeAsync();

            // Assert
            // No direct access to _database, so we test indirectly by saving and retrieving a product
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);
            await productService.SaveProductAsync(product);
            var products = await productService.GetProductsAsync();
            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();

            // Act
            var products = await productService.GetProductsAsync();

            // Assert
            Assert.AreEqual(0, products.Count);
        }

        [TestMethod]
        public async Task SaveProductAsync_ShouldInsertNewProduct()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);

            // Act
            var result = await productService.SaveProductAsync(product);

            // Assert
            Assert.AreEqual(1, result);
            var products = await productService.GetProductsAsync();
            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        public async Task SaveProductAsync_ShouldUpdateExistingProduct()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);
            await productService.SaveProductAsync(product);

            // Modify the product
            product.Name = "Updated Test Product";

            // Act
            var result = await productService.SaveProductAsync(product);

            // Assert
            Assert.AreEqual(1, result);
            var updatedProduct = await productService.GetProductAsync(product.ItemId);
            Assert.AreEqual("Updated Test Product", updatedProduct.Name);
        }

        [TestMethod]
        public async Task GetProductAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);
            await productService.SaveProductAsync(product);

            // Act
            var retrievedProduct = await productService.GetProductAsync(product.ItemId);

            // Assert
            Assert.IsNotNull(retrievedProduct);
            Assert.AreEqual(product.ItemId, retrievedProduct.ItemId);
        }

        [TestMethod]
        public async Task GetProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();

            // Act
            var retrievedProduct = await productService.GetProductAsync("non-existent-id");

            // Assert
            Assert.IsNull(retrievedProduct);
        }

        [TestMethod]
        public async Task DeleteProductAsync_ShouldRemoveProduct()
        {
            // Arrange
            var productService = new ProductService(_testDatabasePath);
            await productService.InitializeAsync();
            var product = new Product("Test Product", "Test Category", DateTime.Now.AddDays(10), DateType.Expiration);
            await productService.SaveProductAsync(product);

            // Act
            var result = await productService.DeleteProductAsync(product);

            // Assert
            Assert.AreEqual(1, result);
            var products = await productService.GetProductsAsync();
            Assert.AreEqual(0, products.Count);
        }
    }
}