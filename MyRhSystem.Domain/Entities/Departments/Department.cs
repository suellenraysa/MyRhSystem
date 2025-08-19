using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
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

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamento 1:N
    public ICollection<JobRole> JobRoles { get; set; } = new List<JobRole>();
}
