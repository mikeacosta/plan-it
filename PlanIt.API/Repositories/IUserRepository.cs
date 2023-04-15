using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<(IEnumerable<User>, PageMetadata)> GetUsersAsync(
        string? username, string? searchQuery, int pageNumber, int pageSize);
    Task<User?> GetUserAsync(int userId, bool includeExperiences);
    Task<bool> UserExistsAsync(int userId);
    Task<IEnumerable<Experience>> GetExperiencesForUserAsync(int userId);
    Task<Experience?> GetExperienceForUserAsync(int userId, int experienceId);
    Task AddExperienceForUserAsync(int userId, Experience experience);
    void DeleteExperience(Experience experience);
    Task<bool> SaveChangesAsync();
}