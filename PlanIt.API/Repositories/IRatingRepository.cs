using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public interface IRatingRepository
{
    Task<Rating?> GetRatingAsync(int userId, int experienceId);
    Task AddRatingAsync(Rating rating);
}