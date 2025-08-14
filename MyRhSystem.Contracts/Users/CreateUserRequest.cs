using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.Contracts.Users;

public class CreateUserRequest
{
    [Required, StringLength(255)] public string Nome { get; set; } = "";
    [Required, EmailAddress, StringLength(255)] public string Email { get; set; } = "";
    [Required, StringLength(128, MinimumLength = 6)]
    public string Password { get; set; } = "";
}
