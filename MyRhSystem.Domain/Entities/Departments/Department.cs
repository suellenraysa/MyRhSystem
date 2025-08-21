using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.JobRoles;

namespace MyRhSystem.Domain.Entities.Departments;

[Table("departments")]
[Index(nameof(Nome), IsUnique = true, Name = "UX_departments_nome")]
public class Department
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("company_id")]
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; } = default!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //[Column("created_by")]
    //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Relacionamento 1:N
    public ICollection<JobRole> JobRoles { get; set; } = new List<JobRole>();
}
