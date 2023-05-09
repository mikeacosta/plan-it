using AutoMapper;

namespace PlanIt.API.Profiles;

public class ExperienceProfile : Profile
{
    public ExperienceProfile()
    {
        CreateMap<Entities.Experience, PlanIt.Models.DTOs.ExperienceDto>();
        CreateMap<PlanIt.Models.DTOs.ExperienceCreationDto, Entities.Experience>();
        CreateMap<PlanIt.Models.DTOs.ExperienceUpdateDto, Entities.Experience>();
        CreateMap<Entities.Experience, PlanIt.Models.DTOs.ExperienceUpdateDto>();
    }
}