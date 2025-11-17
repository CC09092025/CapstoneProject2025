using CapstoneProject2025.Services;

namespace CapstoneProject2025
{
    public partial class App : Application
    {
        public static bool IsAuthenticated { get; set; } = false;

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}