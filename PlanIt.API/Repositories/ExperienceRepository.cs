using Microsoft.EntityFrameworkCore;
using PlanIt.API.DbContexts;
using PlanIt.API.Entities;

namespace PlanIt.API.Repositories;

public class ExperienceRepository : IExperienceRepository
{
    private readonly PlanItDbContext _context;
    
    public ExperienceRepository(PlanItDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<(IEnumerable<Experience>, PageMetadata)> GetExperiencesAsync(
        string? searchQuery, int pageNumber, int pageSize)
    {
        var experiences = _context.Experiences
            .Include(e => e.Ratings) as IQueryable<Experience>;

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            experiences = experiences.Where(e => e.Title.Contains(searchQuery)
                || e.Description.Contains(searchQuery)
                || e.City.Contains(searchQuery)
                || e.State.Contains(searchQuery)
                || e.Country.Contains(searchQuery));
        }
        
        var count = await experiences.CountAsync();
        var pageMetadata = new PageMetadata(count, pageSize, pageNumber);
        
        var result = await experiences
            .OrderBy(e => e.Title)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (result, pageMetadata);
    }
}