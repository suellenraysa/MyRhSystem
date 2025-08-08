using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.Payroll;
using MyRhSystem.Domain.Entities.Uniformes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Funcionarios;

[Table("funcionarios")]
public class Funcionario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("company_id")]
    public int? CompanyId { get; set; }

    [Column("nome")]
    public string? Nome { get; set; }

    [Column("sobrenome")]
    public string? Sobrenome { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("telefone")]
    public string? Telefone { get; set; }

    [Column("posicao")]
    public string? Posicao { get; set; }

    [Column("data_de_admissao")]
    public DateTime? DataDeAdmissao { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }

    public ICollection<FolhaDePagamento> FolhasDePagamentos { get; set; } = new List<FolhaDePagamento>();
    public ICollection<AtribuicaoUniformes> UniformAssignments { get; set; } = new List<AtribuicaoUniformes>();


}
