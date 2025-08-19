using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.ValueObjects;

namespace MyRhSystem.Domain.Entities.Employees
{
    [Table("employees")]
    [Index(nameof(CompanyId))]
    [Index(nameof(Email), IsUnique = true, Name = "IX_employees_email_unique")]
    public class Employee
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // Empresa (FK)
        [Required]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;

        // Pessoais
        [Required, StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = "";

        [Required, StringLength(150)]
        [Column("sobrenome")]
        public string Sobrenome { get; set; } = "";

        [Required, StringLength(10)]
        [Column("sexo")]
        public string Sexo { get; set; } = "";

        [Required]
        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [StringLength(80)]
        [Column("email")]
        public string? Email { get; set; }

        [Required, StringLength(30)]
        [Column("telefone")]
        public string Telefone { get; set; } = "";

        [Column("address_id")]
        public int? AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address? Address { get; set; }

        // Profissional
        [Required, StringLength(255)]
        [Column("cargo")]
        public string Cargo { get; set; } = "";

        [Required, StringLength(255)]
        [Column("departamento")]
        public string Departamento { get; set; } = "";

        [Required, StringLength(255)]
        [Column("funcao")]
        public string Funcao { get; set; } = "";

        [Required]
        [Column("ativo")]
        public bool Ativo { get; set; } = true;

        // Auditoria
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // ---------------------------
        // Navegações (serão mapeadas nas próximas entidades/tabelas)
        // ---------------------------

        // Contatos de emergência (employee_contacts) – 1:N
         public ICollection<EmployeeContacts> Contacts { get; set; } = new List<EmployeeContacts>();

        // Dependentes (employee_dependents) – 1:N
        public ICollection<EmployeeDependents> Dependents { get; set; } = new List<EmployeeDependents>();

        // Documentos (employee_documents) – 1:1
        public EmployeeDocuments? Documents { get; set; }

        // Contrato (employee_contracts) – 1:1
         public EmployeeContract? Contract { get; set; }

        // Benefícios (employee_benefits) – N:N via tabela de junção
         public ICollection<EmployeeBenefits> EmployeeBenefits { get; set; } = new List<EmployeeBenefits>();
    }
}
