using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Application.Abstractions;


namespace MyRhSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<User> Users => Set<User>();
    //public DbSet<Company> Companies => Set<Company>();
    //public DbSet<UserCompany> UserCompanies => Set<UserCompany>();
    //public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    //public DbSet<FolhaDePagamento> FolhaDePagamentos => Set<FolhaDePagamento>();
    //public DbSet<ItensUniformes> ItensUniformes => Set<ItensUniformes>();
    //public DbSet<AtribuicaoUniformes> AtribuicaoUniformes => Set<AtribuicaoUniformes>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Chave composta para relacionamento UserCompany
        mb.Entity<UserCompany>()
            .HasKey(uc => new { uc.UserId, uc.CompanyId });

        // Relacionamentos
        mb.Entity<UserCompany>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCompanies)
            .HasForeignKey(uc => uc.UserId);

        mb.Entity<UserCompany>()
            .HasOne(uc => uc.Company)
            .WithMany(c => c.UserCompanies)
            .HasForeignKey(uc => uc.CompanyId);

        mb.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).HasMaxLength(255).IsRequired();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();
            e.HasIndex(x => x.Email).IsUnique();
        });


    }

}
