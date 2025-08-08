using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.Funcionarios;
using MyRhSystem.Domain.Entities.Payroll;
using MyRhSystem.Domain.Entities.Uniformes;
using MyRhSystem.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;


namespace MyRhSystem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<UserCompany> UserCompanies => Set<UserCompany>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<FolhaDePagamento> FolhaDePagamentos => Set<FolhaDePagamento>();
    public DbSet<ItensUniformes> ItensUniformes => Set<ItensUniformes>();
    public DbSet<AtribuicaoUniformes> AtribuicaoUniformes => Set<AtribuicaoUniformes>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Chave composta para relacionamento UserCompany
        modelBuilder.Entity<UserCompany>()
            .HasKey(uc => new { uc.UserId, uc.CompanyId });

        // Relacionamentos
        modelBuilder.Entity<UserCompany>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCompanies)
            .HasForeignKey(uc => uc.UserId);

        modelBuilder.Entity<UserCompany>()
            .HasOne(uc => uc.Company)
            .WithMany(c => c.UserCompanies)
            .HasForeignKey(uc => uc.CompanyId);

        
    }

}
