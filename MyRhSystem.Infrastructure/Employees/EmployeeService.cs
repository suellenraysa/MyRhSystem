namespace MyRhSystem.Infrastructure.Employees;

using System.Collections.Concurrent;
using MyRhSystem.Application.Employees;

public class EmployeeService : IEmployeeService
{
    private static readonly ConcurrentDictionary<Guid, EmployeeDto> _db = new();

    static EmployeeService()
    {
        var seed = new[]
        {
            new EmployeeDto { Id = Guid.NewGuid(), Nome = "João Silva", Cargo = "Desenvolvedor Sênior", Departamento = "Tecnologia", Email = "joao.silva@techcorp.com", Telefone = "(11) 99999-1111", Cidade = "São Paulo", UF = "SP", Contratacao = new(2023,1,14), Salario = 8500m, Ativo = true },
            new EmployeeDto { Id = Guid.NewGuid(), Nome = "Maria Santos", Cargo = "Gerente de Vendas", Departamento = "Vendas", Email = "maria.santos@techcorp.com", Telefone = "(11) 99999-2222", Cidade = "São Paulo", UF = "SP", Contratacao = new(2022,8,19), Salario = 12000m, Ativo = true },
            new EmployeeDto { Id = Guid.NewGuid(), Nome = "Pedro Oliveira", Cargo = "Analista de RH", Departamento = "Recursos Humanos", Email = "pedro.oliveira@techcorp.com", Telefone = "(11) 99999-3333", Cidade = "São Paulo", UF = "SP", Contratacao = new(2023,3,9), Salario = 6500m, Ativo = true },
            new EmployeeDto { Id = Guid.NewGuid(), Nome = "Ana Costa", Cargo = "Designer UX/UI", Departamento = "Design", Email = "ana.costa@techcorp.com", Telefone = "(11) 99999-4444", Cidade = "São Paulo", UF = "SP", Contratacao = new(2023,5,31), Salario = 7500m, Ativo = false },
        };
        foreach (var e in seed) _db[e.Id] = e;
    }

    public Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<EmployeeDto>>(_db.Values.OrderBy(x => x.Nome).ToList());

    public Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_db.TryGetValue(id, out var v) ? v : null);

    public Task<EmployeeDto> CreateAsync(EmployeeDto dto, CancellationToken ct = default)
    {
        dto.Id = Guid.NewGuid();
        _db[dto.Id] = dto;
        return Task.FromResult(dto);
    }

    public Task<EmployeeDto> UpdateAsync(EmployeeDto dto, CancellationToken ct = default)
    {
        if (dto.Id == Guid.Empty) throw new InvalidOperationException("Id inválido.");
        _db[dto.Id] = dto;
        return Task.FromResult(dto);
    }

    public Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        _db.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
