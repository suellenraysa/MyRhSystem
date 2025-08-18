using Microsoft.AspNetCore.Components;

namespace MyRhSystem.APP.Shared.ViewModels
{
    public class LegalRepresentativeViewModel : ComponentBase
    {
        public string FullName { get; set; } = "João da Silva";
        public string CPF { get; set; } = "123.456.789-00";
        public string Phone { get; set; } = "(11) 91234-5678";
        public string Email { get; set; } = "joao.silva@empresa.com.br";
    }
}
