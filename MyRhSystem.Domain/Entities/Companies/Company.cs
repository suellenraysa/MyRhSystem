using MyRhSystem.APP.Shared.Models;
using MyRhSystem.Domain.Entities.Employees;
using MyRhSystem.Domain.Entities.Users;
using MyRhSystem.Domain.Entities.ValueObjects;
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

    [Column("cnpj")]
    public string? Cnpj { get; set; }

    [Column("telefone")]
    public string? Telefone { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("address_id")]
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address? Address { get; set; }

    [Column("representative_id")] 
    public int? RepresentativeId { get; set; }
        
    public LegalRepresentativeModel? Representative { get; set; }

    [Column("created_by_user_id")]
    public int? CreatedByUserId { get; set; }

    [ForeignKey(nameof(CreatedByUserId))]
    public User? CreatedBy { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
