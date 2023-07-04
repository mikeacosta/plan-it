using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using PlanIt.API.Entities;

namespace PlanIt.API.Profiles;

public class ExperienceProfile : Profile
{
    public ExperienceProfile()
    {
        CreateMap<Entities.Experience, PlanIt.Models.DTOs.ExperienceDto>()
            .ForMember(dest => dest.AverageRating, 
                opt => opt.MapFrom(
                    src => GetAvgRating(src)));
        CreateMap<PlanIt.Models.DTOs.ExperienceCreationDto, Entities.Experience>();
        CreateMap<PlanIt.Models.DTOs.ExperienceUpdateDto, Entities.Experience>();
        CreateMap<Entities.Experience, PlanIt.Models.DTOs.ExperienceUpdateDto>();
    }

    private double GetAvgRating(Experience experience)
    {
        if (experience.Ratings.IsNullOrEmpty())
            return 0.0;
        
        return experience.Ratings.Average(r => r.StarCount);
    }
}