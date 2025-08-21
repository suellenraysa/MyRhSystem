using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyRhSystem.Domain.Entities.Departments;
using MyRhSystem.Domain.Entities.Employees;
using MyRhSystem.Domain.Entities.JobRoles;
using MyRhSystem.Domain.Entities.ValueObjects;

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

    // ========= Auditoria =========
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<JobLevels> JobLevels { get; set; } = new List<JobLevels>();
    public ICollection<JobRole> JobRoles { get; set; } = new List<JobRole>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
