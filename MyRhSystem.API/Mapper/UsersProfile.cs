using MyRhSystem.Contracts.Users;
using MyRhSystem.Domain.Entities.Users;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyRhSystem.API.Mapper
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserRequest, User>()
                .ForMember(d => d.Password, opt => opt.Ignore()); // vamos setar manualmente
            CreateMap<UpdateUserRequest, User>()
                .ForMember(d => d.Password, opt => opt.Ignore());
        }
    }
}
