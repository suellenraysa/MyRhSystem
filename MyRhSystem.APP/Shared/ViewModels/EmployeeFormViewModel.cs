using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Constants;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.Employees;
using MyRhSystem.Contracts.JobRole;
using MyRhSystem.APP.Shared.Services;

namespace MyRhSystem.APP.Shared.ViewModels
{
    public class EmployeeFormViewModel : ComponentBase
    {
        [Inject] private EmployeesApiService EmployeesApi { get; set; } = default!;
        // Header / estado do formulário
        public string Title { get; set; } = "Cadastro de Funcionário";
        public int Tab { get; set; }
        public bool IsViewOnly { get; set; }
        public bool IsRoleDisabled => IsViewOnly || SelectedDepartmentId is null;

        // Lookups (AppDefaults)
        public IReadOnlyList<string> EducationLevels => AppDefaults.EducationLevels;
        public IReadOnlyList<string> WorkRegime => AppDefaults.WorkRegime;
        public IReadOnlyList<string> States => AppDefaults.States;
        public IReadOnlyList<string> Genders => AppDefaults.Genders;

        // Listas carregadas (da API futuramente)
        public List<DepartmentDto> Departments { get; set; } = new();
        public List<JobRoleDto> JobRoles { get; set; } = new();
        public List<JobLevelDto> JobLevels { get; set; } = new();

        // Seleções
        [Required] public int? SelectedDepartmentId { get; set; }
        [Required] public int? SelectedJobRoleId { get; set; }

        // Bind do Status (para permitir placeholder “Selecione”)
        public bool? Status { get; set; }

        // Flags simples de benefícios
        public bool HasValeRefeicao { get; set; }
        public bool HasValeTransporte { get; set; }

        // Horários (entrada/saída) + attrs dos inputs numéricos
        public int? EntradaHora { get; set; } = 9;
        public int? EntradaMinuto { get; set; } = 0;
        public int? SaidaHora { get; set; } = 18;
        public int? SaidaMinuto { get; set; } = 0;

        public Dictionary<string, object> HourAttrs { get; } =
            new() { ["min"] = 0, ["max"] = 23, ["step"] = 1, ["inputmode"] = "numeric" };
        public Dictionary<string, object> MinuteAttrs { get; } =
            new() { ["min"] = 0, ["max"] = 59, ["step"] = 1, ["inputmode"] = "numeric" };

        // DTO principal
        public EmployeeDetailsDto? Details { get; set; }

        // Navegação de abas / submit
        public void SetTab(int index) => Tab = index;
        public Task OnSubmit() => Task.CompletedTask;

        public Task CancelAsync()
        {
            Details = null; // fecha modal
            StateHasChanged();
            return Task.CompletedTask;
        }

        // Dependentes
        public void AddDependente()
        {
            Details ??= new EmployeeDetailsDto();
            Details.Dependentes ??= new List<DependenteDto>();
            Details.Dependentes.Add(new DependenteDto
            {
                Id = Guid.NewGuid(),
                Nome = "",
                GrauParentesco = "",
                Nascimento = DateTime.Today
            });
            StateHasChanged();
        }

        public void RemoveDependente(Guid id)
        {
            var item = Details?.Dependentes?.FirstOrDefault(x => x.Id == id);
            if (item != null) Details!.Dependentes!.Remove(item);
            StateHasChanged();
        }

        // Contatos de emergência
        public void AddContato()
        {
            Details ??= new EmployeeDetailsDto();
            Details.Contatos ??= new List<ContatoDto>();
            Details.Contatos.Add(new ContatoDto
            {
                Id = Guid.NewGuid(),
                Nome = "",
                GrauParentesco = "",
                Telefone = ""
            });
            StateHasChanged();
        }

        public void RemoveContato(Guid id)
        {
            var item = Details?.Contatos?.FirstOrDefault(x => x.Id == id);
            if (item != null) Details!.Contatos!.Remove(item);
            StateHasChanged();
        }

        // Abrir: Novo
        public void OpenNew()
        {
            Title = "Novo Funcionário";
            IsViewOnly = false;

            Details = new EmployeeDetailsDto
            {
                Ativo = true,
                JornadaHoras = 40,
                Dependentes = new List<DependenteDto>(),
                Contatos = new List<ContatoDto>()
            };

            // sincroniza o combo de status
            Status = Details.Ativo;

            Tab = 0;
            StateHasChanged();
        }

