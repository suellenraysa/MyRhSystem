using Microsoft.AspNetCore.Components;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;

namespace MyRhSystem.APP.Shared.ViewModels;

public class OrganizationalStructureViewModel : ComponentBase
{
    // Dados
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<JobRoleDto> JobRoles { get; set; } = new();
    public List<JobLevelDto> Levels { get; set; } = new();

    // Seleções
    public int? SelectedDepartmentId { get; set; }
    public int? SelectedRoleId { get; set; }
    public int? SelectedRoleLevelId { get; set; }

    // Modais - estado
    public bool ShowDepartmentModal { get; set; }
    public bool ShowRoleModal { get; set; }
    public bool ShowLevelModal { get; set; }

    // Títulos dos modais
    public string DepartmentModalTitle { get; set; } = "Novo Departamento";
    public string RoleModalTitle { get; set; } = "Novo Cargo";
    public string LevelModalTitle { get; set; } = "Novo Nível";

    // Objetos em edição
    public DepartmentDto DepartmentEditing { get; set; } = new();
    public JobRoleDto RoleEditing { get; set; } = new();
    public JobLevelDto LevelEditing { get; set; } = new();

    protected override Task OnInitializedAsync()
    {
        // TODO: substituir por chamadas a serviços (API/EF)
        Departments = new()
        {
            new DepartmentDto { Id = 1, Nome = "RH" },
            new DepartmentDto { Id = 2, Nome = "Financeiro" },
            new DepartmentDto { Id = 3, Nome = "Tecnologia" },
        };

        Levels = new()
        {
            new JobLevelDto { Id = 1, Nome = "Estagiário", Ordem = 1 },
            new JobLevelDto { Id = 2, Nome = "Júnior",     Ordem = 2 },
            new JobLevelDto { Id = 3, Nome = "Pleno",      Ordem = 3 },
            new JobLevelDto { Id = 4, Nome = "Sênior",     Ordem = 4 },
            new JobLevelDto { Id = 5, Nome = "Trainer",    Ordem = 5 },
        };

        JobRoles = new()
        {
            new JobRoleDto { Id = 10, DepartmentId = 1, Nome = "Analista de RH", LevelId = 3, SalarioBase = 4500, SalarioMaximo = 6500 },
            new JobRoleDto { Id = 11, DepartmentId = 2, Nome = "Analista Financeiro", LevelId = 2, SalarioBase = 3800 },
            new JobRoleDto { Id = 12, DepartmentId = 3, Nome = "Desenvolvedor", LevelId = 4, SalarioBase = 12000, SalarioMaximo = 16000 },
        };

        return Task.CompletedTask;
    }

    // ===== Navegação / Seleção =====
    public void SelectDepartment(int id)
    {
        SelectedDepartmentId = id;
        // opcional: limpar seleção de cargo ao trocar de depto
        SelectedRoleId = null;
        SelectedRoleLevelId = null;
    }

    public void SelectRole(int id)
    {
        SelectedRoleId = id;
        var role = JobRoles.FirstOrDefault(r => r.Id == id);
        SelectedRoleLevelId = role?.LevelId;
    }

    public Task ReloadAsync()
    {
        // TODO: recarregar de serviços
        StateHasChanged();
        return Task.CompletedTask;
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

    public Task SaveDepartmentAsync()
    {
        if (string.IsNullOrWhiteSpace(DepartmentEditing.Nome))
            return Task.CompletedTask;

        if (DepartmentEditing.Id == 0)
        {
            DepartmentEditing.Id = (Departments.LastOrDefault()?.Id ?? 0) + 1;
            Departments.Add(new DepartmentDto { Id = DepartmentEditing.Id, Nome = DepartmentEditing.Nome });
        }
        else
        {
            var d = Departments.First(x => x.Id == DepartmentEditing.Id);
            d.Nome = DepartmentEditing.Nome;
        }

        ShowDepartmentModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void ConfirmDeleteDepartment(int id)
    {
        // Remoção simples + cascata em memória (remover cargos do depto)
        Departments.RemoveAll(d => d.Id == id);
        JobRoles.RemoveAll(r => r.DepartmentId == id);
        if (SelectedDepartmentId == id)
        {
            SelectedDepartmentId = null;
            SelectedRoleId = null;
            SelectedRoleLevelId = null;
        }
    }

    // ===== Cargo: Modal CRUD =====
    public void OpenRoleModalNew()
    {
        RoleModalTitle = "Novo Cargo";
        RoleEditing = new JobRoleDto
        {
            DepartmentId = SelectedDepartmentId ?? 0
        };
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

    public Task SaveRoleAsync()
    {
        if (string.IsNullOrWhiteSpace(RoleEditing.Nome) || RoleEditing.DepartmentId == 0 || RoleEditing.LevelId == 0)
            return Task.CompletedTask;

        if (RoleEditing.Id == 0)
        {
            RoleEditing.Id = (JobRoles.LastOrDefault()?.Id ?? 0) + 1;
            JobRoles.Add(RoleEditing);
        }
        else
        {
            var r = JobRoles.First(x => x.Id == RoleEditing.Id);
            r.Nome = RoleEditing.Nome;
            r.DepartmentId = RoleEditing.DepartmentId;
            r.LevelId = RoleEditing.LevelId;
            r.SalarioBase = RoleEditing.SalarioBase;
            r.SalarioMaximo = RoleEditing.SalarioMaximo;
            r.Requisitos = RoleEditing.Requisitos;
            r.Responsabilidades = RoleEditing.Responsabilidades;
        }

        ShowRoleModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void ConfirmDeleteRole(int id)
    {
        JobRoles.RemoveAll(r => r.Id == id);
        if (SelectedRoleId == id)
        {
            SelectedRoleId = null;
            SelectedRoleLevelId = null;
        }
    }

    public void OnChangeRoleLevel(ChangeEventArgs e)
    {
        if (!SelectedRoleId.HasValue) return;

        if (int.TryParse(e?.Value?.ToString(), out var newLevelId))
        {
            SelectedRoleLevelId = newLevelId;
            var role = JobRoles.First(r => r.Id == SelectedRoleId.Value);
            role.LevelId = newLevelId;
        }
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
        LevelEditing = new JobLevelDto { Id = l.Id, Nome = l.Nome, Ordem = l.Ordem };
        ShowLevelModal = true;
    }

    public void CloseLevelModal() => ShowLevelModal = false;

    public Task SaveLevelAsync()
    {
        if (string.IsNullOrWhiteSpace(LevelEditing.Nome))
            return Task.CompletedTask;

        if (LevelEditing.Id == 0)
        {
            LevelEditing.Id = (Levels.LastOrDefault()?.Id ?? 0) + 1;
            Levels.Add(new JobLevelDto { Id = LevelEditing.Id, Nome = LevelEditing.Nome, Ordem = LevelEditing.Ordem });
        }
        else
        {
            var l = Levels.First(x => x.Id == LevelEditing.Id);
            l.Nome = LevelEditing.Nome;
            l.Ordem = LevelEditing.Ordem;
        }

        ShowLevelModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void ConfirmDeleteLevel(int id)
    {
        // Evitar apagar nível em uso (simples verificação em memória)
        if (JobRoles.Any(r => r.LevelId == id))
        {
            // aqui você pode exibir um alerta/toast na UI
            return;
        }
        Levels.RemoveAll(x => x.Id == id);
    }
}
