using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;
using PlanIt.API.Repositories;

namespace PlanIt.API.Controllers;

[Route("api/users/{userId}/experiences")]
[ApiController]
public class ExperiencesController : ControllerBase
{
    private readonly UsersDataStore _usersDataStore;
    private readonly ILogger<ExperiencesController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ExperiencesController(UsersDataStore usersDataStore,
        ILogger<ExperiencesController> logger,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _usersDataStore = usersDataStore ?? throw new ArgumentNullException(nameof(usersDataStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences(
        int userId)
    {
        try
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                _logger.LogInformation(
                    $"User with id {userId} wasn't found when accessing experiences.");
                return NotFound();
            }
            
            var experiences = await _userRepository.GetExperiencesForUserAsync(userId);

            return Ok(_mapper.Map<IEnumerable<ExperienceDto>>(experiences));
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Exception while getting experiences for user with id {userId}.", ex);
            return StatusCode(500, "A problem occured while handling the request.");
        }
    }

    [HttpGet("{experienceId}", Name = "GetExperience")]
    public async Task<ActionResult<ExperienceDto>> GetExperience(int userId, int experienceId)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var experience = await _userRepository.GetExperienceForUserAsync(userId, experienceId);

        if (experience == null)
            return NotFound();

        return Ok(_mapper.Map<ExperienceDto>(experience));
    }
    
    [HttpPost]
    public async Task<ActionResult<ExperienceDto>> CreateExperience(int userId,
        ExperienceCreationDto experience)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var createdExperience = _mapper.Map<Entities.Experience>(experience);

        await _userRepository.AddExperienceForUserAsync(userId, createdExperience);
        await _userRepository.SaveChangesAsync();

        var result = _mapper.Map<Models.ExperienceDto>(createdExperience);
        
        return CreatedAtRoute("GetExperience",
            new
            {
                userId = userId,
                experienceId = result.Id
            },
            result);
    }
    
    [HttpPut("{experienceId}")]
    public ActionResult UpdateExperience(int userId, int experienceId,
        ExperienceUpdateDto experience)
    {
        var user = _usersDataStore.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        var experienceFromStore = user.Experiences.FirstOrDefault(e => e.Id == experienceId);
        if (experienceFromStore == null)
            return NotFound();

        experienceFromStore.Title = experience.Title;
        experienceFromStore.Description = experience.Description;
        experienceFromStore.City = experience.City;
        experienceFromStore.State = experience.State;
        experienceFromStore.Country = experience.Country;

        return NoContent();
    }
    
    [HttpPatch("{experienceId}")]
    public ActionResult PartiallyUpdateExperience(
        int userId, int experienceId,
        JsonPatchDocument<ExperienceUpdateDto> patchDocument)
    {
        var user = _usersDataStore.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        var experienceFromStore = user.Experiences.FirstOrDefault(e => e.Id == experienceId);
        if (experienceFromStore == null)
            return NotFound();

        var experienceToPatch = new ExperienceUpdateDto()
        {
            Title = experienceFromStore.Title,
            Description = experienceFromStore.Description,
            City = experienceFromStore.City,
            State = experienceFromStore.State,
            Country = experienceFromStore.Country
        };

        patchDocument.ApplyTo(experienceToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(experienceToPatch))
            return BadRequest(ModelState);

        experienceFromStore.Title = experienceToPatch.Title;
        experienceFromStore.Description = experienceToPatch.Description;
        experienceFromStore.City = experienceToPatch.City;
        experienceFromStore.State = experienceToPatch.State;
        experienceFromStore.Country = experienceToPatch.Country;

        return NoContent();
    }
    
    [HttpDelete("{experienceId}")]
    public ActionResult DeleteExperience(int userId, int experienceId)
    {
        var user = _usersDataStore.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        var experienceFromStore = user.Experiences.FirstOrDefault(e => e.Id == experienceId);
        if (experienceFromStore == null)
            return NotFound();

        user.Experiences.Remove(experienceFromStore);

        return NoContent();
    }
}