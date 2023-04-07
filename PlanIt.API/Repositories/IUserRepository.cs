using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User?> GetUserAsync(int userId, bool includeExperiences);
    Task<bool> UserExistsAsync(int userId);
    Task<IEnumerable<Experience>> GetExperiencesForUserAsync(int userId);
    Task<Experience?> GetExperienceForUserAsync(int userId, int experienceId);
    Task AddExperienceForUserAsync(int userId, Experience experience);
    Task<bool> SaveChangesAsync();
}