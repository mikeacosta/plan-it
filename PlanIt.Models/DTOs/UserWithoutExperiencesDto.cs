namespace PlanIt.Models.DTOs;

public class UserWithoutExperiencesDto
{
    public int Id { get; set; }
    
    public string Username { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;
}