        // Abrir: Editar
        public void OpenEdit(EmployeeDetailsDto dto)
        {
            Title = "Editar Funcionário";
            IsViewOnly = false;

            Details = dto;

            // sincroniza o combo de status
            Status = Details.Ativo;

            // pré-seleção das combos com base nos textos do DTO
            if (!string.IsNullOrWhiteSpace(Details.Departamento))
                SelectedDepartmentId = Departments.FirstOrDefault(d => d.Nome == Details.Departamento)?.Id;

            if (!string.IsNullOrWhiteSpace(Details.Cargo) && SelectedDepartmentId.HasValue)
                SelectedJobRoleId = JobRoles
                    .FirstOrDefault(r => r.Nome == Details.Cargo && r.DepartmentId == SelectedDepartmentId.Value)
                    ?.Id;

            Tab = 0;
            StateHasChanged();
        }

        // Ao trocar de departamento
        protected void OnDepartmentChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e?.Value?.ToString(), out var id))
                SelectedDepartmentId = id;
            else
                SelectedDepartmentId = null;

            SelectedJobRoleId = null;
            StateHasChanged();
        }

        // Carregamento inicial (mock estático por enquanto)
        protected override Task OnInitializedAsync()
        {
            Departments = new()
            {
                new DepartmentDto { Id = 1, Nome = "RH" },
                new DepartmentDto { Id = 2, Nome = "Financeiro" },
                new DepartmentDto { Id = 3, Nome = "Tecnologia" },
            };

            JobLevels = new()
            {
                new JobLevelDto { Id = 1, Nome = "Estagiário", Ordem = 1 },
                new JobLevelDto { Id = 2, Nome = "Júnior",     Ordem = 2 },
                new JobLevelDto { Id = 3, Nome = "Pleno",      Ordem = 3 },
                new JobLevelDto { Id = 4, Nome = "Sênior",     Ordem = 4 },
                new JobLevelDto { Id = 5, Nome = "Trainer",    Ordem = 5 },
            };

            JobRoles = new()
            {
                new JobRoleDto { Id = 10, DepartmentId = 1, LevelId = 3, Nome = "Analista de RH",       SalarioBase = 4500 },
                new JobRoleDto { Id = 11, DepartmentId = 2, LevelId = 2, Nome = "Analista Financeiro", SalarioBase = 3800 },
                new JobRoleDto { Id = 12, DepartmentId = 3, LevelId = 4, Nome = "Desenvolvedor",       SalarioBase = 12000 },
            };

            StateHasChanged();
            return Task.CompletedTask;
        }

        // 👉 helper pra criar TimeSpan dos campos hh:mm
        private static TimeSpan BuildTime(int? hh, int? mm)
            => new TimeSpan(hh.GetValueOrDefault(), mm.GetValueOrDefault(), 0);

        // 👉 chamada de “Salvar” (create/update)
        public async Task SaveAsync(CancellationToken ct = default)
        {
            if (Details is null) return;

            // 1) aplica o combo de Status no DTO
            Details.Ativo = Status ?? false;

            // 2) horários
            Details.HorarioEntrada = BuildTime(EntradaHora, EntradaMinuto);
            Details.HorarioSaida = BuildTime(SaidaHora, SaidaMinuto);

            // 3) nomes de Departamento/Cargo a partir dos ids selecionados
            if (SelectedDepartmentId is int depId)
            {
                var dep = Departments.FirstOrDefault(d => d.Id == depId);
                if (dep is not null) Details.Departamento = dep.Nome;
            }

            if (SelectedJobRoleId is int roleId)
            {
                var role = JobRoles.FirstOrDefault(r => r.Id == roleId);
                if (role is not null)
                {
                    Details.Cargo = role.Nome;
                    if (string.IsNullOrWhiteSpace(Details.Funcao))
                        Details.Funcao = role.Nome; // opcional: por enquanto iguala função ao cargo
                }
            }

            // 4) cria ou atualiza
            if (Details!.Id == Guid.Empty)
            {
                // create
                Details = await EmployeesApi.CreateAsync(Details, ct);
                Status = Details.Ativo;
            }
            else
            {
                // update
                await EmployeesApi.UpdateAsync(Details.Id, Details, ct);
            }

            // 5) fecha modal
            await CancelAsync();
        }

        // Helper para aplicar Status -> Details antes de salvar (se necessário)
        public void SyncStatusToDetails()
        {
            if (Details is null) return;
            Details.Ativo = Status ?? false;
        }
    }
}
