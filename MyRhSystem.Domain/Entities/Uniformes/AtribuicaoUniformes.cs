using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Employees;

namespace MyRhSystem.Domain.Entities.Uniformes;

[Table("atribuicao_uniformes")]
[Index(nameof(FuncionarioId), Name = "IX_atrib_uniformes_funcionario")]
[Index(nameof(ItensUniformesId), Name = "IX_atrib_uniformes_item")]
public class AtribuicaoUniformes
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    // FK -> employees.id
    [Required]
    [Column("funcionario_id")]
    public int FuncionarioId { get; set; }

    [ForeignKey(nameof(FuncionarioId))]
    public Employee Employee { get; set; } = null!; // ou 'Employees' se essa for a sua classe

    // FK -> itens_uniformes.id
    [Required]
    [Column("itens_uniformes_id")]
    public int ItensUniformesId { get; set; }

    [ForeignKey(nameof(ItensUniformesId))]
    public ItensUniformes ItemUniforme { get; set; } = null!;

    [Required]
    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Required]
    [Column("data_entrega")]
    public DateTime DataEntrega { get; set; }

    [Column("data_retornada")]
    public DateTime? DataRetornada { get; set; }

    [Column("data_devolucao")]
    public DateTime? DataDevolucao { get; set; }

    [StringLength(255)]
    [Column("condicoes")]
    public string? Condicoes { get; set; }
}
