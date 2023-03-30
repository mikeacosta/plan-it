using System.ComponentModel.DataAnnotations;

namespace PlanIt.API.Models;

public class ExperienceCreationDto
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    [Required(ErrorMessage = "City is required")]
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? State { get; set; }
    
    [Required(ErrorMessage = "Country is required")]
    [MaxLength(50)]
    public string Country { get; set; } = string.Empty;
}