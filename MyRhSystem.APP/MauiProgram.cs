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
            // Backend base URL por plataforma (porta fixa 7006)
            // =========================
            string BackendBase;
#if WINDOWS
            BackendBase = "https://localhost:7006/";
#elif ANDROID
            // Emulador usa 10.0.2.2; dispositivo físico usa o IP do seu PC (192.168.1.23 no seu caso)
            BackendBase = DeviceInfo.DeviceType == DeviceType.Virtual
                ? "https://10.0.2.2:7006/"
                : "https://192.168.2.29:7006/";
#elif IOS
            BackendBase = "https://192.168.0.10:7006/";
#else
            BackendBase = "https://localhost:7006/";
#endif

            // Handler que aceita o certificado de dev (apenas em DEBUG)
            static HttpMessageHandler DevHandlerFactory()
            {
                var h = new HttpClientHandler();
#if DEBUG
                h.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif
                return h;
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
