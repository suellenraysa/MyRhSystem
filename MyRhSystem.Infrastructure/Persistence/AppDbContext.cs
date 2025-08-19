using Microsoft.EntityFrameworkCore;
using MyRhSystem.Application.Abstractions;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.Departments;
using MyRhSystem.Domain.Entities.Employees;
using MyRhSystem.Domain.Entities.JobRoles;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Domain.Entities.ValueObjects;

namespace MyRhSystem.Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        // ==== Usuários & Empresas ====
        public DbSet<User> Users => Set<User>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<UserCompany> UserCompanies => Set<UserCompany>();

        // ==== Funcionários ====
        public DbSet<Employee> Employees => Set<Employee>();

        // ==== Estrutura Organizacional ====
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<JobRole> JobRoles => Set<JobRole>();
        public DbSet<JobLevels> JobLevels => Set<JobLevels>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // ===== Users =====
            mb.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Nome).HasMaxLength(255).IsRequired();
                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();
            });

            // ===== UserCompany (chave composta) =====
            mb.Entity<UserCompany>()
                .HasKey(uc => new { uc.UserId, uc.CompanyId });

            mb.Entity<UserCompany>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCompanies)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            mb.Entity<UserCompany>()
                .HasOne(uc => uc.Company)
                .WithMany(c => c.UserCompanies)
                .HasForeignKey(uc => uc.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Company: Address como Value Object =====
            mb.Entity<Company>().OwnsOne<Address>(c => c.Address, nav =>
            {
                nav.Property(a => a.Endereco).HasColumnName("address_street").HasMaxLength(255);
                nav.Property(a => a.Numero).HasColumnName("address_number").HasMaxLength(50);
                nav.Property(a => a.Bairro).HasColumnName("address_district").HasMaxLength(120);
                nav.Property(a => a.Cidade).HasColumnName("address_city").HasMaxLength(120);
                nav.Property(a => a.UF).HasColumnName("address_state").HasMaxLength(2);
                nav.Property(a => a.Cep).HasColumnName("address_zipcode").HasMaxLength(20);
                nav.Property(a => a.Complemento).HasColumnName("address_complement").HasMaxLength(150);
            });

            // ===== Employee: Address como Value Object =====
            mb.Entity<Employee>().OwnsOne<Address>(e => e.Address, nav =>
            {
                nav.Property(a => a.Endereco).HasColumnName("address_street").HasMaxLength(255);
                nav.Property(a => a.Numero).HasColumnName("address_number").HasMaxLength(50);
                nav.Property(a => a.Bairro).HasColumnName("address_district").HasMaxLength(120);
                nav.Property(a => a.Cidade).HasColumnName("address_city").HasMaxLength(120);
                nav.Property(a => a.UF).HasColumnName("address_state").HasMaxLength(2);
                nav.Property(a => a.Cep).HasColumnName("address_zipcode").HasMaxLength(20);
                nav.Property(a => a.Complemento).HasColumnName("address_complement").HasMaxLength(150);
            });

            // ===== Departments =====
            mb.Entity<Department>(e =>
            {
                e.HasKey(d => d.Id);
                e.Property(d => d.Nome).HasMaxLength(150).IsRequired();
                e.HasIndex(d => d.Nome).HasDatabaseName("UX_departments_nome").IsUnique(false);
            });

            // ===== JobLevels =====
            mb.Entity<JobLevels>(e =>
            {
                e.HasKey(l => l.Id);
                e.Property(l => l.Nome).HasMaxLength(100).IsRequired();
                e.Property(l => l.Ordem).IsRequired();
                e.HasIndex(l => l.Nome).HasDatabaseName("UX_job_levels_nome").IsUnique(true);
            });

            // (Opcional) Seed de níveis padrão
            mb.Entity<JobLevels>().HasData(
                new JobLevels { Id = 1, Nome = "Estagiário", Ordem = 1 },
                new JobLevels { Id = 2, Nome = "Júnior", Ordem = 2 },
                new JobLevels { Id = 3, Nome = "Pleno", Ordem = 3 },
                new JobLevels { Id = 4, Nome = "Sênior", Ordem = 4 },
                new JobLevels { Id = 5, Nome = "Trainer", Ordem = 5 }
            );

            // ===== JobRoles =====
            mb.Entity<JobRole>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.Nome).HasMaxLength(150).IsRequired();

                e.Property(r => r.SalarioBase).HasColumnType("decimal(18,2)").IsRequired();
                e.Property(r => r.SalarioMaximo).HasColumnType("decimal(18,2)");
                e.Property(r => r.Requisitos);
                e.Property(r => r.Responsabilidades);

                // FK: Department
                e.HasOne(r => r.Department)
                 .WithMany(d => d.JobRoles)
                 .HasForeignKey(r => r.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);

                // FK: JobLevel
                e.HasOne(r => r.Level)
                 .WithMany() // se quiser reverso: adicione ICollection<JobRoles> em JobLevels e troque por .WithMany(l => l.JobRoles)
                 .HasForeignKey(r => r.LevelId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Índice auxiliar
                e.HasIndex(r => new { r.Nome, r.DepartmentId })
                 .HasDatabaseName("UX_job_roles_nome_department")
                 .IsUnique(false);
            });
        }
    }
}
