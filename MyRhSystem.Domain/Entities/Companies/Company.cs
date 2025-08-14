using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Companies;

[Table("companies")]
public class Company
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string? Nome { get; set; }

    [Column("address")]
    public string? Address { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
    //public ICollection<Employee> Funcionarios { get; set; } = new List<Employee>();

}
