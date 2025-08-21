using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;

namespace MyRhSystem.APP.Shared.ViewModels;

public class OrganizationalStructureViewModel : ComponentBase
{
    [Inject] protected IOrganizationalStructureApiService Api { get; set; } = default!;

    // Contexto — injete/atribua a empresa selecionada (ex.: do layout/seleção)
    [Parameter] public int CompanyId { get; set; }

    // Dados
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<JobRoleDto> JobRoles { get; set; } = new();
    public List<JobLevelDto> Levels { get; set; } = new();

    // Seleções
    public int? SelectedDepartmentId { get; set; }
    public int? SelectedRoleId { get; set; }
    public int? SelectedRoleLevelId { get; set; }

    // Modais
    public bool ShowDepartmentModal { get; set; }
    public bool ShowRoleModal { get; set; }
    public bool ShowLevelModal { get; set; }

    // Títulos
    public string DepartmentModalTitle { get; set; } = "Novo Departamento";
    public string RoleModalTitle { get; set; } = "Novo Cargo";
    public string LevelModalTitle { get; set; } = "Novo Nível";

    // Edição
    public DepartmentDto DepartmentEditing { get; set; } = new();
    public JobRoleDto RoleEditing { get; set; } = new();
    public JobLevelDto LevelEditing { get; set; } = new();

    protected override async Task OnParametersSetAsync()
    {
        // carrega toda a estrutura quando o CompanyId chegar ou mudar
        await ReloadAsync();
    }

    // ===== Navegação / Seleção =====
    public async Task SelectDepartmentAsync(int id)
    {
        SelectedDepartmentId = id;
        SelectedRoleId = null;
        SelectedRoleLevelId = null;

        JobRoles = await Api.GetRolesAsync(CompanyId, id);
        StateHasChanged();
    }

    public void SelectRole(int id)
    {
        SelectedRoleId = id;
        var role = JobRoles.FirstOrDefault(r => r.Id == id);
        SelectedRoleLevelId = role?.LevelId;
    }

    public async Task ReloadAsync()
    {
        if (CompanyId <= 0) return;

        Departments = await Api.GetDepartmentsAsync(CompanyId);
        Levels = await Api.GetLevelsAsync(CompanyId);

        // Se já houver depto selecionado, recarrega seus cargos;
        // senão limpa a grade de cargos.
        if (SelectedDepartmentId is int dId && Departments.Any(d => d.Id == dId))
            JobRoles = await Api.GetRolesAsync(CompanyId, dId);
        else
            JobRoles = new();

        StateHasChanged();
    }

    // ===== Departamento: Modal CRUD =====
    public void OpenDepartmentModalNew()
    {
        DepartmentModalTitle = "Novo Departamento";
        DepartmentEditing = new DepartmentDto();
        ShowDepartmentModal = true;
    }

    public void OpenDepartmentModalEdit(DepartmentDto d)
    {
        DepartmentModalTitle = "Editar Departamento";
        DepartmentEditing = new DepartmentDto { Id = d.Id, Nome = d.Nome };
        ShowDepartmentModal = true;
    }

    public void CloseDepartmentModal() => ShowDepartmentModal = false;

    public async Task SaveDepartmentAsync()
    {
        if (string.IsNullOrWhiteSpace(DepartmentEditing.Nome)) return;

        if (DepartmentEditing.Id == 0)
        {
            // cria e usa o que a API devolve (com Id preenchido)
            var created = await Api.CreateDepartmentAsync(CompanyId, DepartmentEditing);
            Departments.Add(created);
        }
        else
        {
            var updated = await Api.UpdateDepartmentAsync(
                CompanyId,
                DepartmentEditing.Id,   // aqui é leitura, não atribuição
                DepartmentEditing);

            var idx = Departments.FindIndex(x => x.Id == updated.Id);
            if (idx >= 0) Departments[idx] = updated;
        }

        ShowDepartmentModal = false;
        StateHasChanged();
    }

    public async Task ConfirmDeleteDepartmentAsync(int id)
    {
        await Api.DeleteDepartmentAsync(CompanyId, id);
        Departments.RemoveAll(d => d.Id == id);

        if (SelectedDepartmentId == id)
        {
            SelectedDepartmentId = null;
            SelectedRoleId = null;
            SelectedRoleLevelId = null;
            JobRoles.Clear();
        }
        StateHasChanged();
    }

