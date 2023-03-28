namespace PlanIt.API.Models;

public class UserDto
{
    public int Id { get; set; }
    
    public string Username { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;
    
    public int NumberOfExperiences { get; set; }
}