using AutoMapper;
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
    private readonly IMapper _mapper;

    public UsersController(ILogger<ExperiencesController> logger,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithoutExperiencesDto>>> GetUsers()
    {
        var userEntities = await _userRepository.GetUsersAsync();
        // var users = userEntities.Select(u => new UserDto()
        // {
        //     Id = u.Id,
        //     Username = u.Username,
        //     Email = u.Email
        // });
        var users = _mapper.Map<IEnumerable<UserWithoutExperiencesDto>>(userEntities);
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id, bool includeExperiences = false)
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

            // var user = new UserDto()
            // {
            //     Id = userEntity.Id,
            //     Username = userEntity.Username,
            //     Experiences = userEntity.Experiences.Select(e => new ExperienceDto()
            //     {
            //         Id = e.Id,
            //         Title = e.Title,
            //         Description = e.Description,
            //         City = e.City,
            //         State = e.State,
            //         Country = e.Country
            //     }).ToList()
            // };

            if (includeExperiences)
                return Ok(_mapper.Map<UserDto>(userEntity));
            
            return Ok(_mapper.Map<UserWithoutExperiencesDto>(userEntity));
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Exception while getting user with id {id}.", ex);
            return StatusCode(500, "A problem occured while handling the request.");
        }
    }
}