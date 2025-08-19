using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyRhSystem.Contracts.Employees;

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
                Contratacao = new DateTime(2023,1,14),
                Admissao = new DateTime(2023,1,14),
                DataExperiencia = new DateTime(2023,1,14),
                DataExperiencia1 = new DateTime(2023,7,14),
                Salario = 8500m, Ativo = true,
                Endereco="Rua A", Numero="100", Bairro="Centro", Cep="01000-000",
                GrauInstrucao="Superior completo",
                Beneficios = new [] { "VR", "VT", "Plano de Saúde" },
                Dependentes = new()
                {
                    new DependenteDto { Id = Guid.NewGuid(), Nome="Pedro Silva", GrauParentesco="Filho", Nascimento=new DateTime(2015,5,1) }
                },
                Contatos = new()
                {
                    new ContatoDto { Id = Guid.NewGuid(), Nome="Maria Silva", GrauParentesco="Esposa", Telefone="(11) 98888-7777" }
                }
            },
            new EmployeeDetailsDto
            {
                Id = Guid.NewGuid(), Nome = "Maria", Sobrenome="Santos",
                Email="maria.santos@techcorp.com", Telefone="(11) 99999-2222",
                Cidade="São Paulo", UF="SP", Departamento="Vendas",
                Cargo="Gerente de Vendas", Funcao="Gestão",
                Contratacao = new DateTime(2022,8,19),
                Admissao = new DateTime(2022,8,19),
                DataExperiencia = new DateTime(2023,1,14),
                DataExperiencia1 = new DateTime(2023,7,14),
                Salario = 12000m, Ativo = true
            }
        });
    }

    // Listagem: agora devolve DETAILS (cópias) para grid
    public Task<IEnumerable<EmployeeDetailsDto>> GetAllAsync()
        => Task.FromResult(_store.Select(e => e.Clone()));

    // Detalhes para o formulário (também como cópia)
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
}

internal static class EmployeeCloneExtensions
{
    public static EmployeeDetailsDto Clone(this EmployeeDetailsDto x)
        => new()
        {
            Id = x.Id,

            // Pessoais
            Nome = x.Nome,
            Sobrenome = x.Sobrenome,
            Email = x.Email,
            Telefone = x.Telefone,

            // Endereço
            Endereco = x.Endereco,
            Numero = x.Numero,
            Bairro = x.Bairro,
            Cidade = x.Cidade,
            UF = x.UF,
            Cep = x.Cep,

            // Profissional
            Departamento = x.Departamento,
            Cargo = x.Cargo,
            Funcao = x.Funcao,
            HorarioEntrada = x.HorarioEntrada,
            HorarioSaida = x.HorarioSaida,
            Contratacao = x.Contratacao,
            Admissao = x.Admissao,
            DataExperiencia = x.DataExperiencia,
            DataExperiencia1 = x.DataExperiencia1,
            Salario = x.Salario,
            Ativo = x.Ativo,
            GrauInstrucao = x.GrauInstrucao,

            // Documentos
            Cpf = x.Cpf,
            Rg = x.Rg,
            OrgaoEmissor = x.OrgaoEmissor,
            RgEmissao = x.RgEmissao,
            CtpsNumero = x.CtpsNumero,
            CtpsSerie = x.CtpsSerie,
            TituloEleitor = x.TituloEleitor,
            Zona = x.Zona,
            Sessao = x.Sessao,

            // Benefícios e coleções (null-safe)
            Beneficios = (x.Beneficios ?? Array.Empty<string>()).ToArray(),

            Dependentes = (x.Dependentes ?? new List<DependenteDto>())
                .Select(d => new DependenteDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Nascimento = d.Nascimento,
                    GrauParentesco = d.GrauParentesco
                })
                .ToList(),

            Contatos = (x.Contatos ?? new List<ContatoDto>())
                .Select(c => new ContatoDto
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Telefone = c.Telefone, // correção: não usar x.Telefone
                    GrauParentesco = c.GrauParentesco
                })
                .ToList()
        };
}
