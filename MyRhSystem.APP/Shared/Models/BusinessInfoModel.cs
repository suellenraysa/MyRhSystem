using System.ComponentModel.DataAnnotations;

namespace MyRhSystem.APP.Shared.Models;
public class BusinessInfoModel
{
    [Required(ErrorMessage = "Setor é obrigatório")]
    public string Sector { get; set; } = string.Empty;

    [Required(ErrorMessage = "Porte da empresa é obrigatório")]
    public string Size { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Ano de fundação inválido")]
    public int FoundingYear { get; set; }

    [Required(ErrorMessage = "Faturamento mensal é obrigatório")]
    public string MonthlyRevenue { get; set; } = string.Empty;
}
