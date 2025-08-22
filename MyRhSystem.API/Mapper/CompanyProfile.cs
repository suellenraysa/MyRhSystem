using AutoMapper;
using MyRhSystem.APP.Shared.Models;
using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Companies;
using MyRhSystem.Domain.Entities.Companies;
using MyRhSystem.Domain.Entities.ValueObjects;

namespace MyRhSystem.API.Mapper;

public class CompanyProfile : Profile
{
    public CompanyProfile()
    {
        CreateMap<CreateCompanyRequest, Company>()
           .ForMember(d => d.CreatedAt, o => o.Ignore())
           .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
           .ForMember(d => d.Representative, o => o.MapFrom(s => s.Representative));

        CreateMap<AddressCreateRequest, Address>();
        CreateMap<RepresentativeCreateRequest, LegalRepresentativeModel>();

        CreateMap<Company, CompanyDTO>();
    }
}
