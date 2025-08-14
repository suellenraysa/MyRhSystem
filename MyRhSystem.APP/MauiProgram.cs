using Microsoft.Extensions.Logging;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.APP.Shared.ViewModels;
using MyRhSystem.APP.ViewModels;

namespace MyRhSystem.APP
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

            // =========================
            // Backend base URL por plataforma
            // =========================
#if WINDOWS
            const string BackendBase = "https://localhost:7006/";
#elif ANDROID
            // Emulador Android acessa a máquina host por 10.0.2.2
            const string BackendBase = "https://10.0.2.2:7006/";
#elif IOS
            // Use o IP da sua máquina (se rodando no simulador/devices)
            // Ex.: "https://192.168.0.10:7006/"
            const string BackendBase = "https://192.168.0.10:7006/";
#else
            const string BackendBase = "https://localhost:7006/";
#endif

            // Handler que aceita o certificado de dev (apenas em DEBUG)
            HttpMessageHandler DevHandlerFactory()
            {
#if DEBUG
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
                };
#else
                return new HttpClientHandler();
#endif
            }

            // =========================
            // HTTP CLIENTS TIPADOS
            // =========================

          
            builder.Services.AddHttpClient<UsersApiService>(c =>
            {
                c.BaseAddress = new Uri(BackendBase);
            }).ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            builder.Services.AddHttpClient<AuthApiService>(c =>
            {
                c.BaseAddress = new Uri(BackendBase);
            }).ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            // =========================
            // VIEWMODELS
            // =========================
            builder.Services.AddScoped<UsersViewModel>();
            builder.Services.AddScoped<UserEditViewModel>();
            builder.Services.AddScoped<LoginViewModel>();

            var app = builder.Build();
            return app;
        }
    }
}
