using MyRhSystem.Contracts.Employees;

namespace MyRhSystem.Infrastructure.Employees
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDetailsDto>> GetAllAsync();
        Task<EmployeeDetailsDto?> GetDetailsAsync(Guid id);
        Task<Guid> CreateAsync(EmployeeDetailsDto dto);
        Task UpdateAsync(EmployeeDetailsDto dto);
        Task DeleteAsync(Guid id);
    }
}