using Microsoft.EntityFrameworkCore;
using PlanIt.API.DbContexts;
using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PlanItDbContext _context;
    
    public UserRepository(PlanItDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await _context.Users.OrderBy(u => u.Username).ToListAsync();
    }

    public async Task<User?> GetUserAsync(int userId, bool includeExperiences)
    {
        if (includeExperiences)
        {
            return await _context.Users.Include(u => u.Experiences)
                .Where(u => u.Id == userId).FirstOrDefaultAsync();
        }

        return await _context.Users
            .Where(u => u.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<IEnumerable<Experience>> GetExperiencesForUserAsync(int userId)
    {
        return await _context.Experiences
            .Where(e => e.UserId == userId).ToListAsync();
    }

    public async Task<Experience?> GetExperienceForUserAsync(int userId, int experienceId)
    {
        return await _context.Experiences
            .Where(e => e.UserId == userId && e.Id == experienceId)
            .FirstOrDefaultAsync();
    }

    public async Task AddExperienceForUserAsync(int userId, Experience experience)
    {
        var user = await GetUserAsync(userId, false);
        user?.Experiences.Add(experience);
    }

    public void DeleteExperience(Experience experience)
    {
        _context.Experiences.Remove(experience);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}