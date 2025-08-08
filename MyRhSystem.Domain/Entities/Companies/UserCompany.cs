using MyRhSystem.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Companies;

[Table("user_companies")]
public class UserCompany
{

    [Key, Column("user_id", Order = 0)]
    public int UserId { get; set; }

    [Key, Column("company_id", Order = 1)]
    public int CompanyId { get; set; }

    [Column("role")]
    public string? Role { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company? Company { get; set; }

}
