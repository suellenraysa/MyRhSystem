using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Uniformes;

[Table("atribuicao_uniformes")]
public class AtribuicaoUniformes
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("funcionario_id")]
    public int FuncionarioId { get; set; }

    [Column("itens_uniformes_id")]
    public int ItensUniformesId { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("data_entrega")]
    public DateTime DataEntrega { get; set; }

    [Column("data_retornada")]
    public DateTime? DataRetornada { get; set; }

    [Column("data_devolucao")]
    public DateTime? DataDevolução { get; set; }

    [Column("condicoes")]
    public string? Condicoes { get; set; }

    //[ForeignKey(nameof(EmployeeId))]
    //public Employee? Employee { get; set; }

    [ForeignKey(nameof(ItensUniformesId))]
    public ItensUniformes? ItemUniforme { get; set; }
}
