using AutoMapper;

namespace PlanIt.API.Profiles;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        CreateMap<Entities.Rating, PlanIt.Models.DTOs.RatingDto>();
    }
}