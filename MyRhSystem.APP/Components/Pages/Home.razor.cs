using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Web;

namespace MyRhSystem.APP.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private NavigationManager Nav { get; set; } = default!;

    // Se/Quando você tiver um serviço real, injete aqui:
    // [Inject] private ICompanyService CompanyService { get; set; } = default!;

    protected string Search { get; set; } = string.Empty;
    protected CompanyVM? Selected { get; set; }
    protected List<CompanyVM> Companies { get; set; } = new();

    protected IEnumerable<CompanyVM> Filtered =>
        string.IsNullOrWhiteSpace(Search)
            ? Companies
            : Companies.Where(c =>
                (!string.IsNullOrEmpty(c.Name) && c.Name.Contains(Search, StringComparison.InvariantCultureIgnoreCase)) ||
                (!string.IsNullOrEmpty(c.Sector) && c.Sector.Contains(Search, StringComparison.InvariantCultureIgnoreCase)));

    protected override async Task OnInitializedAsync()
    {
        // TODO: troque este mock por chamada ao seu serviço.
        // var list = await CompanyService.GetAllAsync();
        // Companies = list.Select(x => new CompanyVM(x.Name, x.Sector, x.EmployeeRange, x.City, x.State, x.Website)).ToList();

        Companies = new()
        {
            new("TechCorp", "Tecnologia", "51-200 funcionários", "São Paulo", "SP", "https://techcorp.com"),
            new("InovaDigital", "Tecnologia", "11-50 funcionários", "Rio de Janeiro", "RJ", "https://inovadigital.com"),
            new("Comercial Santos & Filhos", "Comércio", "1-10 funcionários", "Belo Horizonte", "MG", ""),
            new("Horizonte", "Construção", "201-500 funcionários", "Porto Alegre", "RS", "https://horizonteconstrucoes.com"),
        };

        await Task.CompletedTask;
    }

    protected void Select(CompanyVM c) => Selected = c;

    protected void Cancel() => Selected = null;

    protected void GoToCreate() => Nav.NavigateTo("/company-registration");

    protected void Gerenciar()
    {
        if (Selected is null) return;

        // Ajuste a rota de gerenciamento conforme sua app
        var companyQuery = HttpUtility.UrlEncode(Selected.Name);
        Nav.NavigateTo($"/dashboard?company={companyQuery}");
    }

    // Você pode mover este record para Shared/ViewModels depois, se preferir.
    public record CompanyVM(string Name, string Sector, string EmployeeRange, string City, string State, string? Website)
    {
        public string? WebsiteHost =>
            string.IsNullOrWhiteSpace(Website) ? null : new Uri(Website).Host.Replace("www.", "");
    }
}
