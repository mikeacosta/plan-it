using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Repositories;
using PlanIt.Models.DTOs;

namespace PlanIt.API.Controllers;

[Route("api/v{version:apiVersion}/users/{userId}/experiences")]
[ApiController]
[Authorize]
public class ExperienceController : ControllerBase
{
    private readonly ILogger<ExperienceController> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IExperienceRepository _experienceRepository;
    private readonly IMapper _mapper;
    private const int MaxPageSize = 20;

    public ExperienceController(ILogger<ExperienceController> logger,
        IUserRepository userRepository,
        IExperienceRepository experienceRepository,
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Get all experiences
    /// </summary>
    [HttpGet]
    [Route("~/api/v{version:apiVersion}/experiences/all")]
    public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetAllExperiences(
        string? searchQuery, int pageNumber = 1, int pageSize = 10)
    {
        if (pageSize > MaxPageSize)
            pageSize = MaxPageSize;
        
        var (entities, pageMetadata) = await _experienceRepository
            .GetExperiencesAsync(searchQuery, pageNumber, pageSize);
        
        Response.Headers.Add("X-Pagination",
            JsonSerializer.Serialize(pageMetadata));

        var experiences = _mapper.Map<IEnumerable<ExperienceDto>>(entities);
        return Ok(experiences);
    }
    
    /// <summary>
    /// Get experiences for a user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExperienceDto>>> GetExperiences(
        int userId)
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

    /// <summary>
    /// Enter one experience for a user
    /// </summary>
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
    
    /// <summary>
    /// Create an experience
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ExperienceDto>> CreateExperience(int userId,
        ExperienceCreationDto experience)
    {
        if (!await _userRepository.UserExistsAsync(userId))
            return NotFound();

        var createdExperience = _mapper.Map<Entities.Experience>(experience);

        await _userRepository.AddExperienceForUserAsync(userId, createdExperience);
        await _userRepository.SaveChangesAsync();

        var result = _mapper.Map<ExperienceDto>(createdExperience);
        
        return CreatedAtRoute("GetExperience",
            new
            {
                userId = userId,
                experienceId = result.Id
            },
            result);
    }
    
    /// <summary>
    /// Update an experience
    /// </summary>
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
    
    /// <summary>
    /// Partial update to an experience
    /// </summary>
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
    
    /// <summary>
    /// Delete an experience
    /// </summary>
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