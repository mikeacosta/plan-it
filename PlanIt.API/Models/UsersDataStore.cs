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
                Email = "joe@blow.com"
            },
            new UserDto()
            {
                Id = 2,
                Username = "mickey.mouse",
                Email = "mickey@disney.com"
            },
            new UserDto()
            {
                Id = 3,
                Username = "anonymous",
                Email = "user@gmail.com"
            }
        };
    }
}