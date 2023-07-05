using AutoMapper;

namespace PlanIt.API.Profiles;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        CreateMap<Entities.Rating, Models.DTOs.RatingDto>();
        CreateMap<Models.DTOs.RatingCreationDto, Entities.Rating>();
        CreateMap<Models.DTOs.RatingDto, Entities.Rating>();
    }
}