using AutoMapper;
using MyRhSystem.Contracts.Employees;
using MyRhSystem.Domain.Entities.Employees;

namespace MyRhSystem.API.Mapper
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            // Lista
            CreateMap<Employee, EmployeeDto>();

            // Detalhe
            CreateMap<Employee, EmployeeDetailsDto>()
                .ForMember(d => d.Beneficios, opt => opt.MapFrom(s => s.EmployeeBenefits.Select(b => b.BenefitType.Nome).ToArray()))
                .ForMember(d => d.Dependentes, opt => opt.MapFrom(s => s.Dependents))
                .ForMember(d => d.Contatos, opt => opt.MapFrom(s => s.Contacts));

            // Coleções simples (ajuste os nomes das entidades conforme seu Domain)
            CreateMap<EmployeeDependents, DependenteDto>();
            CreateMap<EmployeeContacts, ContatoDto>();

            // Requests -> Entity (Create/Update)
            CreateMap<CreateEmployeesRequest, Employee>()
                .ForMember(d => d.Id, opt => opt.Ignore()); // Id gerado pelo banco

            CreateMap<UpdateEmployeesRequest, Employee>();
        }
    }
}
