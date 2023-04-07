using AutoMapper;

namespace PlanIt.API.Profiles;

public class ExperienceProfile : Profile
{
    public ExperienceProfile()
    {
        CreateMap<Entities.Experience, Models.ExperienceDto>();
        CreateMap<Models.ExperienceCreationDto, Entities.Experience>();
    }
}