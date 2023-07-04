using Microsoft.EntityFrameworkCore;
using PlanIt.API.DbContexts;
using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly PlanItDbContext _context;

    public RatingRepository(PlanItDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Rating?> GetRatingAsync(int userId, int experienceId)
    {
        var rating = await _context.Ratings.FirstOrDefaultAsync(
            r => r.UserId == userId && r.ExperienceId == experienceId);
        return rating;
    }

    public async Task AddRatingAsync(Rating rating)
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}