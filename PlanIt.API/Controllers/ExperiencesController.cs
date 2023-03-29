using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Models;

namespace PlanIt.API.Controllers;

[Route("api/users/{userId}/experiences")]
[ApiController]
public class ExperiencesController : ControllerBase
{
    private readonly UsersDataStore _usersDataStore;

    public ExperiencesController(UsersDataStore usersDataStore)
    {
        _usersDataStore = usersDataStore;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<ExperienceDto>> GetPointsOfInterest(
        int userId)
    {
        var user = _usersDataStore.Users.FirstOrDefault(c => c.Id == userId);

        if (user == null)
            return NotFound();

        return Ok(user.Experiences);
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
}