using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyRhSystem.Domain.Entities.Employees;

[Table("employee_contacts")]
public class EmployeeContacts
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(50)]
    [Column("grau_parentesco")]
    public string? GrauParentesco { get; set; }

    [Required, StringLength(20)]
    [Column("telefone")]
    public string Telefone { get; set; } = string.Empty;

    [Column("created_at")] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 🔗 Relacionamento com Employees
    [ForeignKey("Employee")]
    [Column("employee_id")]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = default!;
}
