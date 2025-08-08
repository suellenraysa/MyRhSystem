namespace MyRhSystem.Application.Employees;

public interface IEmployeeService
{
    Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken ct = default);
    Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<EmployeeDto> CreateAsync(EmployeeDto dto, CancellationToken ct = default);
    Task<EmployeeDto> UpdateAsync(EmployeeDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
