namespace CapstoneProject2025.Services
{
    public interface IAuthStateProvider
    {
        
        bool IsAuthenticated { get; set; }
        event Action? OnAuthStateChanged;
        void NotifyAuthStateChanged();
    }

    public class AuthStateProvider : IAuthStateProvider
    {
        private bool _isAuthenticated = false;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        public event Action? OnAuthStateChanged;

        public void NotifyAuthStateChanged()
        {
            OnAuthStateChanged?.Invoke();
        }
    }
}