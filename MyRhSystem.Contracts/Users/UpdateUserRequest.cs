using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Contracts.Users;

public class UpdateUserRequest
{
    [Required] public int Id { get; set; }
    [Required, StringLength(255)] public string Nome { get; set; } = "";
    [Required, EmailAddress, StringLength(255)] public string Email { get; set; } = "";

    // nulo = não alterar; valor = trocar senha
    [StringLength(128, MinimumLength = 6)]
    public string? NewPassword { get; set; }
}
