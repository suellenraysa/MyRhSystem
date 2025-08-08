using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.APP.Shared.Models;
public class CompanyAddressModel
{
    [Required(ErrorMessage = "Endereço é obrigatório")]
    public string FullAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cidade é obrigatória")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Estado é obrigatório")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "CEP é obrigatório")]
    [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "CEP deve estar no formato 00000-000")]
    public string ZipCode { get; set; } = string.Empty;
}
