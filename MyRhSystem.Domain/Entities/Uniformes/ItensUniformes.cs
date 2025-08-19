using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyRhSystem.Domain.Entities.Uniformes;

[Table("itens_uniformes")]
[Index(nameof(Code), IsUnique = true, Name = "UX_itens_uniformes_code")]
public class ItensUniformes
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(50)]
    [Column("code")]
    public string Code { get; set; } = string.Empty;

    [StringLength(200)]
    [Column("descricao")]
    public string? Descricao { get; set; }

    [StringLength(10)]
    [Column("tamanho")]
    public string? Tamanho { get; set; }

    [Required]
    [Column("total_estoque")]
    public int TotalEstoque { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<AtribuicaoUniformes> Assignments { get; set; } = new List<AtribuicaoUniformes>();
}
