using AutoMapper;

namespace PlanIt.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Entities.User, Models.UserWithoutExperiencesDto>();
        CreateMap<Entities.User, Models.UserDto>();
    }
}