using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyRhSystem.Domain.Entities.ValueObjects;
using MyRhSystem.Domain.Entities.Employees;

namespace MyRhSystem.Domain.Entities.Companies;

[Table("companies")]
public class Company
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string? Nome { get; set; }
    
    [Column("address_id")]
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
