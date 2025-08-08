using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Uniformes;

[Table("itens_uniformes")]
public class ItensUniformes
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("code")]
    public string? Code { get; set; }

    [Column("descricao")]
    public string? Descricao { get; set; }

    [Column("tamanho")]
    public string? Tamanho { get; set; }

    [Column("total_estoque")]
    public int TotalEstoque { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    public ICollection<AtribuicaoUniformes> Assignments { get; set; } = new List<AtribuicaoUniformes>();
}
