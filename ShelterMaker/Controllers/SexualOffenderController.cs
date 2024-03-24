using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SexualOffenderController : ControllerBase
    {

        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<SexualOffenderController> _logger;

        public SexualOffenderController(ShelterDbContext dbContext, ILogger<SexualOffenderController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSexualOffender([FromBody] SexualOffenderCreateDto dto)
        {
            var newSexualOffender = new SexualOffender
            {
                Completed = dto.Completed,
                IsOffender = dto.IsOffender
            };

            try
            {
                _dbContext.SexualOffenders.Add(newSexualOffender);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Sexual offender created with ID {newSexualOffender.SexualOffenderId}.");
                return CreatedAtAction(nameof(GetSexualOffenderById), new { id = newSexualOffender.SexualOffenderId }, newSexualOffender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new sexual offender.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSexualOffenderById(int id)
        {
            try
            {
                var sexualOffender = await _dbContext.SexualOffenders
                    .Where(so => so.SexualOffenderId == id)
                    .Select(so => new SexualOffenderDto
                    {
                        SexualOffenderId = so.SexualOffenderId,
                        Completed = so.Completed,
                        IsOffender = so.IsOffender
                    })
                    .FirstOrDefaultAsync();

                if (sexualOffender == null)
                {
                    _logger.LogWarning("Sexual offender with ID {Id} not found.", id);
                    return NotFound($"Sexual offender with ID {id} not found.");
                }

                return Ok(sexualOffender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching sexual offender with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSexualOffender(int id, [FromBody] SexualOffenderUpdateDto dto)
        {
            var sexualOffender = await _dbContext.SexualOffenders.FindAsync(id);
            if (sexualOffender == null)
            {
                _logger.LogWarning($"Sexual offender with ID {id} not found.");
                return NotFound($"Sexual offender with ID {id} not found.");
            }

            if (dto.Completed.HasValue)
            {
                sexualOffender.Completed = dto.Completed.Value;
            }
            if (dto.IsOffender.HasValue)
            {
                sexualOffender.IsOffender = dto.IsOffender.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Sexual offender with ID {id} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sexual offender with ID {id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
