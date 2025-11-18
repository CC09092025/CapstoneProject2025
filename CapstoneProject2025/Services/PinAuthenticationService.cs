using System.Security.Cryptography;
using System.Text;

namespace CapstoneProject2025.Services
{
    public interface IPinAuthenticationService
    {
        Task<bool> HasPinAsync();
        Task<bool> SetPinAsync(string pin);
        Task<bool> ValidatePinAsync(string pin);
        Task<bool> ClearPinAsync();
    }

    public class PinAuthenticationService : IPinAuthenticationService
    {
        private const string PIN_HASH_KEY = "user_pin_hash";
        private const string PIN_SALT_KEY = "user_pin_salt";

        public async Task<bool> HasPinAsync()
        {
            try
            {
                var hash = await SecureStorage.GetAsync(PIN_HASH_KEY);
                return !string.IsNullOrEmpty(hash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SetPinAsync(string pin)
        {
            try
            {
                if (string.IsNullOrEmpty(pin) || pin.Length != 4 || !pin.All(char.IsDigit))
                {
                    return false;
                }

                // Generate a unique salt
                var salt = GenerateSalt();
                var hash = HashPin(pin, salt);

                // Store both hash and salt in secure storage
                await SecureStorage.SetAsync(PIN_HASH_KEY, hash);
                await SecureStorage.SetAsync(PIN_SALT_KEY, salt);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidatePinAsync(string pin)
        {
            try
            {
                if (string.IsNullOrEmpty(pin) || pin.Length != 4)
                {
                    return false;
                }

                var storedHash = await SecureStorage.GetAsync(PIN_HASH_KEY);
                var storedSalt = await SecureStorage.GetAsync(PIN_SALT_KEY);

                if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
                {
                    return false;
                }

                var computedHash = HashPin(pin, storedSalt);
                return storedHash == computedHash;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ClearPinAsync()
        {
            try
            {
                SecureStorage.Remove(PIN_HASH_KEY);
                SecureStorage.Remove(PIN_SALT_KEY);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GenerateSalt()
        {
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private string HashPin(string pin, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combined = Encoding.UTF8.GetBytes(pin + salt);
                var hashBytes = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}