using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public interface IExperienceRepository
{
    Task<(IEnumerable<Experience>, PageMetadata)> GetExperiencesAsync(
        string? searchQuery, int pageNumber, int pageSize);
}