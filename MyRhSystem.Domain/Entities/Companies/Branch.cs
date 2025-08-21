using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyRhSystem.Domain.Entities.ValueObjects;

namespace MyRhSystem.Domain.Entities.Companies;

[Table("branches")]
public class Branch
{
    [Key]
    [Column("id")]
    public int Id { get; set; }    

    [Column("nome")]
    [MaxLength(255)]
    public string Nome { get; set; } = string.Empty;

    [Column("address_id")]
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    [Column("company_id")]
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = default!;

    // ========= Auditoria =========
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}
