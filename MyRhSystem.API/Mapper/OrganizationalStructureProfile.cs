using AutoMapper;
using MyRhSystem.Contracts.Departments;
using MyRhSystem.Contracts.JobRole;
using MyRhSystem.Domain.Entities.Departments;
using MyRhSystem.Domain.Entities.JobRoles;

namespace MyRhSystem.API.Profiles;

public sealed class OrganizationalStructureProfile : Profile
{
    public OrganizationalStructureProfile()
    {
        // Department
        CreateMap<Department, DepartmentDto>().ReverseMap();

        // JobLevel
        CreateMap<JobLevels, JobLevelDto>().ReverseMap();

        // JobRole
        CreateMap<JobRole, JobRoleDto>().ReverseMap();
    }
}
