using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;
using PlanIt.API.Repositories;

namespace PlanIt.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private const int maxPageSize = 20;

    public UsersController(ILogger<UsersController> logger,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserWithoutExperiencesDto>>> GetUsers(
        string? username, string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > maxPageSize)
            pageSize = maxPageSize;
        
        var (userEntities, pageMetadata) = await _userRepository
            .GetUsersAsync(username, searchQuery, pageNumber, pageSize);
        
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pageMetadata));
        
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