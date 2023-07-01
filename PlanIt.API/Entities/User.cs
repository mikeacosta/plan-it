using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlanIt.API.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    public ICollection<Experience> Experiences { get; set; }
        = new List<Experience>();

    public ICollection<Rating> Ratings { get; set; }
        = new List<Rating>();

    public User(string username, string email)
    {
        Username = username;
        Email = email;
    }
}