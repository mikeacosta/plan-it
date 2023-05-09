using AutoMapper;

namespace PlanIt.API.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Entities.User, PlanIt.Models.DTOs.UserWithoutExperiencesDto>();
        CreateMap<Entities.User, PlanIt.Models.DTOs.UserDto>();
    }
}