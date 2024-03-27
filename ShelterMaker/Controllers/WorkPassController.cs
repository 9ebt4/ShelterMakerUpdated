using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkPassController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<WorkPassController> _logger;

        public WorkPassController(ShelterDbContext dbContext, ILogger<WorkPassController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkPass([FromBody] WorkPassCreateDto workPassCreateDto)
        {
            if (workPassCreateDto == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var newWorkPass = new WorkPass
                {
                    Needed = workPassCreateDto.Needed ?? false,
                    Confirmed = workPassCreateDto.Confirmed ?? false
                };

                _dbContext.WorkPasses.Add(newWorkPass);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Created new WorkPass with ID {WorkPassId}.", newWorkPass.WorkPassId);

                return CreatedAtAction(nameof(GetWorkPassById), new { id = newWorkPass.WorkPassId }, newWorkPass);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new WorkPass.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkPassById(int id)
        {
            var workPass = await _dbContext.WorkPasses
                .Select(wp => new WorkPassDto
                {
                    Id = wp.WorkPassId,
                    Needed = wp.Needed,
                    Confirmed = wp.Confirmed
                })
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workPass == null)
            {
                _logger.LogWarning("WorkPass with ID {id} not found.", id);
                return NotFound($"WorkPass with ID {id} not found.");
            }

            return Ok(workPass);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkPass(int id, [FromBody] WorkPassUpdateDto workPassUpdateDto)
        {
            if (workPassUpdateDto == null)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                var workPass = await _dbContext.WorkPasses.FindAsync(id);

                if (workPass == null)
                {
                    _logger.LogWarning("WorkPass with ID {WorkPassId} not found.", id);
                    return NotFound($"WorkPass with ID {id} not found.");
                }

                // Update only if values are provided
                if (workPassUpdateDto.Needed.HasValue)
                {
                    workPass.Needed = workPassUpdateDto.Needed.Value;
                }

                if (workPassUpdateDto.Confirmed.HasValue)
                {
                    workPass.Confirmed = workPassUpdateDto.Confirmed.Value;
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Updated WorkPass with ID {WorkPassId}.", id);

                return NoContent(); // 204 No Content is standard for successful PUT operations
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating WorkPass with ID {WorkPassId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
