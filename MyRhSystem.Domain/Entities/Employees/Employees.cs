using MyRhSystem.Domain.Entities.Companies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Employees;

[Table("employees")]
public class Employees
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public required string Nome { get; set; }

    [Column("sobrenome")]
    public required string Sobrenome { get; set; }

    [Column("sexo")]
    public required string Sexo { get; set; }

    [Column("data_nascimento")]
    public required DateTime DataNascimento { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("telefone")]
    public required string Telefone { get; set; }
    [Column("cargo")]
    public required string Cargo { get; set; }
    [Column("departamento")]
    public required string Departamento { get; set; }
    [Column("funcao")]
    public required string Funcao { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = null!;

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

}

