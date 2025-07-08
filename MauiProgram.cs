using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

// Base Infrastructure
using TSV.ViewModels.Base;
using TSV.Services.Navigation;
using TSV.Services.Data;

// Kunden Module
using TSV.Views.Kunden;
using TSV.ViewModels.Kunden;
using TSV.ViewModels;

// Value Converters
using TSV.Converters;

namespace TSV;

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

#if DEBUG
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        // =====================================================
        // DATABASE SERVICES
        // =====================================================

        // Connection String für SQL Server Express
        var connectionString = "Server=L12296\\SQLEXPRESS;Database=TSV;Integrated Security=true;TrustServerCertificate=true;";

        // Entity Framework registrieren
        builder.Services.AddDbContext<TsvDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Database Service (Repository Pattern)
        builder.Services.AddScoped<IDatabaseService, DatabaseService>();

        // =====================================================
        // NAVIGATION SERVICE
        // =====================================================
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        // =====================================================
        // VALUE CONVERTERS - Werden in App.xaml registriert
        // =====================================================
        // Value Converters werden in App.xaml als Resources definiert

        // =====================================================
        // PAGES & VIEWMODELS - Hauptseiten
        // =====================================================

        // Main Page
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MainPageViewModel>();

        // =====================================================
        // KUNDEN MODULE - Pages & ViewModels
        // =====================================================

        // Kunden Liste
        builder.Services.AddTransient<KundenListePage>();
        builder.Services.AddTransient<KundenListeViewModel>();

        // Kunden Detail (NEU!)
        builder.Services.AddTransient<KundenDetailPage>();
        builder.Services.AddTransient<KundenDetailViewModel>();

        // =====================================================
        // ROUTING CONFIGURATION
        // =====================================================

        // Navigation Routes registrieren
        Routing.RegisterRoute("KundenListe", typeof(KundenListePage));
        Routing.RegisterRoute("KundenDetail", typeof(KundenDetailPage)); // NEU!

        // Weitere Routes (für später)
        // Routing.RegisterRoute("KurseListe", typeof(KurseListePage));
        // Routing.RegisterRoute("TeamListe", typeof(TeamListePage));
        // Routing.RegisterRoute("Statistik", typeof(StatistikPage));

        return builder.Build();
    }
}