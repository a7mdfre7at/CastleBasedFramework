using AutoMapper;

namespace Castle.Mvc.Models
{
    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.UserAge, cfg => cfg.MapFrom(s => s.Age))
                .ForMember(d => d.UserName, cfg => cfg.MapFrom(s => s.Name));
        }
    }
}