using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;

namespace PlanIt.API.Controllers;

[Route("api/users/{userId}/experiences")]
[ApiController]
public class ExperiencesController : ControllerBase
{
    private readonly UsersDataStore _usersDataStore;
    private readonly ILogger<ExperiencesController> _logger;

    public ExperiencesController(UsersDataStore usersDataStore,
        ILogger<ExperiencesController> logger)
    {
        _usersDataStore = usersDataStore ?? throw new ArgumentNullException(nameof(usersDataStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<ExperienceDto>> GetExperiences(
        int userId)
    {
        try
        {
            var user = _usersDataStore.Users.FirstOrDefault(c => c.Id == userId);

            if (user == null)
            {
                _logger.LogInformation(
                    $"User with id {userId} wasn't found when accessing experiences.");
                return NotFound();
            }
            return Ok(user.Experiences);
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Exception while getting experiences for user with id {userId}.", ex);
            return StatusCode(500, "A problem occured while handling the request.");
        }
    }

    [HttpGet("{experienceId}", Name = "GetExperience")]
    public ActionResult<ExperienceDto> GetExperience(int userId, int experienceId)
    {
        var user = (_usersDataStore.Users.FirstOrDefault(c => c.Id == userId));

        if (user == null)
            return NotFound();

        var experience = user.Experiences
            .FirstOrDefault(p => p.Id == experienceId);

        if (experience == null)
            return NotFound();

        return Ok(experience);
    }
    
    [HttpPost]
    public ActionResult<ExperienceDto> CreateExperience(int userId,
        ExperienceCreationDto experience)
    {
        var user = _usersDataStore.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        var maxExperienceId = _usersDataStore.Users.SelectMany(u => u.Experiences)
            .Max(e => e.Id);

        var createdExperience = new ExperienceDto()
        {
            Id = ++maxExperienceId,
            Title = experience.Title,
            Description = experience.Description,
            City = experience.City,
            State = experience.State,
            Country = experience.Country
        };

        user.Experiences.Add(createdExperience);

        return CreatedAtRoute("GetExperience",
            new
            {
                userId = userId,
                experienceId = createdExperience.Id
            },
            createdExperience);
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