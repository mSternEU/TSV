using Microsoft.Extensions.Logging;
using TSV.Services.Navigation;
using TSV.ViewModels;
using TSV.ViewModels.Base;

namespace TSV
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // =====================================================
            // SERVICES REGISTRIERUNG (wie in deinem WPF-Template)
            // =====================================================

            // Navigation Service (Singleton - eine Instanz für die ganze App)
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            // =====================================================
            // VIEWMODELS REGISTRIERUNG
            // =====================================================

            // Main ViewModels (Transient - neue Instanz bei jeder Anfrage)
            builder.Services.AddTransient<MainPageViewModel>();

            // Hier kommen später die anderen ViewModels dazu:
            // builder.Services.AddTransient<KundenViewModel>();
            // builder.Services.AddTransient<KurseViewModel>();
            // builder.Services.AddTransient<TeamViewModel>();
            // builder.Services.AddTransient<StatistikViewModel>();

            // =====================================================
            // PAGES REGISTRIERUNG (für Dependency Injection)
            // =====================================================

            // Main Page (bereits vorhanden)
            builder.Services.AddTransient<MainPage>();

            // Hier kommen später die anderen Pages dazu:
            // builder.Services.AddTransient<KundenPage>();
            // builder.Services.AddTransient<KursePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}