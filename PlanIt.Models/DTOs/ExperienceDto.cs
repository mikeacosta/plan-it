namespace PlanIt.Models.DTOs;

public class ExperienceDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    
    public string City { get; set; } = string.Empty;

    public string? State { get; set; }
    
    public string Country { get; set; } = string.Empty;

    public double AverageRating { get; set; } = 0.0;
}