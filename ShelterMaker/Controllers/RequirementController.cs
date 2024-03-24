using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequirementController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<RequirementController> _logger;

        public RequirementController(ShelterDbContext dbContext, ILogger<RequirementController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult<Requirement>> CreateRequirement([FromBody] RequirementsCreateDto requirementDto)
        {
            try
            {
                var newRequirement = new Requirement
                {
                    Completed = requirementDto.Completed ?? false,
                    Confirmed = requirementDto.Confirmed ?? false
                };

                _dbContext.Requirements.Add(newRequirement);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRequirementById), new { id = newRequirement.RequirementsId }, newRequirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new requirement.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequirementsDto>> GetRequirementById(int id)
        {
            try
            {
                var requirement = await _dbContext.Requirements
                    .Where(r => r.RequirementsId == id)
                    .Select(r => new RequirementsDto
                    {
                        Id = r.RequirementsId,
                        Completed = r.Completed,
                        Confirmed = r.Confirmed
                    })
                    .FirstOrDefaultAsync();

                if (requirement == null)
                {
                    return NotFound($"Requirement with ID {id} not found.");
                }

                return Ok(requirement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the requirement with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{requirementsId}")]
        public async Task<IActionResult> UpdateRequirements(int requirementsId, [FromBody] RequirementUpdateDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var requirement = await _dbContext.Requirements.FindAsync(requirementsId);
            if (requirement == null)
            {
                _logger.LogWarning($"Requirement with ID {requirementsId} not found.");
                return NotFound($"Requirement with ID {requirementsId} not found.");
            }

            if (updateDto.Completed.HasValue)
            {
                requirement.Completed = updateDto.Completed.Value;
            }

            if (updateDto.Confirmed.HasValue)
            {
                requirement.Confirmed = updateDto.Confirmed.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Requirement with ID {requirementsId} updated.");
                return NoContent(); // 204 No Content is a typical response for a successful PUT request.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Requirement with ID {RequirementsId}.", requirementsId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
