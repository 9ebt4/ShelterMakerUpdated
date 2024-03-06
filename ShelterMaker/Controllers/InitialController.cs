using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitialController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<InitialController> _logger;

        public InitialController(ShelterDbContext dbContext, ILogger<InitialController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInitialAsync([FromBody] InitialUpdateDto initialDto)
        {
            try
            {
                var newInitial = new Initial
                {
                    Locations = initialDto.Locations ?? false,
                    Medical = initialDto.Medical ?? false,
                    Covid = initialDto.Covid ?? false,
                    InitialAgreement = initialDto.InitialAgreement ?? false
                };

                _dbContext.Initials.Add(newInitial);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Initial created with ID {newInitial.InitialId}.");

                return CreatedAtAction(nameof(GetInitialByIdAsync), new { id = newInitial.InitialId }, newInitial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new initial.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Initial>> GetInitialByIdAsync(int id)
        {
            try
            {
                var initial = await _dbContext.Initials.FindAsync(id);
                if (initial == null)
                {
                    _logger.LogWarning($"Initial with ID {id} not found.");
                    return NotFound($"Initial with ID {id} not found.");
                }

                return Ok(initial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching initial with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInitialAsync(int id, [FromBody] InitialUpdateDto initialUpdateDto)
        {
            try
            {
                var initial = await _dbContext.Initials.FindAsync(id);
                if (initial == null)
                {
                    _logger.LogWarning($"Initial with ID {id} not found.");
                    return NotFound($"Initial with ID {id} not found.");
                }

                // Update only the provided fields
                initial.Locations = initialUpdateDto.Locations ?? initial.Locations;
                initial.Medical = initialUpdateDto.Medical ?? initial.Medical;
                initial.Covid = initialUpdateDto.Covid ?? initial.Covid;
                initial.InitialAgreement = initialUpdateDto.InitialAgreement ?? initial.InitialAgreement;

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Initial with ID {id} updated.");

                return NoContent(); // 204 No Content is typically returned when an update is successful
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating initial with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
