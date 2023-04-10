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
    private readonly ILogger<ExperiencesController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ExperiencesController(ILogger<ExperiencesController> logger,
        IUserRepository userRepository,
        IMapper mapper)
    {
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
    public async Task<ActionResult> UpdateExperience(int userId, int experienceId,
        ExperienceUpdateDto experience)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var experienceEntity = await _userRepository.GetExperienceForUserAsync(userId, experienceId);
        if (experienceEntity == null)
            return NotFound();

        _mapper.Map(experience, experienceEntity);
        await _userRepository.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpPatch("{experienceId}")]
    public async Task<ActionResult> PartiallyUpdateExperience(
        int userId, int experienceId,
        JsonPatchDocument<ExperienceUpdateDto> patchDocument)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var experienceEntity = await _userRepository.GetExperienceForUserAsync(userId, experienceId);
        if (experienceEntity == null)
            return NotFound();

        var experienceToPatch = _mapper.Map<ExperienceUpdateDto>(experienceEntity);

        patchDocument.ApplyTo(experienceToPatch, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!TryValidateModel(experienceToPatch))
            return BadRequest(ModelState);

        _mapper.Map(experienceToPatch, experienceEntity);
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{experienceId}")]
    public async Task<ActionResult> DeleteExperience(int userId, int experienceId)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var experienceEntity = await _userRepository.GetExperienceForUserAsync(userId, experienceId);
        if (experienceEntity == null)
            return NotFound();

        _userRepository.DeleteExperience(experienceEntity);
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }
}