using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;
using PlanIt.API.Repositories;

namespace PlanIt.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<ExperiencesController> _logger;
    private readonly IUserRepository _userRepository;

    public UsersController(ILogger<ExperiencesController> logger,
        IUserRepository userRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var userEntities = await _userRepository.GetUsersAsync();
        var users = userEntities.Select(u => new UserDto()
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email
        });
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            var userEntity = await _userRepository.GetUserAsync(id, true);

            if (userEntity == null)
            {
                _logger.LogInformation(
                    $"User with id {id} wasn't found.");
                return NotFound();
            }

            var user = new UserDto()
            {
                Id = userEntity.Id,
                Username = userEntity.Username,
                Experiences = userEntity.Experiences.Select(e => new ExperienceDto()
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    City = e.City,
                    State = e.State,
                    Country = e.Country
                }).ToList()
            };
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Exception while getting user with id {id}.", ex);
            return StatusCode(500, "A problem occured while handling the request.");
        }
    }
}