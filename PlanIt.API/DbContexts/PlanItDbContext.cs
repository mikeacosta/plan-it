using Microsoft.EntityFrameworkCore;
using PlanIt.API.Entities;

namespace PlanIt.API.DbContexts;

public class PlanItDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Experience> Experiences { get; set; } = null!;

    public DbSet<Rating> Ratings { get; set; } = null!;
    
    public PlanItDbContext(DbContextOptions<PlanItDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>().HasKey(t => new { t.UserId, t.ExperienceId });
        
        modelBuilder.Entity<User>().HasData(
            new User("joeblow", "joe@blow.com") {Id = 1},
            new User("mickey.mouse", "mickey@disney.com") {Id = 2},
            new User("anonymous", "user@gmail.com") {Id = 3}
        );
        
        modelBuilder.Entity<Experience>().HasData(
            new Experience("Amsterdam Open Boat Canal Cruise - Live Guide - from Anne Frank House", 
                "Amsterdam", "Netherlands")
            {
                Id = 1,
                UserId = 1,
                Description = "Experience the beauty of Amsterdam’s canals by going on this scenic cruise. Pass by Anne Frank House, the Jordaan, the Houseboat Museum, Leiden Square, Rijksmuseum, De Duif and much more."
            },
            new Experience("Dubai: Red Dunes Quad Bike, Sandsurf, Camels & BBQ at Al Khayma Camp", 
                "Dubai", "United Arab Emirates")
            {
                Id = 2,
                UserId = 1,
                Description = "Experience several desert pursuits in one outing, including ATV-driving—something many tours only offer at an extra cost—on this red-dune desert tour from Dubai. Skip the hassle of transport and logistical planning; and be free to simply enjoy the dunes and activities provided. Zoom off on an ATV, ride a camel, go sandboarding; enjoy henna art and Arabian-costume photos; and conclude with a barbecue-buffet dinner and live shows."
            },
            new Experience("Tour of North Shore and Sightseeing", 
                "Haleiwa", "United States")
            {
                Id = 3,
                UserId = 2,
                State = "Hawaii",
                Description = "Skip the hassle of renting a car and see the highlights of Oahu’s North Shore from the comfort of a minibus or van. Along the way, a guide keeps you entertained and shares details about the island that you would likely miss if traveling on your own. At each stop, you can enjoy free time to swim, shop, paddleboard/kayak or do an optional waterfall hike while getting to know the island."
            },
            new Experience("Tower of London", 
                "London", "United Kingdom")
            {
                Id = 4,
                UserId = 2,
                Description = "This historic castle with over 1,000 years of history is home to the Crown Jewels, the iconic 'Beefeater' Yeoman Warders, and the legendary ravens that have kept the kingdom from collapsing. Inside the White Tower, the oldest building of the castle, is an 11th-century chapel and historic Royal Armouries collections."
            },
            new Experience("Gyeongbokgung Palace",
                "Seoul", "South Korea")
            {
                Id = 5,
                UserId = 3,
                Description = "The National Museum of Korea and the National Folk Museum are located on the grounds of this palace, built six centuries ago by the founder of the Chosun dynasty."
            },
            new Experience("Jardin Majorelle",
                "Marrakech", "Morocco")
            {
                Id = 6,
                UserId = 3,
                Description = "Nicely designed and maintained gardens, similar to those of Generalife in Granada, Spain. It’s a good place to recoup from the intensity of the market atmosphere."
            }
        );
        
        modelBuilder.Entity<Rating>().HasData(
            new Rating() {StarCount = 5, UserId = 1, ExperienceId = 1},
            new Rating() {StarCount = 4, UserId = 1, ExperienceId = 2},
            new Rating() {StarCount = 2, UserId = 2, ExperienceId = 1}
        );
        
        base.OnModelCreating(modelBuilder);
    }
}