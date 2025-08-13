using MyRhSystem.Domain.Entities.Companies;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Users;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string? Nome { get; set; }

    [Column("email")]
    public string? Email { get; set; }


    [Column("password")]
    public string? Password { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();

}
