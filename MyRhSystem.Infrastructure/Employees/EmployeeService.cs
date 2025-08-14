using MyRhSystem.Application.Employees;

namespace MyRhSystem.Infrastructure.Employees;

public class EmployeeService : IEmployeeService
{
    // "Banco" em memória
    private readonly List<EmployeeDetailsDto> _store = new();

    public EmployeeService()
    {
        // Seed de exemplo
        _store.AddRange(new[]
        {
            new EmployeeDetailsDto
            {
                Id = Guid.NewGuid(), Nome = "João", Sobrenome="Silva",
                Email="joao.silva@techcorp.com", Telefone="(11) 99999-1111",
                Cidade="São Paulo", UF="SP", Departamento="Tecnologia",
                Cargo="Desenvolvedor Sênior", Funcao="Dev",
                Contratacao = new DateTime(2023,1,14), Admissao = new DateTime(2023,1,14), DataExperiencia = new DateTime(2023,1,14), DataExperiencia1 = new DateTime(2023,7,14),
                Salario = 8500m, Ativo = true,
                Endereco="Rua A", Numero="100", Bairro="Centro", Cep="01000-000",
                GrauInstrucao="Superior completo",
                Beneficios = new [] { "VR", "VT", "Plano de Saúde" },
                Dependentes = new()
                {
                    new DependenteDto { Nome="Pedro Silva", GrauParentesco="Filho", Nascimento=new DateTime(2015,5,1)}
                },
                Contatos = new()
                {
                    new ContatoDto { Nome="Maria Silva", GrauParentesco="Esposa", Telefone="(11) 98888-7777"}
                }                
            },
            new EmployeeDetailsDto
            {
                Id = Guid.NewGuid(), Nome = "Maria", Sobrenome="Santos",
                Email="maria.santos@techcorp.com", Telefone="(11) 99999-2222",
                Cidade="São Paulo", UF="SP", Departamento="Vendas",
                Cargo="Gerente de Vendas", Funcao="Gestão",
                Contratacao = new DateTime(2022,8,19), Admissao = new DateTime(2022,8,19), DataExperiencia = new DateTime(2023,1,14), DataExperiencia1 = new DateTime(2023,7,14),
                Salario = 12000m, Ativo = true
            }
        });
    }

    public Task<IEnumerable<EmployeeDto>> GetAllAsync()
        => Task.FromResult(_store.Select(ToListDto));

    public Task<EmployeeDetailsDto?> GetDetailsAsync(Guid id)
        => Task.FromResult(_store.FirstOrDefault(x => x.Id == id)?.Clone());

    public Task<Guid> CreateAsync(EmployeeDetailsDto dto)
    {
        dto.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;
        _store.Add(dto.Clone()); // guarda cópia
        return Task.FromResult(dto.Id);
    }

    public Task UpdateAsync(EmployeeDetailsDto dto)
    {
        var idx = _store.FindIndex(x => x.Id == dto.Id);
        if (idx >= 0) _store[idx] = dto.Clone();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _store.RemoveAll(x => x.Id == id);
        return Task.CompletedTask;
    }

    private static EmployeeDto ToListDto(EmployeeDetailsDto x) => new()
    {
        Id = x.Id,
        Nome = x.Nome,
        Sobrenome = x.Sobrenome,
        Email = x.Email,
        Telefone = x.Telefone,
        Cidade = x.Cidade,
        UF = x.UF,
        Departamento = x.Departamento,
        Cargo = x.Cargo,
        Contratacao = x.Contratacao,
        Salario = x.Salario,
        Ativo = x.Ativo
    };
}

internal static class EmployeeCloneExtensions
{
    public static EmployeeDetailsDto Clone(this EmployeeDetailsDto x)
        => new()
        {
            Id = x.Id,
            Nome = x.Nome,
            Sobrenome = x.Sobrenome,
            Email = x.Email,
            Telefone = x.Telefone,
            Cidade = x.Cidade,
            UF = x.UF,
            Departamento = x.Departamento,
            Cargo = x.Cargo,
            Funcao = x.Funcao,
            HorarioEntrada = x.HorarioEntrada,
            HorarioSaida = x.HorarioSaida,
            Contratacao = x.Contratacao,
            Admissao = x.Admissao,
            Salario = x.Salario,
            Ativo = x.Ativo,
            Endereco = x.Endereco,
            Numero = x.Numero,
            Bairro = x.Bairro,
            Cep = x.Cep,
            GrauInstrucao = x.GrauInstrucao,
            Beneficios = x.Beneficios.ToArray(),
            Dependentes = x.Dependentes.Select(d => new DependenteDto
            {
                Nome = d.Nome,
                Nascimento = d.Nascimento,
                GrauParentesco = d.GrauParentesco
            }).ToList(),
            Contatos = x.Contatos.Select(d => new ContatoDto
            {
                Nome = d.Nome,
                Telefone = x.Telefone,
                GrauParentesco = d.GrauParentesco
            }).ToList(),            
            Cpf = x.Cpf,
            Rg = x.Rg,
            CtpsNumero = x.CtpsNumero,
            CtpsSerie = x.CtpsSerie,
            TituloEleitor = x.TituloEleitor
        };
}