    // ===== Cargo: Modal CRUD =====
    public void OpenRoleModalNew()
    {
        RoleModalTitle = "Novo Cargo";
        RoleEditing = new JobRoleDto { DepartmentId = SelectedDepartmentId ?? 0 };
        ShowRoleModal = true;
    }

    public void OpenRoleModalEdit(JobRoleDto r)
    {
        RoleModalTitle = "Editar Cargo";
        RoleEditing = new JobRoleDto
        {
            Id = r.Id,
            Nome = r.Nome,
            DepartmentId = r.DepartmentId,
            LevelId = r.LevelId,
            SalarioBase = r.SalarioBase,
            SalarioMaximo = r.SalarioMaximo,
            Requisitos = r.Requisitos,
            Responsabilidades = r.Responsabilidades
        };
        ShowRoleModal = true;
    }

    public void CloseRoleModal() => ShowRoleModal = false;

    public async Task SaveRoleAsync()
    {
        if (string.IsNullOrWhiteSpace(RoleEditing.Nome) || RoleEditing.DepartmentId == 0 || RoleEditing.LevelId == 0)
            return;

        if (RoleEditing.Id == 0)
        {
            var created = await Api.CreateRoleAsync(CompanyId, RoleEditing);
            JobRoles.Add(created);
        }
        else
        {
            var updated = await Api.UpdateRoleAsync(CompanyId, RoleEditing.Id, RoleEditing);
            var idx = JobRoles.FindIndex(x => x.Id == updated.Id);
            if (idx >= 0) JobRoles[idx] = updated;
        }

        ShowRoleModal = false;
        StateHasChanged();
    }

    public async Task ConfirmDeleteRoleAsync(int id)
    {
        await Api.DeleteRoleAsync(CompanyId, id);
        JobRoles.RemoveAll(r => r.Id == id);

        if (SelectedRoleId == id)
        {
            SelectedRoleId = null;
            SelectedRoleLevelId = null;
        }
        StateHasChanged();
    }

    public async Task OnChangeRoleLevelAsync(ChangeEventArgs e)
    {
        if (!SelectedRoleId.HasValue) return;
        if (!int.TryParse(e?.Value?.ToString(), out var newLevelId)) return;

        SelectedRoleLevelId = newLevelId;

        // atualiza no servidor (reaproveita o PUT de role)
        var role = JobRoles.First(r => r.Id == SelectedRoleId.Value);
        var dto = new JobRoleDto
        {
            Id = role.Id,
            Nome = role.Nome,
            DepartmentId = role.DepartmentId,
            LevelId = newLevelId,
            SalarioBase = role.SalarioBase,
            SalarioMaximo = role.SalarioMaximo,
            Requisitos = role.Requisitos,
            Responsabilidades = role.Responsabilidades
        };

        var updated = await Api.UpdateRoleAsync(CompanyId, role.Id, dto);
        var idx = JobRoles.FindIndex(x => x.Id == updated.Id);
        if (idx >= 0) JobRoles[idx] = updated;

        StateHasChanged();
    }

    // ===== Nível: Modal CRUD =====
    public void OpenLevelModalNew()
    {
        LevelModalTitle = "Novo Nível";
        LevelEditing = new JobLevelDto();
        ShowLevelModal = true;
    }

    public void OpenLevelModalEdit(JobLevelDto l)
    {
        LevelModalTitle = "Editar Nível";
        LevelEditing = new JobLevelDto { Id = l.Id, Nome = l.Nome };
        ShowLevelModal = true;
    }

    public void CloseLevelModal() => ShowLevelModal = false;

    public async Task SaveLevelAsync()
    {
        if (string.IsNullOrWhiteSpace(LevelEditing.Nome)) return;

        if (LevelEditing.Id == 0)
        {
            var created = await Api.CreateLevelAsync(CompanyId, LevelEditing);
            Levels.Add(created);
        }
        else
        {
            var updated = await Api.UpdateLevelAsync(CompanyId, LevelEditing.Id, LevelEditing);
            var idx = Levels.FindIndex(x => x.Id == updated.Id);
            if (idx >= 0) Levels[idx] = updated;
        }

        ShowLevelModal = false;
        StateHasChanged();
    }

    public async Task ConfirmDeleteLevelAsync(int id)
    {
        // servidor já valida "em uso"
        await Api.DeleteLevelAsync(CompanyId, id);
        Levels.RemoveAll(x => x.Id == id);
        StateHasChanged();
    }
}
