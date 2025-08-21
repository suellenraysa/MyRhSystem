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
        public List<BranchDtos> Departments { get; set; } = new();
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
                Id = 0,
                Nome = "",
                GrauParentesco = "",
                Nascimento = DateTime.Today
            });
            StateHasChanged();
        }

        public void RemoveDependente(int id)
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
                Id = 0,
                Nome = "",
                GrauParentesco = "",
                Telefone = ""
            });
            StateHasChanged();
        }

        public void RemoveContato(int id)
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
                new BranchDtos { Id = 1, Nome = "RH" },
                new BranchDtos { Id = 2, Nome = "Financeiro" },
                new BranchDtos { Id = 3, Nome = "Tecnologia" },
            };

            JobLevels = new()
            {
                new JobLevelDto { Id = 1, Nome = "Estagiário"},
                new JobLevelDto { Id = 2, Nome = "Júnior"},
                new JobLevelDto { Id = 3, Nome = "Pleno"},
                new JobLevelDto { Id = 4, Nome = "Sênior"},
                new JobLevelDto { Id = 5, Nome = "Trainer"},
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

            // mantém Status -> Details
            SyncStatusToDetails();

            // se usar selects, alinhe os textos mostrados
            if (SelectedDepartmentId is not null)
                Details.Departamento = Departments.FirstOrDefault(d => d.Id == SelectedDepartmentId)!.Nome;

            if (SelectedJobRoleId is not null)
            {
                var role = JobRoles.FirstOrDefault(r => r.Id == SelectedJobRoleId);
                if (role is not null)
                {
                    Details.Cargo = role.Nome;
                    Details.Funcao = role.Nome; // se desejar
                }
            }

            if (Details.Id == 0)
            {
                // === CREATE ===
                var req = new CreateEmployeesRequest
                {
                    // pessoais
                    Nome = Details.Nome,
                    Sobrenome = Details.Sobrenome,
                    Sexo = Details.Sexo,
                    DataNascimento = Details.DataNascimento,
                    Email = Details.Email,
                    Telefone = Details.Telefone,

                    // profissional
                    Cargo = Details.Cargo,
                    Departamento = Details.Departamento,
                    Funcao = Details.Funcao,
                    GrauInstrucao = Details.GrauInstrucao,
                    Ativo = Details.Ativo,

                    // filiação
                    NomeMae = Details.NomeMae,
                    NomePai = Details.NomePai,

                    // documentos
                    Cpf = Details.Cpf,
                    Rg = Details.Rg,
                    OrgaoEmissor = Details.OrgaoEmissor,
                    RgEmissao = Details.RgEmissao,
                    TituloEleitor = Details.TituloEleitor,
                    Zona = Details.Zona,
                    Sessao = Details.Sessao,
                    CtpsNumero = Details.CtpsNumero,
                    CtpsSerie = Details.CtpsSerie,

                    // contrato
                    TipoContrato = Details.TipoContrato,
                    JornadaHoras = Details.JornadaHoras,
                    Admissao = Details.Admissao,
                    DataExperiencia = Details.DataExperiencia,
                    DataExperiencia1 = Details.DataExperiencia1,
                    Contratacao = Details.Contratacao,
                    Salario = Details.Salario,

                    // endereço
                    Endereco = Details.Endereco,
                    Numero = Details.Numero,
                    Complemento = Details.Complemento,
                    Bairro = Details.Bairro,
                    Cidade = Details.Cidade,
                    UF = Details.UF,
                    Cep = Details.Cep,

                    // coleções/outros
                    Dependentes = Details.Dependentes,
                    Contatos = Details.Contatos,
                    Beneficios = Details.Beneficios,
                    HorarioEntrada = Details.HorarioEntrada,
                    HorarioSaida = Details.HorarioSaida
                };

                var created = await EmployeesApi.CreateAsync(req, ct);
                Details.Id = created.Id; // agora o DTO “conhece” seu Id
            }
            else
            {
                // === UPDATE ===
                var req = new UpdateEmployeesRequest
                {
                    Id = Details.Id,
                    // preencha os mesmos campos que desejar atualizar
                    Nome = Details.Nome,
                    Sobrenome = Details.Sobrenome,
                    Sexo = Details.Sexo,
                    DataNascimento = Details.DataNascimento,
                    Email = Details.Email,
                    Telefone = Details.Telefone,
                    Cargo = Details.Cargo,
                    Departamento = Details.Departamento,
                    Funcao = Details.Funcao,
                    GrauInstrucao = Details.GrauInstrucao,
                    Ativo = Details.Ativo,
                    NomeMae = Details.NomeMae,
                    NomePai = Details.NomePai,
                    Cpf = Details.Cpf,
                    Rg = Details.Rg,
                    OrgaoEmissor = Details.OrgaoEmissor,
                    RgEmissao = Details.RgEmissao,
                    TituloEleitor = Details.TituloEleitor,
                    Zona = Details.Zona,
                    Sessao = Details.Sessao,
                    CtpsNumero = Details.CtpsNumero,
                    CtpsSerie = Details.CtpsSerie,
                    TipoContrato = Details.TipoContrato,
                    JornadaHoras = Details.JornadaHoras,
                    Admissao = Details.Admissao,
                    DataExperiencia = Details.DataExperiencia,
                    DataExperiencia1 = Details.DataExperiencia1,
                    Contratacao = Details.Contratacao,
                    Salario = Details.Salario,
                    Endereco = Details.Endereco,
                    Numero = Details.Numero,
                    Complemento = Details.Complemento,
                    Bairro = Details.Bairro,
                    Cidade = Details.Cidade,
                    UF = Details.UF,
                    Cep = Details.Cep,
                    Dependentes = Details.Dependentes,
                    Contatos = Details.Contatos,
                    Beneficios = Details.Beneficios,
                    HorarioEntrada = Details.HorarioEntrada,
                    HorarioSaida = Details.HorarioSaida
                };

                await EmployeesApi.UpdateAsync(Details.Id, req, ct);
            }

            await CancelAsync(); // fecha modal
        }

        // Helper para aplicar Status -> Details antes de salvar (se necessário)
        public void SyncStatusToDetails()
        {
            if (Details is null) return;
            Details.Ativo = Status ?? false;
        }
    }
}
