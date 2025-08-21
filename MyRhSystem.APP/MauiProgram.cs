using Microsoft.Extensions.Configuration;
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

            // Carrega configs (base + Development em DEBUG)
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#endif

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(f => f.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Aceitar certificado de dev (apenas DEBUG)
            static HttpMessageHandler DevHandlerFactory()
            {
                var h = new HttpClientHandler();
#if DEBUG
                h.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif
                return h;
            }

            // Escolhe a base URL conforme plataforma/dispositivo
            var apiBaseUrl = PickApiBaseUrl(builder.Configuration);

            // HTTP Clients tipados
            builder.Services.AddHttpClient<UsersApiService>(c => c.BaseAddress = new Uri(apiBaseUrl))
                            .ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            builder.Services.AddHttpClient<AuthApiService>(c => c.BaseAddress = new Uri(apiBaseUrl))
                            .ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            builder.Services.AddHttpClient<EmployeesApiService>(c => c.BaseAddress = new Uri(apiBaseUrl))
                            .ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            builder.Services.AddHttpClient<IOrganizationalStructureApiService, OrganizationalStructureApiService>(
                                c => c.BaseAddress = new Uri(apiBaseUrl))
                            .ConfigurePrimaryHttpMessageHandler(DevHandlerFactory);

            // ViewModels
            builder.Services.AddScoped<UsersViewModel>();
            builder.Services.AddScoped<UserEditViewModel>();
            builder.Services.AddScoped<LoginViewModel>();

            return builder.Build();
        }

        private static string PickApiBaseUrl(IConfiguration config)
        {
            // Fallback global (web/desktop)
            var fallback = config["ApiBaseUrl"];

#if ANDROID
            var forceEmu = bool.TryParse(config["Android:UseEmulator"], out var b) && b;
            var isVirtual = DeviceInfo.DeviceType == DeviceType.Virtual;

            var baseUrl = (forceEmu || isVirtual)
                ? config["Android:EmulatorBaseUrl"]
                : config["Android:DeviceBaseUrl"];

            return baseUrl ?? fallback ?? throw new InvalidOperationException("Defina ApiBaseUrl em appsettings.");
#elif IOS
            var forceSim = bool.TryParse(config["iOS:UseEmulator"], out var b) && b;
            var isVirtual = DeviceInfo.DeviceType == DeviceType.Virtual;

            var baseUrl = (forceSim || isVirtual)
                ? config["iOS:SimulatorBaseUrl"]
                : config["iOS:DeviceBaseUrl"];

            return baseUrl ?? fallback ?? throw new InvalidOperationException("Defina ApiBaseUrl em appsettings.");
        #else
            return fallback ?? "https://localhost:7006/";
        #endif
        }

    }
}
