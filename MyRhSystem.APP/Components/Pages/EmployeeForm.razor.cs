using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MyRhSystem.Application.Employees;

namespace MyRhSystem.APP.Components.Pages;

public partial class EmployeeForm
{
    [Parameter] public string Title { get; set; } = "Novo Funcionário";
    [Parameter, EditorRequired] public EmployeeDetailsDto Details { get; set; } = default!;
    [Parameter] public bool IsViewOnly { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnSave { get; set; }

    // Tabs
    protected int Tab { get; private set; } = 0;
    protected void SetTab(int i) => Tab = i;

    protected Task CancelAsync() => OnCancel.InvokeAsync();

    // ---------------- Benefícios ----------------
    protected bool HasValeRefeicao
    {
        get => Details.Beneficios?.Contains("Vale Refeição", StringComparer.OrdinalIgnoreCase) == true;
        set => ToggleBeneficio("Vale Refeição", value);
    }
    protected bool HasValeTransporte
    {
        get => Details.Beneficios?.Contains("Vale Transporte", StringComparer.OrdinalIgnoreCase) == true;
        set => ToggleBeneficio("Vale Transporte", value);
    }

    private void ToggleBeneficio(string nome, bool add)
    {
        // Garante instância mutável
        var list = (Details.Beneficios ?? Array.Empty<string>()).ToList();
        var idx = list.FindIndex(s => string.Equals(s, nome, StringComparison.OrdinalIgnoreCase));
        if (add && idx < 0) list.Add(nome);
        if (!add && idx >= 0) list.RemoveAt(idx);
        Details.Beneficios = list;
    }

    // ---------------- Dependentes ----------------
    protected void AddDependente()
    {
        Details.Dependentes ??= new();
        Details.Dependentes.Add(new DependenteDto());
    }

    protected void RemoveDependente(Guid id)
    {
        var d = Details.Dependentes?.FirstOrDefault(x => x.Id == id);
        if (d is not null) Details.Dependentes!.Remove(d);
    }

    // ---------------- Contatos ----------------
    protected void AddContato()
    {
        Details.Contatos ??= new();
        Details.Contatos.Add(new ContatoDto());
    }

    protected void RemoveContato(Guid id)
    {
        var c = Details.Contatos?.FirstOrDefault(x => x.Id == id);
        if (c is not null) Details.Contatos!.Remove(c);
    }

    // ---------------- Horário (campos para <InputNumber>) ----------------
    private int? EntradaHora { get; set; }
    private int? EntradaMinuto { get; set; }
    private int? SaidaHora { get; set; }
    private int? SaidaMinuto { get; set; }

    // Atributos para @attributes (NÃO coloque "class"/"onwheel" aqui)
    protected Dictionary<string, object> HourAttrs { get; } = new()
    {
        ["min"] = "0",
        ["max"] = "23",
        ["step"] = "1",
        ["inputmode"] = "numeric",
        ["pattern"] = "[0-9]*"
    };

    protected Dictionary<string, object> MinuteAttrs { get; } = new()
    {
        ["min"] = "0",
        ["max"] = "59",
        ["step"] = "1",
        ["inputmode"] = "numeric",
        ["pattern"] = "[0-9]*"
    };

    protected override void OnParametersSet()
    {
        // Sobe valores do DTO para as caixas
        EntradaHora = Details.HorarioEntrada?.Hour;
        EntradaMinuto = Details.HorarioEntrada?.Minute;
        SaidaHora = Details.HorarioSaida?.Hour;
        SaidaMinuto = Details.HorarioSaida?.Minute;

        ClampTimes(); // garante faixas válidas na UI
    }

    private void ClampTimes()
    {
        EntradaHora = Clamp(EntradaHora, 0, 23);
        SaidaHora = Clamp(SaidaHora, 0, 23);
        EntradaMinuto = Clamp(EntradaMinuto, 0, 59);
        SaidaMinuto = Clamp(SaidaMinuto, 0, 59);

        static int? Clamp(int? v, int min, int max)
            => v is null ? null : Math.Min(max, Math.Max(min, v.Value));
    }

    private static TimeOnly? MakeTime(int? h, int? m)
        => (h is int hh && m is int mm) ? new TimeOnly(hh, mm) : (TimeOnly?)null;

    private void SyncTimeBackToDto()
    {
        Details.HorarioEntrada = MakeTime(EntradaHora, EntradaMinuto);
        Details.HorarioSaida = MakeTime(SaidaHora, SaidaMinuto);
    }

    // Handler do EditForm: use OnValidSubmit="OnSubmit" no .razor
    protected async Task OnSubmit(EditContext _)
    {
        ClampTimes();
        SyncTimeBackToDto();
        await OnSave.InvokeAsync();
    }
}
