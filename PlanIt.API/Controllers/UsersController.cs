using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;

namespace PlanIt.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UsersDataStore _usersDataStore;
    private readonly ILogger<ExperiencesController> _logger;

    public UsersController(UsersDataStore usersDataStore,
        ILogger<ExperiencesController> logger)
    {
        _usersDataStore = usersDataStore ?? throw new ArgumentNullException(nameof(usersDataStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers()
    {
        return Ok(_usersDataStore.Users);
    }
    
    [HttpGet("{id}")]
    public ActionResult<UserDto> GetUser(int id)
    {
        try
        {
            var user = _usersDataStore.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                _logger.LogInformation(
                    $"User with id {id} wasn't found.");
                return NotFound();
            }
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Exception while getting user with id {id}.", ex);
            return StatusCode(500, "A problem occured while handling the request.");
        }
    }
}