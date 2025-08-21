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
        // ==== Usuários, Empresas & Filiais ====
        public DbSet<User> Users => Set<User>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Branch> Branches => Set<Branch>();

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

                e.Property(c => c.Nome).HasColumnName("nome").HasMaxLength(255);
                e.Property(c => c.CreatedAt).HasColumnName("createdAt");
                e.Property(c => c.UpdatedAt).HasColumnName("updated_at");
                e.Property(c => c.AddressId).HasColumnName("address_id");

                // FK opcional para Address
                e.HasOne(c => c.Address)
                 .WithMany()
                 .HasForeignKey(c => c.AddressId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== Branches (Filiais) =====
            // Mapeamento mínimo — ajuste/complete conforme a sua entidade Branch
            mb.Entity<Branch>(e =>
            {
                e.ToTable("branches");
                e.HasKey(b => b.Id);

                e.Property(b => b.CompanyId).HasColumnName("company_id").IsRequired();
                e.Property(b => b.Nome).HasColumnName("nome").HasMaxLength(255);
                e.Property(b => b.AddressId).HasColumnName("address_id");
                e.Property(b => b.CreatedAt).HasColumnName("createdAt");
                e.Property(b => b.UpdatedAt).HasColumnName("updated_at");

                e.HasOne(b => b.Company)
                 .WithMany(c => c.Branches)
                 .HasForeignKey(b => b.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(b => b.Address)
                 .WithMany()
                 .HasForeignKey(b => b.AddressId)
                 .OnDelete(DeleteBehavior.Restrict);

                // Evita nomes repetidos de filial dentro da MESMA empresa
                e.HasIndex(b => new { b.CompanyId, b.Nome })
                 .HasDatabaseName("UX_branches_company_nome")
                 .IsUnique(false);
            });

            // ===== Addresses (VO tabelado) =====
            mb.Entity<Address>(e =>
            {
                e.ToTable("addresses");
                e.HasKey(a => a.Id);

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
                 .OnDelete(DeleteBehavior.Restrict);

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

            // ===== Departments (escopo por empresa) =====
            mb.Entity<Department>(e =>
            {
                e.ToTable("departments");
                e.HasKey(d => d.Id);

                e.Property(d => d.CompanyId).HasColumnName("company_id").IsRequired();
                e.Property(d => d.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
                e.Property(d => d.CreatedAt).HasColumnName("created_at");
                e.Property(d => d.UpdatedAt).HasColumnName("updated_at");

                e.HasOne(d => d.Company)
                 .WithMany(c => c.Departments)
                 .HasForeignKey(d => d.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Nome pode repetir entre empresas, mas não é desejável colidir no mesmo company
                e.HasIndex(d => new { d.CompanyId, d.Nome })
                 .HasDatabaseName("UX_departments_company_nome")
                 .IsUnique(false);
            });

            // ===== JobLevels (escopo por empresa) =====
            mb.Entity<JobLevels>(e =>
            {
                e.ToTable("job_levels");
                e.HasKey(l => l.Id);

                e.Property(l => l.CompanyId).HasColumnName("company_id").IsRequired();
                e.Property(l => l.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
                e.Property(l => l.CreatedAt).HasColumnName("created_at");
                e.Property(l => l.UpdatedAt).HasColumnName("updated_at");

                e.HasOne(l => l.Company)
                 .WithMany(c => c.JobLevels)
                 .HasForeignKey(l => l.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(l => new { l.CompanyId, l.Nome })
                 .HasDatabaseName("UX_job_levels_company_nome")
                 .IsUnique(false);

                // se quiser garantir ordem única por empresa:
                // e.HasIndex(l => new { l.CompanyId, l.Ordem })
                //  .HasDatabaseName("UX_job_levels_company_ordem")
                //  .IsUnique(false);
            });

            // ===== JobRoles (escopo por empresa) =====
            mb.Entity<JobRole>(e =>
            {
                e.ToTable("job_roles");
                e.HasKey(r => r.Id);

                e.Property(r => r.CompanyId).HasColumnName("company_id").IsRequired();
                e.Property(r => r.DepartmentId).HasColumnName("department_id").IsRequired();
                e.Property(r => r.LevelId).HasColumnName("level_id").IsRequired();

                e.Property(r => r.Nome).HasColumnName("nome").HasMaxLength(150).IsRequired();
                e.Property(r => r.SalarioBase).HasColumnName("salario_base").HasColumnType("decimal(18,2)").IsRequired();
                e.Property(r => r.SalarioMaximo).HasColumnName("salario_maximo").HasColumnType("decimal(18,2)");
                e.Property(r => r.Requisitos).HasColumnName("requisitos");
                e.Property(r => r.Responsabilidades).HasColumnName("responsabilidades");
                e.Property(r => r.CreatedAt).HasColumnName("created_at");
                e.Property(r => r.UpdatedAt).HasColumnName("updated_at");

                e.HasOne(r => r.Company)
                 .WithMany(c => c.JobRoles)
                 .HasForeignKey(r => r.CompanyId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(r => r.Department)
                 .WithMany(d => d.JobRoles)
                 .HasForeignKey(r => r.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Level)
                 .WithMany(l => l.JobRoles)
                 .HasForeignKey(r => r.LevelId)
                 .OnDelete(DeleteBehavior.Restrict);

                // evita colisão de nomes de cargo dentro do mesmo dept/empresa
                e.HasIndex(r => new { r.CompanyId, r.DepartmentId, r.Nome })
                 .HasDatabaseName("UX_job_roles_company_dept_nome")
                 .IsUnique(false);
            });
        }
    }
}
