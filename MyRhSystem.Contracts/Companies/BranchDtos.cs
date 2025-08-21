using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Contracts.Departments;

public class BranchDtos
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Nome { get; set; } = string.Empty;
}
public record BranchRequestDto
{
    public int? Id { get; init; }
    public int CompanyId { get; init; }
    public string Nome { get; init; } = string.Empty;
}

public record BranchResponseDto
{
    public int Id { get; init; }
    public int CompanyId { get; init; }
    public string Nome { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}