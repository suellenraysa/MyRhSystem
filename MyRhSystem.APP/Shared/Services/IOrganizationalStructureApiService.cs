using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;

namespace MyRhSystem.APP.Shared.Services;

public interface IOrganizationalStructureApiService
{
    // Departments
    Task<List<DepartmentDto>> GetDepartmentsAsync(int companyId, CancellationToken ct = default);
    Task<DepartmentDto> CreateDepartmentAsync(int companyId, DepartmentDto dto, CancellationToken ct = default);
    Task<DepartmentDto> UpdateDepartmentAsync(int companyId, int id, DepartmentDto dto, CancellationToken ct = default);
    Task DeleteDepartmentAsync(int companyId, int id, CancellationToken ct = default);

    // Levels
    Task<List<JobLevelDto>> GetLevelsAsync(int companyId, CancellationToken ct = default);
    Task<JobLevelDto> CreateLevelAsync(int companyId, JobLevelDto dto, CancellationToken ct = default);
    Task<JobLevelDto> UpdateLevelAsync(int companyId, int id, JobLevelDto dto, CancellationToken ct = default);
    Task DeleteLevelAsync(int companyId, int id, CancellationToken ct = default);

    // Roles
    Task<List<JobRoleDto>> GetRolesAsync(int companyId, int? departmentId = null, CancellationToken ct = default);
    Task<JobRoleDto> CreateRoleAsync(int companyId, JobRoleDto dto, CancellationToken ct = default);
    Task<JobRoleDto> UpdateRoleAsync(int companyId, int id, JobRoleDto dto, CancellationToken ct = default);
    Task DeleteRoleAsync(int companyId, int id, CancellationToken ct = default);
}
