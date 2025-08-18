using Microsoft.AspNetCore.Components;

namespace MyRhSystem.APP.Shared.ViewModels
{
    public class CompanyAddressViewModel : ComponentBase
    {
        public string FullAddress { get; set; } = "Av. Paulista, 1000 - Bela Vista";
        public string City { get; set; } = "São Paulo";
        public string State { get; set; } = "SP";
        public string ZipCode { get; set; } = "01310-100";
    }
}
