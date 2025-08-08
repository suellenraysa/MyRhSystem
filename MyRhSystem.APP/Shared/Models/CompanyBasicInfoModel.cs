using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.APP.Shared.Models;
public class CompanyBasicInfoModel
{
    [Required(ErrorMessage = "Razão social é obrigatória")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "CNPJ é obrigatório")]
    public string CNPJ { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;
}
