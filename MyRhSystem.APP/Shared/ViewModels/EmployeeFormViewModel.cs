using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Constants;
using MyRhSystem.Application.Employees;
using MyRhSystem.Contracts.Employees; // <-- contém EmployeeDetailsDto, DependenteDto, ContatoEmergenciaDto

namespace MyRhSystem.APP.Shared.ViewModels
{
    public class EmployeeFormViewModel : ComponentBase
    {
        public string Title { get; set; } = "Cadastro de Funcionário";
        public int Tab { get; set; }
        public bool IsViewOnly { get; set; }

        public IReadOnlyList<string> EducationLevels => AppDefaults.EducationLevels;
        public IReadOnlyList<string> WorkRegime => AppDefaults.WorkRegime;
        public IReadOnlyList<string> States => AppDefaults.States;

        public bool HasValeRefeicao { get; set; }
        public bool HasValeTransporte { get; set; }

        public int? EntradaHora { get; set; } = 9;
        public int? EntradaMinuto { get; set; } = 0;
        public int? SaidaHora { get; set; } = 18;
        public int? SaidaMinuto { get; set; } = 0;

        public Dictionary<string, object> HourAttrs { get; } = new() { ["min"] = 0, ["max"] = 23, ["step"] = 1, ["inputmode"] = "numeric" };
        public Dictionary<string, object> MinuteAttrs { get; } = new() { ["min"] = 0, ["max"] = 59, ["step"] = 1, ["inputmode"] = "numeric" };

        // ⬇️ Agora com EmployeeDetailsDto
        public EmployeeDetailsDto? Details { get; set; }

        public void SetTab(int index) => Tab = index;
        public Task OnSubmit() => Task.CompletedTask;

        public Task CancelAsync()
        {
            Details = null; // fecha modal
            StateHasChanged();
            return Task.CompletedTask;
        }

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

        // Chamado por Employees.razor
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

            Tab = 0;
            StateHasChanged();
        }

        // Para edição
        public void OpenEdit(EmployeeDetailsDto dto)
        {
            Title = "Editar Funcionário";
            IsViewOnly = false;
            Details = dto;
            Tab = 0;
            StateHasChanged();
        }
    }
}
