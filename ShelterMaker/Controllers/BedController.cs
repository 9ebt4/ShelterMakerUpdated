using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<BedController> _logger;

        public BedController(ShelterDbContext dbContext, ILogger<BedController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        //GET /api/Bed/by-facility/{facilityId}
        [HttpGet("by-facility/{facilityId}")]
        public async Task<ActionResult<IEnumerable<Bed>>> GetBedsByFacilityAsync(int facilityId)
        {
            try
            {
                var beds = await _dbContext.Beds
                    .Where(b => b.FacilityId == facilityId)
                    .ToListAsync();

                if (!beds.Any())
                {
                    _logger.LogWarning($"No beds found for facility with ID {facilityId}.");
                    return NotFound($"No beds found for facility with ID {facilityId}.");
                }

                return Ok(beds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching beds for facility with ID {facilityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bed>> GetBedByIdAsync(int id)
        {
            try
            {
                var bed = await _dbContext.Beds
                    .Include(b => b.Amenities)
                    .Include(b => b.BedMaintenances)
                    .FirstOrDefaultAsync(b => b.BedId == id);

                if (bed == null)
                {
                    _logger.LogWarning($"Bed with ID {id} not found.");
                    return NotFound($"Bed with ID {id} not found.");
                }

                return Ok(bed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching bed with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Bed>> CreateBed([FromBody] BedCreateDto bedDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facilityExists = await _dbContext.Facilities.AnyAsync(f => f.FacilityId == bedDto.FacilityId);
            if (!facilityExists)
            {
                return BadRequest($"Facility with ID {bedDto.FacilityId} does not exist.");
            }

            var bedCount = await _dbContext.Beds.CountAsync(b => b.FacilityId == bedDto.FacilityId);
            var newBedName = $"Bed {bedCount + 1}";

            var newBed = new Bed
            {
                Name = newBedName,
                FacilityId = bedDto.FacilityId
            };

            var newBedMaintenance = new BedMaintenance
            {
                Category = bedDto.MaintenanceType,
                Bed = newBed // Associate with the new bed
            };

            var newAmenity = new Amenity
            {
                Category = bedDto.AmenityType,
                Bed = newBed // Associate with the new bed
            };

            try
            {
                await _dbContext.Beds.AddAsync(newBed);
                await _dbContext.BedMaintenances.AddAsync(newBedMaintenance);
                await _dbContext.Amenities.AddAsync(newAmenity);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBedByIdAsync), new { id = newBed.BedId }, newBed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new bed.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBedAsync(int id)
        {
            var bed = await _dbContext.Beds.FindAsync(id);
            if (bed == null)
            {
                _logger.LogInformation($"Bed with ID {id} not found.");
                return NotFound($"Bed with ID {id} not found.");
            }

            _dbContext.Beds.Remove(bed);
            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Bed with ID {id} deleted successfully.");
                return NoContent(); // 204 No Content is typically returned to indicate successful deletion.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while trying to delete the bed with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
