using GMA_List.Resources.Services;
using GMA_List.Resources.Services.Interfaz;
using GMA_List.Resources.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace GMA_List
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
            // Registrar Pages
            builder.Services.AddTransient<MainPageAnimes>();
            // Registrar Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageSeries>();
            // Cargar configuración desde recurso embebido
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GMA_List.appsettings.json");

            if (stream != null)
            {
                var config = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                var supabaseUrl = config["Supabase:Url"];
                var supabaseKey = config["Supabase:Key"];

                builder.Services.AddSingleton<ISupabaseService>(
                    sp => new SupabaseService(supabaseUrl!, supabaseKey!)
                );
            }

            // Registrar ViewModels
            builder.Services.AddTransient<ListadoViewModel>(sp =>
                new ListadoViewModel(
                    sp.GetRequiredService<ISupabaseService>(),
                    "anime"
                )
            );

            // Registrar Pages
            builder.Services.AddTransient<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}