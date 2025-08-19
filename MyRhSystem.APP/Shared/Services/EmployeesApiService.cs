using System.Net.Http.Json;
using MyRhSystem.Contracts.Employees;

namespace MyRhSystem.APP.Shared.Services;

public class EmployeesApiService
{
    private readonly HttpClient _http;
    public EmployeesApiService(HttpClient http) => _http = http;

    public async Task<EmployeeDetailsDto> CreateAsync(EmployeeDetailsDto dto, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/employees", dto, ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Falha ao criar funcionário: {(int)resp.StatusCode} {resp.ReasonPhrase}");

        var created = await resp.Content.ReadFromJsonAsync<EmployeeDetailsDto>(cancellationToken: ct)
                      ?? throw new InvalidOperationException("Resposta sem o funcionário criado.");

        return created;
    }

    public async Task UpdateAsync(Guid id, EmployeeDetailsDto dto, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync($"api/employees/{id}", dto, ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Falha ao atualizar funcionário: {(int)resp.StatusCode} {resp.ReasonPhrase}");
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var resp = await _http.DeleteAsync($"api/employees/{id}", ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Falha ao excluir funcionário: {(int)resp.StatusCode} {resp.ReasonPhrase}");
    }

    public Task<EmployeeDetailsDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<EmployeeDetailsDto>($"api/employees/{id}", ct);

    // Exemplo de paginação/listagem, ajuste o contrato conforme sua API
    public Task<List<EmployeeDetailsDto>?> GetAllAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<List<EmployeeDetailsDto>>("api/employees", ct);
}
