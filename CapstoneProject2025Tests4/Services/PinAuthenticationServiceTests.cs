using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapstoneProject2025.Services;
using System;
using System.Threading.Tasks;
using Moq;

namespace CapstoneProject2025.Services.Tests
{
    [TestClass()]
    public class PinAuthenticationServiceTests
    {
        private Mock<ISecureStorage> _secureStorageMock;
        private PinAuthenticationService _service;

        [TestInitialize]
        public void Setup()
        {
            _secureStorageMock = new Mock<ISecureStorage>();
            _service = new PinAuthenticationService(_secureStorageMock.Object);
        }

        [TestMethod()]
        public async Task HasPinAsyncTest_WhenPinExists_ReturnsTrue()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ReturnsAsync("some_hash");

            // Act
            var result = await _service.HasPinAsync();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public async Task HasPinAsyncTest_WhenPinDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ReturnsAsync((string)null);

            // Act
            var result = await _service.HasPinAsync();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task HasPinAsyncTest_WhenExceptionOccurs_ReturnsFalse()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _service.HasPinAsync();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task SetPinAsyncTest_WithValidPin_ReturnsTrue()
        {
            // Arrange
            var pin = "1234";

            // Act
            var result = await _service.SetPinAsync(pin);

            // Assert
            Assert.IsTrue(result);
            _secureStorageMock.Verify(
                x => x.SetAsync(PinAuthenticationService.PIN_HASH_KEY, It.IsAny<string>()), Times.Once);
            _secureStorageMock.Verify(
                x => x.SetAsync(PinAuthenticationService.PIN_SALT_KEY, It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public async Task SetPinAsyncTest_WithInvalidPin_ReturnsFalse()
        {
            // Arrange
            var pin = "12";

            // Act
            var result = await _service.SetPinAsync(pin);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task SetPinAsyncTest_WhenExceptionOccurs_ReturnsFalse()
        {
            // Arrange
            var pin = "1234";
            _secureStorageMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _service.SetPinAsync(pin);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task ValidatePinAsyncTest_WithCorrectPin_ReturnsTrue()
        {
            // Arrange
            var pin = "1234";
            var salt = "some_salt";
            var hash = _service.HashPin(pin, salt);

            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ReturnsAsync(hash);
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_SALT_KEY))
                .ReturnsAsync(salt);

            // Act
            var result = await _service.ValidatePinAsync(pin);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public async Task ValidatePinAsyncTest_WithIncorrectPin_ReturnsFalse()
        {
            // Arrange
            var pin = "1234";
            var salt = "some_salt";
            var hash = _service.HashPin(pin, salt);

            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ReturnsAsync(hash);
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_SALT_KEY))
                .ReturnsAsync(salt);

            // Act
            var result = await _service.ValidatePinAsync("0000");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task ValidatePinAsyncTest_WhenPinDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ReturnsAsync((string)null);

            // Act
            var result = await _service.ValidatePinAsync("1234");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task ValidatePinAsyncTest_WhenExceptionOccurs_ReturnsFalse()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.GetAsync(PinAuthenticationService.PIN_HASH_KEY))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _service.ValidatePinAsync("1234");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public async Task ClearPinAsyncTest_WhenSuccessful_ReturnsTrue()
        {
            // Act
            var result = await _service.ClearPinAsync();

            // Assert
            Assert.IsTrue(result);
            _secureStorageMock.Verify(
                x => x.Remove(PinAuthenticationService.PIN_HASH_KEY), Times.Once);
            _secureStorageMock.Verify(
                x => x.Remove(PinAuthenticationService.PIN_SALT_KEY), Times.Once);
        }

        [TestMethod()]
        public async Task ClearPinAsyncTest_WhenExceptionOccurs_ReturnsFalse()
        {
            // Arrange
            _secureStorageMock.Setup(x => x.Remove(It.IsAny<string>()))
                .Throws(new Exception());

            // Act
            var result = await _service.ClearPinAsync();

            // Assert
            Assert.IsFalse(result);
        }
    }
}
