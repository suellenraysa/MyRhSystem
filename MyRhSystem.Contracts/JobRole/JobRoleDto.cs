using System.ComponentModel.DataAnnotations;
using MyRhSystem.Contracts.Departments;

namespace MyRhSystem.Contracts.JobRole;

public class JobRoleDto
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int DepartmentId { get; set; } // Foreign key to Department

    [Required, StringLength(50)]
    public int LevelId { get; set; }

    [Required]
    public decimal SalarioBase { get; set; }

    public decimal? SalarioMaximo { get; set; }

    [StringLength(1000)]
    public string? Requisitos { get; set; }

    [StringLength(2000)]
    public string? Responsabilidades { get; set; }
}
