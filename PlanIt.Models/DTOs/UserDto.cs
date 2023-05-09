namespace PlanIt.Models.DTOs;

public class UserDto
{
    public int Id { get; set; }
    
    public string Username { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;
    
    public int NumberOfExperiences => Experiences.Count;

    public ICollection<ExperienceDto> Experiences { get; set; }
        = new List<ExperienceDto>();
}