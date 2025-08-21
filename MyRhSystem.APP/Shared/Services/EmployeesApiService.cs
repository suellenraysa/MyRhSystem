using System.Net.Http.Json;
using MyRhSystem.Contracts.Employees;

namespace MyRhSystem.APP.Shared.Services;

public class EmployeesApiService
{
    private readonly HttpClient _http;
    public EmployeesApiService(HttpClient http) => _http = http;

    public async Task<EmployeeDetailsDto> CreateAsync(CreateEmployeesRequest req, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/employees", req, ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Falha ao criar funcionário: {(int)resp.StatusCode} {resp.ReasonPhrase}");

        var created = await resp.Content.ReadFromJsonAsync<EmployeeDetailsDto>(cancellationToken: ct)
                     ?? throw new InvalidOperationException("Resposta sem o funcionário criado.");
        return created;
    }

    public async Task<HttpResponseMessage> UpdateAsync(int id, UpdateEmployeesRequest req, CancellationToken ct = default)
        => await _http.PutAsJsonAsync($"api/employees/{id}", req, ct);

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var resp = await _http.DeleteAsync($"api/employees/{id}", ct);
        if (!resp.IsSuccessStatusCode)
            throw new HttpRequestException($"Falha ao excluir funcionário: {(int)resp.StatusCode} {resp.ReasonPhrase}");
    }

    public Task<EmployeeDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<EmployeeDetailsDto>($"api/employees/{id}", ct);

    // Exemplo de paginação/listagem, ajuste o contrato conforme sua API
    public Task<List<EmployeeDetailsDto>?> GetAllAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<List<EmployeeDetailsDto>>("api/employees", ct);
}
