using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;

namespace PlanIt.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UsersDataStore _usersDataStore;

    public UsersController(UsersDataStore usersDataStore)
    {
        _usersDataStore = usersDataStore  ?? throw new ArgumentNullException(nameof(usersDataStore));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers()
    {
        return Ok(_usersDataStore.Users);
    }
    
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetCity(int id)
    {
        var city = _usersDataStore.Users.FirstOrDefault(c => c.Id == id);

        if (city == null)
            return NotFound();

        return Ok(city);
    }
}