namespace PlanIt.API.Models;

public class UsersDataStore
{
    public List<UserDto> Users { get; set; }

    public UsersDataStore()
    {
        Users = new List<UserDto>()
        {
            new UserDto()
            {
                Id = 1,
                Username = "joeblow",
                Email = "joe@blow.com",
                Experiences = new List<ExperienceDto>()
                {
                    new ExperienceDto()
                    {
                        Id = 1,
                        Title = "Amsterdam Open Boat Canal Cruise - Live Guide - from Anne Frank House",
                        Description = "Experience the beauty of Amsterdam’s canals by going on this scenic cruise. Pass by Anne Frank House, the Jordaan, the Houseboat Museum, Leiden Square, Rijksmuseum, De Duif and much more.",
                        City = "Amsterdam",
                        Country = "Netherlands"
                    },
                    new ExperienceDto()
                    {
                        Id = 2,
                        Title = "Dubai: Red Dunes Quad Bike, Sandsurf, Camels & BBQ at Al Khayma Camp",
                        Description = "Experience several desert pursuits in one outing, including ATV-driving—something many tours only offer at an extra cost—on this red-dune desert tour from Dubai. Skip the hassle of transport and logistical planning; and be free to simply enjoy the dunes and activities provided. Zoom off on an ATV, ride a camel, go sandboarding; enjoy henna art and Arabian-costume photos; and conclude with a barbecue-buffet dinner and live shows.",
                        City = "Dubai",
                        Country = "United Arab Emirates"                     
                    }
                }
            },
            new UserDto()
            {
                Id = 2,
                Username = "mickey.mouse",
                Email = "mickey@disney.com",
                Experiences = new List<ExperienceDto>()
                {
                    new ExperienceDto()
                    {
                        Id = 3,
                        Title = "Tour of North Shore and Sightseeing",
                        Description = "Skip the hassle of renting a car and see the highlights of Oahu’s North Shore from the comfort of a minibus or van. Along the way, a guide keeps you entertained and shares details about the island that you would likely miss if traveling on your own. At each stop, you can enjoy free time to swim, shop, paddleboard/kayak or do an optional waterfall hike while getting to know the island.",
                        City = "Haleiwa",
                        State = "Hawaii",
                        Country = "United States"
                    },
                    new ExperienceDto()
                    {
                        Id = 4,
                        Title = "Tower of London",
                        Description = "This historic castle with over 1,000 years of history is home to the Crown Jewels, the iconic 'Beefeater' Yeoman Warders, and the legendary ravens that have kept the kingdom from collapsing. Inside the White Tower, the oldest building of the castle, is an 11th-century chapel and historic Royal Armouries collections.",
                        City = "London",
                        Country = "United Kindgom"                     
                    }
                }
            },
            new UserDto()
            {
                Id = 3,
                Username = "anonymous",
                Email = "user@gmail.com",
                Experiences = new List<ExperienceDto>()
                {
                    new ExperienceDto()
                    {
                        Id = 5,
                        Title = "Gyeongbokgung Palace",
                        Description = "The National Museum of Korea and the National Folk Museum are located on the grounds of this palace, built six centuries ago by the founder of the Chosun dynasty.",
                        City = "Seoul",
                        Country = "South Korea"
                    },
                    new ExperienceDto()
                    {
                        Id = 6,
                        Title = "Jardin Majorelle",
                        Description = "Nicely designed and maintained gardens, similar to those of Generalife in Granada, Spain. It’s a good place to recoup from the intensity of the market atmosphere.",
                        City = "Marrakech",
                        Country = "Morocco"                     
                    }
                }
            }
        };
    }
}