using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanIt.API.Entities;

public class Experience
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string City { get; set; }

    [MaxLength(20)]
    public string? State { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Country { get; set; }
    
    [ForeignKey("UserId")]
    public User? User { get; set; }
    public int UserId { get; set; }

    public Experience(string title, string city, string country)
    {
        Title = title;
        City = city;
        Country = country;
    }
}