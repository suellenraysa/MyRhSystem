
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyRhSystem.APP.Shared.ViewModels;
using MyRhSystem.Application.Services;
using MyRhSystem.Infrastructure.Persistence;
using MyRhSystem.Infrastructure.Seed;


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
            builder.Services.AddHttpClient<CompanyRegisterApiService>(client =>
            {
                client.BaseAddress = new Uri("http://10.0.2.2:5000/");
            });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "hrsystem.db");
                System.Diagnostics.Debug.WriteLine($">>> DB em: {dbPath}");
                options.UseSqlite($"Data Source={dbPath}");

                options.UseSqlite($"Data Source={Path.Combine(FileSystem.AppDataDirectory, "hrsystem.db")}");
            });
            builder.Services.AddTransient<DataSeeder>();
            builder.Services.AddTransient<UserViewModel>();

            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // aqui: cria o arquivo e as tabelas se ainda não existirem
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // se você já gerou migrations e quer aplicá-las:
                db.Database.Migrate();
                // ou, se não usou migrations, apenas crie o schema:
                // db.Database.EnsureCreated();
            }

          

            return app;
        }
    }
}
