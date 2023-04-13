using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<IEnumerable<User>> GetUsersAsync(string? username, string? searchQuery);
    Task<User?> GetUserAsync(int userId, bool includeExperiences);
    Task<bool> UserExistsAsync(int userId);
    Task<IEnumerable<Experience>> GetExperiencesForUserAsync(int userId);
    Task<Experience?> GetExperienceForUserAsync(int userId, int experienceId);
    Task AddExperienceForUserAsync(int userId, Experience experience);
    void DeleteExperience(Experience experience);
    Task<bool> SaveChangesAsync();
}