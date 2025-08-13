using MyRhSystem.Contracts.Auth;
using System.Net.Http.Json;

namespace MyRhSystem.APP.Shared.Services;

public class AuthApiService
{

    private readonly HttpClient _http;
    public AuthApiService(HttpClient http) => _http = http;

    public Task<HasAnyUserResponse?> HasAnyUserAsync(CancellationToken ct = default)
        => _http.GetFromJsonAsync<HasAnyUserResponse>("api/auth/has-user, ct");

    public async Task<LoginResponse?> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login", req, ct);
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
    }
}
