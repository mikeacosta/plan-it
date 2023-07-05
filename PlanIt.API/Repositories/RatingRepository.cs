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

    public async Task UpsertRatingAsync(Rating rating)
    {
        var existingRating = await GetRatingAsync(rating.UserId, rating.ExperienceId);

        if (existingRating == null)
        {
            await _context.Ratings.AddAsync(rating);
        }
        else
        {
            existingRating.StarCount = rating.StarCount;
            _context.Ratings.Update(existingRating);
        }
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}