using Bogus;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Infrastructure.Persistence;
using System.Diagnostics;
using DomainAtribuicao = MyRhSystem.Domain.Entities.Uniformes.AtribuicaoUniformes;
// Aliases para evitar ambiguidade com Bogus.DataSets.Company
using DomainCompany = MyRhSystem.Domain.Entities.Companies.Company;
using DomainFolha = MyRhSystem.Domain.Entities.Payroll.FolhaDePagamento;
using DomainFuncionario = MyRhSystem.Domain.Entities.Funcionarios.Funcionario;
using DomainItemUniforme = MyRhSystem.Domain.Entities.Uniformes.ItensUniformes;
using DomainUser = MyRhSystem.Domain.Entities.Users.User;

namespace MyRhSystem.Infrastructure.Seed
{
    public static class UserFaker
    {
        public static Faker<DomainUser> Get() => new Faker<DomainUser>("pt_BR")
            .RuleFor(u => u.Id, f => f.IndexFaker + 1)
            .RuleFor(u => u.Nome, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2));
    }

    public static class CompanyEntityFaker
    {
        public static Faker<DomainCompany> Get() => new Faker<DomainCompany>("pt_BR")
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Nome, f => f.Company.CompanyName())
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(3));
    }

    public static class FuncionarioFaker
    {
        public static Faker<DomainFuncionario> Get(int companyId) => new Faker<DomainFuncionario>("pt_BR")
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.CompanyId, _ => companyId)
            .RuleFor(e => e.Nome, f => f.Name.FirstName())
            .RuleFor(e => e.Sobrenome, f => f.Name.LastName())
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Nome, e.Sobrenome))
            .RuleFor(e => e.Telefone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.Posicao, f => f.Name.JobTitle())
            .RuleFor(e => e.DataDeAdmissao, f => f.Date.Past(2))
            .RuleFor(e => e.Status, f => f.PickRandom(new[] { "Active", "Inactive", "OnLeave" }));
    }

    public static class FolhaDePagamentoFaker
    {
        public static Faker<DomainFolha> Get(int funcionarioId) => new Faker<DomainFolha>("pt_BR")
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.FuncionarioId, _ => funcionarioId)
            .RuleFor(p => p.HoraEntrada, f => f.Date.Recent().Date.AddHours(f.Random.Int(6, 10)))
            .RuleFor(p => p.HoraSaida, (f, p) => p.HoraEntrada.AddHours(f.Random.Int(6, 10)))
            .RuleFor(p => p.SalarioBase, f => f.Finance.Amount(1500, 5000))
            .RuleFor(p => p.Bonus, f => f.Finance.Amount(0, 500))
            .RuleFor(p => p.Deducoes, f => f.Finance.Amount(0, 300))
            .RuleFor(p => p.SalarioLiquido, (f, p) => p.SalarioBase + p.Bonus - p.Deducoes)
            .RuleFor(p => p.DataPagamento, f => f.Date.Recent(60))
            .RuleFor(p => p.CreatedAt, (f, p) => p.DataPagamento);
    }

    public static class ItensUniformesFaker
    {
        public static Faker<DomainItemUniforme> Get() => new Faker<DomainItemUniforme>("pt_BR")
            .RuleFor(i => i.Id, f => f.IndexFaker + 1)
            .RuleFor(i => i.Code, f => f.Commerce.Ean13())
            .RuleFor(i => i.Descricao, f => f.Commerce.Product())
            .RuleFor(i => i.Tamanho, f => f.PickRandom(new[] { "P", "M", "G", "GG" }))
            .RuleFor(i => i.TotalEstoque, f => f.Random.Int(0, 100))
            .RuleFor(i => i.CreatedAt, f => f.Date.Past(1));
    }

    public static class AtribuicaoUniformesFaker
    {
        public static Faker<DomainAtribuicao> Get(int funcionarioId, int itemUniformeId) => new Faker<DomainAtribuicao>("pt_BR")
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.FuncionarioId, _ => funcionarioId)
            .RuleFor(a => a.ItensUniformesId, _ => itemUniformeId)
            .RuleFor(a => a.Quantidade, f => f.Random.Int(1, 5))
            .RuleFor(a => a.DataEntrega, f => f.Date.Past(1))
            .RuleFor(a => a.DataRetornada, f => f.Random.Bool(0.3f) ? f.Date.Recent(30) : (DateTime?)null)
            .RuleFor(a => a.Condicoes, f => f.Lorem.Sentence());
    }

    public class DataSeeder
    {
        private readonly ApplicationDbContext _db;
        public DataSeeder(ApplicationDbContext db) => _db = db;

        public async Task SeedAsync()
        {
            try
            {
                Debug.WriteLine(">>> Seed start");
                var beforeCount = await _db.Users.CountAsync();
                Debug.WriteLine($">>> Users before seed: {beforeCount}");

                if (beforeCount > 0)
                {
                    Debug.WriteLine(">>> Seed skipped: users already exist");
                    return;
                }

                // Gerar dados
                Debug.WriteLine(">>> Generating fake data");
                var users = UserFaker.Get().Generate(20);
                Debug.WriteLine($">>> Generated {users.Count} users");
                var companies = CompanyEntityFaker.Get().Generate(5);
                Debug.WriteLine($">>> Generated {companies.Count} companies");
                var itens = ItensUniformesFaker.Get().Generate(10);
                Debug.WriteLine($">>> Generated {itens.Count} uniform items");

                // Inserir no contexto
                Debug.WriteLine(">>> Inserting users");
                _db.Users.AddRange(users);
                Debug.WriteLine(">>> Inserting companies");
                _db.Companies.AddRange(companies);
                Debug.WriteLine(">>> Inserting user-companies");
                await _db.UserCompanies.AddRangeAsync(
                    users.SelectMany(u =>
                    {
                        var count = new Random().Next(1, 4);
                        var selected = companies.OrderBy(_ => Guid.NewGuid()).Take(count);
                        return selected.Select(c => new UserCompany
                        {
                            UserId = u.Id,
                            CompanyId = c.Id,
                            Role = new Faker().PickRandom(new[] { "Admin", "Manager", "User" }),
                            CreatedAt = DateTime.UtcNow
                        });
                    })
                );
                Debug.WriteLine(">>> Inserting uniform items");
                _db.ItensUniformes.AddRange(itens);

                // Funcionários, folhas e atribuições
                Debug.WriteLine(">>> Generating and inserting employees, payrolls and assignments");
                var funcionarios = new List<DomainFuncionario>();
                var folhas = new List<DomainFolha>();
                var atribuicoes = new List<DomainAtribuicao>();
                int funcId = 1;

                foreach (var comp in companies)
                {
                    var funcCount = new Random().Next(5, 11);
                    for (int i = 0; i < funcCount; i++)
                    {
                        var func = FuncionarioFaker.Get(comp.Id).Generate();
                        func.Id = funcId++;
                        funcionarios.Add(func);

                        var folha = FolhaDePagamentoFaker.Get(func.Id).Generate();
                        folhas.Add(folha);

                        var assignCount = new Random().Next(1, 4);
                        var selectedItems = itens.OrderBy(_ => Guid.NewGuid()).Take(assignCount);
                        atribuicoes.AddRange(
                            selectedItems.Select(item => AtribuicaoUniformesFaker.Get(func.Id, item.Id).Generate())
                        );
                    }
                }
                Debug.WriteLine($">>> Prepared {funcionarios.Count} employees, {folhas.Count} payrolls and {atribuicoes.Count} assignments");

                // Reassign unique IDs for payrolls and assignments to avoid tracking conflicts
                for (int i = 0; i < folhas.Count; i++) folhas[i].Id = i + 1;
                for (int i = 0; i < atribuicoes.Count; i++) atribuicoes[i].Id = i + 1;
                _db.Funcionarios.AddRange(funcionarios);
                _db.FolhaDePagamentos.AddRange(folhas);
                _db.AtribuicaoUniformes.AddRange(atribuicoes);

                Debug.WriteLine(">>> Saving changes");
                await _db.SaveChangesAsync();

                var afterCount = await _db.Users.CountAsync();
                Debug.WriteLine($">>> Users after seed: {afterCount}");
                Debug.WriteLine(">>> Seed completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(">>> Seed failed: " + ex.Message);
                Debug.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
