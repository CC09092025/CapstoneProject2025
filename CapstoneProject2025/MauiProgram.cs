using Microsoft.Extensions.Logging;
using CapstoneProject2025.Services;

namespace CapstoneProject2025
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Register services
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IPinAuthenticationService, PinAuthenticationService>();
            builder.Services.AddSingleton<IAuthStateProvider, AuthStateProvider>();

            return builder.Build();
        }
    }
}