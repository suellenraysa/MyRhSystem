using Microsoft.AspNetCore.Components;
using MyRhSystem.Application.Employees;

namespace MyRhSystem.APP.Components.Pages;

public partial class FuncionariosBase : ComponentBase
{
    [Inject] private IEmployeeService Service { get; set; } = default!;

    // Lista/tabela (leve)
    protected List<EmployeeDto> Employees { get; set; } = new();
    protected IEnumerable<EmployeeDto> Filtered { get; set; } = Enumerable.Empty<EmployeeDto>();
    protected List<string> Departments { get; set; } = new();

    protected string Search { get; set; } = string.Empty;
    protected string DeptFilter { get; set; } = string.Empty;

    // Modal
    protected bool ShowForm { get; set; }
    protected bool IsViewOnly { get; set; }
    protected string FormTitle { get; set; } = string.Empty;

    // Modelo detalhado para ver/editar/criar
    protected EmployeeDetailsDto Details { get; set; } = new();

    protected bool ShowConfirmDelete { get; set; }

    // Contagens
    protected int TotalCount { get; set; }
    protected Dictionary<string, int> DeptCounts { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        Employees = (await Service.GetAllAsync()).ToList();

        Departments = Employees
            .Select(x => x.Departamento)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        DeptCounts = Employees
            .GroupBy(e => e.Departamento ?? "")
            .ToDictionary(g => g.Key, g => g.Count());

        ApplyFilter();
        StateHasChanged();
    }

    protected void ApplyFilter()
    {
        Filtered = Employees.Where(e =>
            (string.IsNullOrWhiteSpace(Search)
                || e.Nome.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                || e.Email.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                || e.Cargo.Contains(Search, StringComparison.InvariantCultureIgnoreCase))
            && (string.IsNullOrWhiteSpace(DeptFilter) || e.Departamento == DeptFilter));

        TotalCount = Filtered.Count();
    }

    // Actions
    protected void NewEmployee()
    {
        Details = new EmployeeDetailsDto
        {
            Contratacao = DateTime.Today,
            UF = "SP",
            Ativo = true
        };
        IsViewOnly = false;
        FormTitle = "Novo Funcionário";
        ShowForm = true;
    }

    protected void View(EmployeeDto f)
    {
        Details = ToDetails(f);
        IsViewOnly = true;
        FormTitle = "Detalhes do Funcionário";
        ShowForm = true;
    }

    protected void Edit(EmployeeDto f)
    {
        Details = ToDetails(f);
        IsViewOnly = false;
        FormTitle = "Editar Funcionário";
        ShowForm = true;
    }

    protected void CloseForm() => ShowForm = false;

    protected async Task SaveAsync()
    {
        // Se o seu service já tem Create/Update com Details, use-os:
        if (Details.Id == Guid.Empty)
            await Service.CreateAsync(Details);   // overload para EmployeeDetailsDto
        else
            await Service.UpdateAsync(Details);   // overload para EmployeeDetailsDto

        // Caso ainda não tenha overloads, remapeie:
        // var dto = ToSummary(Details);
        // if (dto.Id == Guid.Empty) await Service.CreateAsync(dto); else await Service.UpdateAsync(dto);

        ShowForm = false;
        await ReloadAsync();
    }

    protected void ConfirmDelete(EmployeeDto f)
    {
        Details = ToDetails(f);
        ShowConfirmDelete = true;
    }

    protected void CancelDelete() => ShowConfirmDelete = false;

    protected async Task DeleteAsync()
    {
        await Service.DeleteAsync(Details.Id);
        ShowConfirmDelete = false;
        await ReloadAsync();
    }

    // Mapeamentos básicos (resumo <-> detalhes)
    private static EmployeeDetailsDto ToDetails(EmployeeDto e) => new()
    {
        Id = e.Id,
        Nome = e.Nome,
        Sobrenome = e.Sobrenome,          
        Cargo = e.Cargo,
        Departamento = e.Departamento,
        Email = e.Email,
        Telefone = e.Telefone,
        Cidade = e.Cidade,
        UF = e.UF,
        Contratacao = e.Contratacao,
        Salario = e.Salario,
        Ativo = e.Ativo,
        Logradouro = e.Endereco, 
        Bairro = e.Bairro,
        Cep = e.CEP
    };

    //private static EmployeeDto ToSummary(EmployeeDetailsDto d) => new()
    //{
    //    Id = d.Id,
    //    Nome = d.Nome,
    //    Cargo = d.Cargo,
    //    Departamento = d.Departamento,
    //    Email = d.Email,
    //    Telefone = d.Telefone,
    //    Cidade = d.Cidade,
    //    UF = d.UF,
    //    Contratacao = d.Contratacao ?? DateTime.Today,
    //    Salario = d.Salario,
    //    Ativo = d.Ativo,

    //    // Se o seu dto de lista tiver estes campos:
    //    Endereco = d.Logradouro,
    //    Bairro = d.Bairro,
    //    CEP = d.Cep
    //};
}
