using Microsoft.AspNetCore.Components;

namespace MyRhSystem.APP.Shared.ViewModels
{
    public class CompanyBasicInfoViewModel : ComponentBase
    {
        public string CompanyName { get; set; } = "Empresa Exemplo LTDA";
        public string CNPJ { get; set; } = "00.000.000/0000-00";
        public string Phone { get; set; } = "(11) 99999-9999";
        public string Email { get; set; } = "contato@empresa.com.br";
    }
}
