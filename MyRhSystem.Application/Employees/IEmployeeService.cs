namespace MyRhSystem.Application.Employees;

public interface IEmployeeService
{
    // Grid
    Task<IEnumerable<EmployeeDto>> GetAllAsync();

    // Detalhes/edição
    Task<EmployeeDetailsDto?> GetDetailsAsync(Guid id);

    // Persistência
    Task<Guid> CreateAsync(EmployeeDetailsDto dto);
    Task UpdateAsync(EmployeeDetailsDto dto);
    Task DeleteAsync(Guid id);
}
