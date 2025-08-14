using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MyRhSystem.APP.Shared.Services;

public class UsersApiService
{
    private readonly HttpClient _http;
    public UsersApiService(HttpClient http) => _http = http;


    public async Task<PagedResponse<UserDto>> GetAsync(PagedRequest req, CancellationToken ct = default)
    {
        var url = $"api/users?Page={req.Page}&PageSize={req.PageSize}&Search={Uri.EscapeDataString(req.Search ?? "")}";
        return await _http.GetFromJsonAsync<PagedResponse<UserDto>>(url, ct)
               ?? new(new List<UserDto>(), req.Page, req.PageSize, 0);
    }

    public Task<UserDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => _http.GetFromJsonAsync<UserDto>($"api/users/{id}", ct);

    public async Task<UserDto> CreateAsync(CreateUserRequest req, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/users", req, ct);
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<UserDto>(cancellationToken: ct);
    }

    public Task<HttpResponseMessage> UpdateAsync(int id, UpdateUserRequest req, CancellationToken ct = default)
        => _http.PutAsJsonAsync($"api/users/{id}", req, ct);

    public Task<HttpResponseMessage> DeleteAsync(int id, CancellationToken ct = default)
        => _http.DeleteAsync($"api/users/{id}", ct);

}
