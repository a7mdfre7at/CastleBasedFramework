using AutoMapper;

namespace Castle.Mvc.Models
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>()
                .ForMember(d => d.Age, cfg => cfg.MapFrom(s => s.UserAge))
                .ForMember(d => d.Name, cfg => cfg.MapFrom(s => s.UserName));
        }
    }
}