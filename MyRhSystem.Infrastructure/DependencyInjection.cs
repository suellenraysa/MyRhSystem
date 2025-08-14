using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRhSystem.Application.Abstractions;
using MyRhSystem.Infrastructure.Persistence;

namespace MyRhSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Default")
                 ?? throw new InvalidOperationException("ConnectionStrings:Default não configurada.");

        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(cs, sql =>
            {
                // mantém as migrations neste assembly
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                // resiliente a quedas transitórias
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));

        // expõe o contexto pela interface para Application/casos de uso
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }
}
