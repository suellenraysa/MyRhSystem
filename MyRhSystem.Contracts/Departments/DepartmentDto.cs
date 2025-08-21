using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Contracts.Departments;

public class DepartmentDto
{
    public int Id { get; init; }
    public int CompanyId { get; init; }    
    [Required, StringLength(150)]
    public string Nome { get; set; } = string.Empty;
}
