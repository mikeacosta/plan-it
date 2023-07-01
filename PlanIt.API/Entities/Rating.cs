using System.ComponentModel.DataAnnotations.Schema;

namespace PlanIt.API.Entities;

public class Rating
{
    public int StarCount { get; set; }
    
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public int UserId { get; set; }
    
    [ForeignKey("ExperienceId")]
    public Experience? Experience { get; set; }
    public int ExperienceId { get; set; }
}