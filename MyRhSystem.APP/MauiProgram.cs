using Microsoft.Extensions.Logging;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Application.Employees;
using MyRhSystem.Infrastructure.Employees;


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
            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
            builder.Services.AddHttpClient<CompanyRegisterApiService>(client =>
            {
                client.BaseAddress = new Uri("http://10.0.2.2:5000/");
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
