using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.APP.Shared.Models;

public class LegalRepresentativeModel
{
    [Required(ErrorMessage = "Nome completo é obrigatório")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "CPF é obrigatório")]
    [RegularExpression(@"\d{3}\.\d{3}\.\d{3}-\d{2}", ErrorMessage = "CPF deve estar no formato 000.000.000-00")]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [Phone(ErrorMessage = "Telefone inválido")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
}
