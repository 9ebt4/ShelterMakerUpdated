using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalConditionController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<MedicalConditionController> _logger;

        public MedicalConditionController(ShelterDbContext dbContext, ILogger<MedicalConditionController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedicalCondition([FromBody] MedicalConditionCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newMedicalCondition = new MedicalCondition
            {
                PatronId = dto.PatronId,
                Details = dto.Details,
                MedicalConditionMaintenanceId = dto.MedicalConditionMaintenanceId
            };

            try
            {
                await _dbContext.MedicalConditions.AddAsync(newMedicalCondition);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMedicalConditionByIdAsync), new { id = newMedicalCondition.MedicalConditionId }, newMedicalCondition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating medical condition.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalConditionDetailsDto>> GetMedicalConditionByIdAsync(int id)
        {
            try
            {
                var medicalCondition = await _dbContext.MedicalConditions
                    .Include(mc => mc.MedicalConditionMaintenance)
                    .Where(mc => mc.MedicalConditionId == id)
                    .Select(mc => new MedicalConditionDetailsDto
                    {
                        MedicalConditionId = mc.MedicalConditionId,
                        PatronId = mc.PatronId??0,
                        Details = mc.Details,
                        MedicalConditionMaintenanceId = mc.MedicalConditionMaintenanceId??0,
                        MedicalConditionMaintenanceCategory = mc.MedicalConditionMaintenance.Category ?? string.Empty
                    })
                    .FirstOrDefaultAsync();

                if (medicalCondition == null)
                {
                    return NotFound($"Medical condition with ID {id} not found.");
                }

                return Ok(medicalCondition);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching medical condition with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("by-patron/{patronId}")]
        public async Task<ActionResult<IEnumerable<MedicalConditionDetailsDto>>> GetAllByPatronIdAsync(int patronId)
        {
            try
            {
                var medicalConditions = await _dbContext.MedicalConditions
                    .Where(mc => mc.PatronId == patronId)
                    .Include(mc => mc.MedicalConditionMaintenance)
                    .Select(mc => new MedicalConditionDetailsDto
                    {
                        MedicalConditionId = mc.MedicalConditionId,
                        Details = mc.Details,
                        PatronId = mc.PatronId ?? 0, // Assuming the DTO and model have been adjusted to handle nullable types correctly.
                        MedicalConditionMaintenanceId = mc.MedicalConditionMaintenanceId ?? 0,
                        MedicalConditionMaintenanceCategory = mc.MedicalConditionMaintenance.Category ?? string.Empty
                    })
                    .ToListAsync();

                if (!medicalConditions.Any())
                {
                    return NotFound($"No medical conditions found for patron ID {patronId}.");
                }

                return Ok(medicalConditions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching medical conditions for patron ID {patronId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicalConditionAsync(int id, [FromBody] MedicalConditionUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalCondition = await _dbContext.MedicalConditions.FindAsync(id);
            if (medicalCondition == null)
            {
                return NotFound($"Medical condition with ID {id} not found.");
            }

            // Update the fields if they are provided in the DTO
            if (updateDto.MedicalConditionMaintenanceId.HasValue)
            {
                medicalCondition.MedicalConditionMaintenanceId = updateDto.MedicalConditionMaintenanceId.Value;
            }
            if (updateDto.Details != null)
            {
                medicalCondition.Details = updateDto.Details;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent(); // 204 No Content is typically returned to indicate successful update without sending data back
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating medical condition with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalConditionAsync(int id)
        {
            var medicalCondition = await _dbContext.MedicalConditions.FindAsync(id);
            if (medicalCondition == null)
            {
                return NotFound($"Medical condition with ID {id} not found.");
            }

            try
            {
                _dbContext.MedicalConditions.Remove(medicalCondition);
                await _dbContext.SaveChangesAsync();
                return NoContent(); // Successfully deleted
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while trying to delete the medical condition with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
