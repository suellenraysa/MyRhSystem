using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.APP.Shared.Models;

public class BankingInfoModel
{
    [Required(ErrorMessage = "Banco é obrigatório")]
    public string Bank { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tipo de conta é obrigatório")]
    public string AccountType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Agência é obrigatória")]
    public string Agency { get; set; } = string.Empty;

    [Required(ErrorMessage = "Conta é obrigatória")]
    public string Account { get; set; } = string.Empty;
}
