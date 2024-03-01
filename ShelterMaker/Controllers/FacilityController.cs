using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<FacilityController> _logger;

        public FacilityController(ShelterDbContext dbContext, ILogger<FacilityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facility>>> GetAllFacilitiesAsync()
        {
            try
            {
                var facilities = await _dbContext.Facilities.ToListAsync();
                if (facilities == null || facilities.Count == 0)
                {
                    _logger.LogWarning("No facilities found.");
                    return NotFound("No facilities found.");
                }

                return Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllFacilityAsync");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("filter-by-status")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetAllFacilitiesByStatusAsync([FromQuery] bool isActive)
        {
            try
            {
                var facilities = await _dbContext.Facilities
                    .Where(f => f.IsActive == isActive)
                    .ToListAsync();

                if (!facilities.Any())
                {
                    _logger.LogWarning($"No facilities found with IsActive status of {isActive}.");
                    return NotFound($"No facilities found with IsActive status of {isActive}.");
                }

                return Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching facilities");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetFacilityByIdAsync(int id)
        {
            try
            {
                var facility = await _dbContext.Facilities.FindAsync(id);

                if (facility == null)
                {
                    _logger.LogWarning($"Facility with ID {id} not found.");
                    return NotFound($"Facility with ID {id} not found.");
                }

                return Ok(facility);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching facility with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("active/{id}")]
        public async Task<ActionResult<IEnumerable<Facility>>> GetActiveFacilityByIdAsync(int id)
        {
            var facility = await _dbContext.Facilities
                .Where(f => f.FacilityId == id && f.IsActive == true)
                .FirstOrDefaultAsync();

            if (facility == null)
            {
                _logger.LogWarning($"Active facility with ID {id} not found.");
                return NotFound($"Active facility with ID {id} not found.");
            }

            return Ok(facility);
        }
        [HttpPost]
        public async Task<ActionResult> CreateFacilityAsync([FromBody] FacilityDto facilityDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for the FacilityCreateDto.");
                return BadRequest(ModelState);
            }

            var newFacility = new Facility
            {
                Name = facilityDto.Name,
                FacilityCode = facilityDto.FacilityCode,
                IsActive = true
            };

            try
            {
                await _dbContext.Facilities.AddAsync(newFacility);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Facility created with ID {newFacility.FacilityId}.");
                return CreatedAtAction(nameof(GetFacilityByIdAsync), new { id = newFacility.FacilityId }, newFacility);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new facility.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacilityAsync(int id, [FromBody] FacilityUpdateDto facilityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facility = await _dbContext.Facilities.FindAsync(id);
            if (facility == null)
            {
                _logger.LogWarning($"Facility with ID {id} not found.");
                return NotFound($"Facility with ID {id} not found.");
            }

            facility.Name = facilityDto.Name;
            facility.FacilityCode = facilityDto.FacilityCode;
            facility.IsActive = facilityDto.IsActive;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Facility with ID {id} updated.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating facility with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
