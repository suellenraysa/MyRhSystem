//using Microsoft.AspNetCore.Components;
//using MyRhSystem.Application.Employees;
//using MyRhSystem.APP.Shared.Constants;

//namespace MyRhSystem.APP.Components.Pages;

//public partial class EmployeesBase : ComponentBase
//{
//    [Inject] private IEmployeeService Service { get; set; } = default!;

//    // ------ Dados do grid/filtro ------
//    //protected List<EmployeeDto> Employees { get; private set; } = new();
//    protected IEnumerable<EmployeeDto> Filtered { get; private set; } = Enumerable.Empty<EmployeeDto>();
//    protected List<string> Departments { get; private set; } = new();
//    protected Dictionary<string, int> DeptCounts { get; private set; } = new();
//    protected int TotalCount { get; private set; }

//    private string _search = string.Empty;
//    protected string Search { get => _search; set { _search = value ?? string.Empty; ApplyFilter(); } }

//    private string _deptFilter = string.Empty;
//    protected string DeptFilter { get => _deptFilter; set { _deptFilter = value ?? string.Empty; ApplyFilter(); } }

//    // ------ Modal/Form ------
//    protected bool ShowForm { get; private set; }
//    protected bool IsViewOnly { get; private set; }
//    protected string FormTitle { get; private set; } = string.Empty;
//    protected EmployeeDetailsDto Details { get; private set; } = new();
//    protected int ActiveTab { get; private set; }

//    // ------ Confirmação de exclusão ------
//    protected bool ShowConfirmDelete { get; private set; }

//    protected override async Task OnInitializedAsync() => await ReloadAsync();

//    private async Task ReloadAsync()
//    {
//        Employees = (await Service.GetAllAsync()).ToList();
//        BuildDepartmentsAndCounts();
//        ApplyFilter();
//    }

//    private void BuildDepartmentsAndCounts()
//    {
//        Departments = Employees
//            .Select(e => e.Departamento)
//            .Where(s => !string.IsNullOrWhiteSpace(s))
//            .Distinct(StringComparer.OrdinalIgnoreCase)
//            .OrderBy(s => s)
//            .ToList();

//        DeptCounts = Employees
//            .GroupBy(e => e.Departamento ?? string.Empty, StringComparer.OrdinalIgnoreCase)
//            .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);
//    }

//    protected void ApplyFilter()
//    {
//        var term = Search?.Trim();
//        Filtered = Employees.Where(e =>
//            (string.IsNullOrEmpty(term) ||
//             ContainsCI(e.Nome, term) ||
//             ContainsCI(e.Email, term) ||
//             ContainsCI(e.Cargo, term))
//            && (string.IsNullOrWhiteSpace(DeptFilter) ||
//                string.Equals(e.Departamento, DeptFilter, StringComparison.OrdinalIgnoreCase)));

//        TotalCount = Filtered.Count();
//    }

//    private static bool ContainsCI(string? source, string value) =>
//        source?.Contains(value, StringComparison.OrdinalIgnoreCase) == true;

//    // ------ Abrir/fechar formulário ------
//    protected void NewEmployee()
//    {
//        Details = CreateDefaultDetails();
//        OpenForm("Novo Funcionário", viewOnly: false);
//    }

//    protected Task EditAsync(EmployeeDto f) => OpenFromExistingAsync(f, "Editar Funcionário", viewOnly: false);
//    protected Task ViewAsync(EmployeeDto f) => OpenFromExistingAsync(f, "Detalhes do Funcionário", viewOnly: true);

//    private async Task OpenFromExistingAsync(EmployeeDto? f, string title, bool viewOnly)
//    {
//        if (f is null) return;
//        Details = await Service.GetDetailsAsync(f.Id) ?? new EmployeeDetailsDto { Id = f.Id };
//        OpenForm(title, viewOnly);
//    }

//    private void OpenForm(string title, bool viewOnly)
//    {
//        FormTitle = title;
//        IsViewOnly = viewOnly;
//        ActiveTab = 0;
//        ShowForm = true;
//    }

//    protected void CloseForm() => ShowForm = false;

//    private static EmployeeDetailsDto CreateDefaultDetails() => new()
//    {
//        Ativo = true,
//        UF = "SP",
//        TipoContrato = AppDefaults.WorkRegime.FirstOrDefault() ?? "CLT",
//        JornadaHoras = 40,
//        Admissao = DateTime.Today,
//        HorarioEntrada = new TimeOnly(8, 0),
//        HorarioSaida = new TimeOnly(17, 0),
//        Beneficios = Array.Empty<string>(),
//        Dependentes = new()
//    };

//    // ------ Persistência (chamado pelo <EmployeeForm/>) ------
//    protected async Task HandleSave()
//    {
//        if (Details is null) return;

//        if (Details.Id == Guid.Empty)
//            Details.Id = await Service.CreateAsync(Details);
//        else
//            await Service.UpdateAsync(Details);

//        ShowForm = false;
//        await ReloadAsync();
//    }

//    // ------ Exclusão ------
//    protected void ConfirmDelete(EmployeeDto f)
//    {
//        if (f is null) return;
//        Details = new EmployeeDetailsDto { Id = f.Id, Nome = f.Nome };
//        ShowConfirmDelete = true;
//    }

//    protected void CancelDelete() => ShowConfirmDelete = false;

//    protected async Task DeleteAsync()
//    {
//        if (Details.Id == Guid.Empty) return;
//        await Service.DeleteAsync(Details.Id);
//        ShowConfirmDelete = false;
//        await ReloadAsync();
//    }

//    // ------ Util ------
//    protected void ClearDept() => DeptFilter = string.Empty;
//    protected void SelectDept(string key) => DeptFilter = key;
//    protected void OnTab0() => ActiveTab = 0;
//    protected void OnTab1() => ActiveTab = 1;
//    protected void OnTab2() => ActiveTab = 2;
//    protected void OnTab3() => ActiveTab = 3;
//    protected void OnTab4() => ActiveTab = 4;
//    protected void OnTab5() => ActiveTab = 5;
//    protected void OnTab6() => ActiveTab = 6;
//}

    
    
    
//    //protected async Task HandleSave()
//    //{
//    //    if (Details is null) return;

//    //    // Persistência
//    //    if (Details.Id == Guid.Empty)
//    //        Details.Id = await Service.CreateAsync(Details);
//    //    else
//    //        await Service.UpdateAsync(Details);

//    //    // Fecha modal e recarrega lista
//    //    ShowForm = false;
//    //    await ReloadAsync();
//    //}