using System.Net.Http.Json;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;

namespace MyRhSystem.APP.Shared.Services;

public sealed class OrganizationalStructureApiService(HttpClient http) : IOrganizationalStructureApiService
{
    const string BasePath = "/api/org";

    // -------- Departments --------
    public async Task<List<DepartmentDto>> GetDepartmentsAsync(int companyId, CancellationToken ct = default)
        => await http.GetFromJsonAsync<List<DepartmentDto>>($"{BasePath}/{companyId}/departments", ct) ?? [];

    public async Task<DepartmentDto> CreateDepartmentAsync(int companyId, DepartmentDto dto, CancellationToken ct = default)
    {
        var resp = await http.PostAsJsonAsync($"{BasePath}/{companyId}/departments", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<DepartmentDto>(cancellationToken: ct))!;
    }

    public async Task<DepartmentDto> UpdateDepartmentAsync(int companyId, int id, DepartmentDto dto, CancellationToken ct = default)
    {
        var resp = await http.PutAsJsonAsync($"{BasePath}/{companyId}/departments/{id}", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<DepartmentDto>(cancellationToken: ct))!;
    }

    public async Task DeleteDepartmentAsync(int companyId, int id, CancellationToken ct = default)
    {
        var resp = await http.DeleteAsync($"{BasePath}/{companyId}/departments/{id}", ct);
        resp.EnsureSuccessStatusCode();
    }

    // -------- Levels --------
    public async Task<List<JobLevelDto>> GetLevelsAsync(int companyId, CancellationToken ct = default)
        => await http.GetFromJsonAsync<List<JobLevelDto>>($"{BasePath}/{companyId}/levels", ct) ?? [];

    public async Task<JobLevelDto> CreateLevelAsync(int companyId, JobLevelDto dto, CancellationToken ct = default)
    {
        var resp = await http.PostAsJsonAsync($"{BasePath}/{companyId}/levels", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<JobLevelDto>(cancellationToken: ct))!;
    }

    public async Task<JobLevelDto> UpdateLevelAsync(int companyId, int id, JobLevelDto dto, CancellationToken ct = default)
    {
        var resp = await http.PutAsJsonAsync($"{BasePath}/{companyId}/levels/{id}", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<JobLevelDto>(cancellationToken: ct))!;
    }

    public async Task DeleteLevelAsync(int companyId, int id, CancellationToken ct = default)
    {
        var resp = await http.DeleteAsync($"{BasePath}/{companyId}/levels/{id}", ct);
        resp.EnsureSuccessStatusCode();
    }

    // -------- Roles --------
    public async Task<List<JobRoleDto>> GetRolesAsync(int companyId, int? departmentId = null, CancellationToken ct = default)
    {
        var q = departmentId is null ? "" : $"?departmentId={departmentId}";
        return await http.GetFromJsonAsync<List<JobRoleDto>>($"{BasePath}/{companyId}/roles{q}", ct) ?? [];
    }

    public async Task<JobRoleDto> CreateRoleAsync(int companyId, JobRoleDto dto, CancellationToken ct = default)
    {
        var resp = await http.PostAsJsonAsync($"{BasePath}/{companyId}/roles", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<JobRoleDto>(cancellationToken: ct))!;
    }

    public async Task<JobRoleDto> UpdateRoleAsync(int companyId, int id, JobRoleDto dto, CancellationToken ct = default)
    {
        var resp = await http.PutAsJsonAsync($"{BasePath}/{companyId}/roles/{id}", dto, ct);
        resp.EnsureSuccessStatusCode();
        return (await resp.Content.ReadFromJsonAsync<JobRoleDto>(cancellationToken: ct))!;
    }

    public async Task DeleteRoleAsync(int companyId, int id, CancellationToken ct = default)
    {
        var resp = await http.DeleteAsync($"{BasePath}/{companyId}/roles/{id}", ct);
        resp.EnsureSuccessStatusCode();
    }
}
