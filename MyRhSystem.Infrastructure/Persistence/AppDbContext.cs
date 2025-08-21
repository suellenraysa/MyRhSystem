using Microsoft.EntityFrameworkCore;
using MyRhSystem.APP.Shared.Models;
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

        public DbSet<Reminder> Reminders => Set<Reminder>();
        
        public DbSet<LegalRepresentativeModel> LegalRepresentatives => Set<LegalRepresentativeModel>();
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // ===== Users =====
            mb.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Nome).HasMaxLength(255).IsRequired();
                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();
            });

            // ===== Companies =====
            mb.Entity<Company>(e =>
            {
                e.ToTable("companies");
                e.HasKey(c => c.Id);

                e.HasOne(c => c.Address)
                    .WithMany()
                    .HasForeignKey(c => c.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(c => c.UserCompanies)
                    .WithOne(uc => uc.Company)
                    .HasForeignKey(uc => uc.CompanyId);

                e.HasMany(c => c.Employees)
                    .WithOne(emp => emp.Company)
                    .HasForeignKey(emp => emp.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                // 1:1 -> empresa tem um representante, representante só pode estar numa empresa
                e.HasOne(c => c.Representative)
                    .WithOne(r => r.Company)
                    .HasForeignKey<Company>(c => c.RepresentativeId)
                    .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(c => c.CreatedBy)
                      .WithMany() // se não precisar navegação reversa
                      .HasForeignKey(c => c.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            mb.Entity<Reminder>(e =>
            {
                e.HasIndex(x => new { x.CompanyId, x.IsDone });
                e.HasIndex(x => x.RemindAt);
                e.HasIndex(x => x.DueAt);

                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.Notes).HasMaxLength(4000);
            });

            mb.Entity<LegalRepresentativeModel>(e =>
            {
                e.ToTable("legal_representatives");
                e.HasKey(r => r.Id);

                e.Property(r => r.FullName).HasMaxLength(255).IsRequired();
                e.Property(r => r.CPF).HasMaxLength(20).IsRequired();
                e.HasIndex(r => r.CPF).IsUnique(); // garante que CPF seja único
            });

            mb.Entity<Address>(e =>
            {
                e.ToTable("addresses");
                e.HasKey(a => a.Id);

                // ajuste os nomes/limites conforme seu VO
                e.Property(a => a.Endereco).HasColumnName("street").HasMaxLength(255);
                e.Property(a => a.Numero).HasColumnName("number").HasMaxLength(50);
                e.Property(a => a.Bairro).HasColumnName("district").HasMaxLength(120);
                e.Property(a => a.Cidade).HasColumnName("city").HasMaxLength(120);
                e.Property(a => a.UF).HasColumnName("state").HasMaxLength(2);
                e.Property(a => a.Cep).HasColumnName("zipcode").HasMaxLength(20);
                e.Property(a => a.Complemento).HasColumnName("complement").HasMaxLength(150);
            });

            // ===== Employees =====
            mb.Entity<Employee>(e =>
            {
                e.ToTable("employees");
                e.HasKey(x => x.Id);

                e.Property(x => x.CompanyId).HasColumnName("company_id");
                e.HasOne(x => x.Company)
                 .WithMany(c => c.Employees)
                 .HasForeignKey(x => x.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                // campos (mantive seus nomes)
                e.Property(x => x.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
                e.Property(x => x.Sobrenome).HasColumnName("sobrenome").HasMaxLength(150).IsRequired();
                e.Property(x => x.Sexo).HasColumnName("sexo").HasMaxLength(10).IsRequired();
                e.Property(x => x.DataNascimento).HasColumnName("data_nascimento").IsRequired();
                e.Property(x => x.Email).HasColumnName("email").HasMaxLength(80);
                e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(30).IsRequired();
                
                e.Property(x => x.AddressId).HasColumnName("address_id");
                e.HasOne(x => x.Address)
                 .WithMany()
                 .HasForeignKey(x => x.AddressId)
                 .OnDelete(DeleteBehavior.Restrict); // idem

                e.Property(x => x.Cargo).HasColumnName("cargo").HasMaxLength(255).IsRequired();
                e.Property(x => x.Departamento).HasColumnName("departamento").HasMaxLength(255).IsRequired();
                e.Property(x => x.Funcao).HasColumnName("funcao").HasMaxLength(255).IsRequired();
                e.Property(x => x.Ativo).HasColumnName("ativo").IsRequired();
                e.Property(x => x.CreatedAt).HasColumnName("created_at");
                e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            });

            // ===== UserCompany (chave composta) =====
            mb.Entity<UserCompany>(e =>
            {
                e.ToTable("user_companies");

                e.HasKey(uc => new { uc.UserId, uc.CompanyId });

                e.HasOne(uc => uc.User)
                 .WithMany(u => u.UserCompanies)
                 .HasForeignKey(uc => uc.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(uc => uc.Company)
                 .WithMany(c => c.UserCompanies)
                 .HasForeignKey(uc => uc.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== Departments =====
            mb.Entity<Department>(e =>
            {
                e.ToTable("departments");
                e.HasKey(d => d.Id);
                e.Property(d => d.Nome).HasMaxLength(150).IsRequired();
                e.HasIndex(d => d.Nome).HasDatabaseName("UX_departments_nome").IsUnique(false);
            });

            // ===== JobLevels =====
            mb.Entity<JobLevels>(e =>
            {
                e.ToTable("job_levels");
                e.HasKey(l => l.Id);
                e.Property(l => l.Nome).HasMaxLength(100).IsRequired();
                e.Property(l => l.Ordem).IsRequired();
                e.HasIndex(l => l.Nome).HasDatabaseName("UX_job_levels_nome").IsUnique(true);
            });

            

            // ===== JobRoles =====
            mb.Entity<JobRole>(e =>
            {
                e.ToTable("job_roles");

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
                 .WithMany() // ou .WithMany(l => l.JobRoles)
                 .HasForeignKey(r => r.LevelId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(r => new { r.Nome, r.DepartmentId })
                 .HasDatabaseName("UX_job_roles_nome_department")
                 .IsUnique(false);
            });

        }
    }
}
