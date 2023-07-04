using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlanIt.API.Repositories;
using PlanIt.Models.DTOs;

namespace PlanIt.API.Controllers;

[Route("api")]
[ApiController]
public class RatingController : ControllerBase
{
    private readonly ILogger<RatingController> _logger;
    private readonly IRatingRepository _ratingRepository;
    private readonly IMapper _mapper;
    
    public RatingController(ILogger<RatingController> logger,
        IRatingRepository ratingRepository, 
        IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _ratingRepository = ratingRepository ?? throw new ArgumentNullException(nameof(ratingRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Get all experiences
    /// </summary>
    [HttpGet]
    [Route("users/{userId}/experiences/{experienceId}/ratings")]
    public async Task<ActionResult<RatingDto>> GetRating(int userId, int experienceId)
    {
        try
        {
            var rating = await _ratingRepository.GetRatingAsync(userId, experienceId);

            if (rating == null)
                return NotFound();
        
            return Ok(_mapper.Map<RatingDto>(rating));
        }
        catch (Exception ex)
        {
            var msg = $"Exception while getting rating for user {userId} and experience {experienceId}.";
            _logger.LogCritical(msg, ex);
            return StatusCode(500, msg);
        }
    }
}