using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociateController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public AssociateController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Associate>>> GetAllAssociatesAsync()
        {
            try
            {
                var associates = await _dbContext.Associates.ToListAsync();

                if (!associates.Any())
                {
                    _logger.LogInformation("No associates found.");
                    return NotFound("No associates found.");
                }

                return Ok(associates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all associates.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("by-facility/{facilityId}")]
        public async Task<ActionResult<IEnumerable<Associate>>> GetAssociatesByFacilityIdAsync(int facilityId)
        {
            try
            {
                var associates = await _dbContext.Associates
                    .Where(a => a.FacilityId == facilityId)
                    .Include(a => a.AssociateMaintenance) // Consider including related entities as needed
                    .Include(a => a.GoogleUser)
                    .ThenInclude(a => a.Person)// Include other entities as necessary
                    .ToListAsync();

                if (!associates.Any())
                {
                    _logger.LogInformation($"No associates found for Facility ID {facilityId}.");
                    return NotFound($"No associates found for Facility ID {facilityId}.");
                }

                return Ok(associates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching associates for Facility ID {facilityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Associate>> GetAssociateByIdAsync(int id)
        {
            try
            {
                var associate = await _dbContext.Associates
                    .FirstOrDefaultAsync(a => a.AssociateId == id);

                if (associate == null)
                {
                    _logger.LogInformation($"Associate with ID {id} not found.");
                    return NotFound($"Associate with ID {id} not found.");
                }

                return Ok(associate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the associate with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Associate>> CreateAssociate([FromBody] AssociateCreateDto associateDto)
        {
            var newAssociate = new Associate
            {
                IsActive = true,
                FacilityId = associateDto.FacilityId,
                AssociateMaintenanceId = associateDto.AssociateMaintenanceId,
                GoogleUserId = associateDto.GoogleUserId,
                // Map other properties as necessary
            };

            _dbContext.Associates.Add(newAssociate);
            try
            {
                await _dbContext.SaveChangesAsync();

                // Optionally include related entities in the response
                // by explicitly loading them here, if needed.

                return CreatedAtAction(nameof(GetAssociateByIdAsync), new { id = newAssociate.AssociateId }, newAssociate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new associate.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //use to activate and deactivate associates. 
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateAssociateStatus(int id, [FromBody] AssociateStatusUpdateDto statusUpdateDto)
        {
            var associate = await _dbContext.Associates.FindAsync(id);
            if (associate == null)
            {
                _logger.LogInformation($"Associate with ID {id} not found.");
                return NotFound($"Associate with ID {id} not found.");
            }

            associate.IsActive = statusUpdateDto.IsActive;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Associate with ID {id} active status updated to {statusUpdateDto.IsActive}.");
                return NoContent(); // 204 No Content is typically returned to indicate successful update without returning data.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the active status of associate with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        //used to change employee from volunteer to employee or the reverse. 
        [HttpPut("{id}/maintenance")]
        public async Task<IActionResult> UpdateAssociateMaintenance(int id, [FromBody] AssociateMaintenanceUpdateDto maintenanceUpdateDto)
        {
            //check if associate exist
            var associate = await _dbContext.Associates.FindAsync(id);
            if (associate == null)
            {
                _logger.LogInformation($"Associate with ID {id} not found.");
                return NotFound($"Associate with ID {id} not found.");
            }

            //check if associate level exists
            var maintenanceExists = await _dbContext.AssociateMaintenances.AnyAsync(m => m.AssociateMaintenanceId == maintenanceUpdateDto.AssociateMaintenanceId);
            if (!maintenanceExists)
            {
                _logger.LogInformation($"AssociateMaintenance with ID {maintenanceUpdateDto.AssociateMaintenanceId} not found.");
                return NotFound($"AssociateMaintenance with ID {maintenanceUpdateDto.AssociateMaintenanceId} not found.");
            }

            associate.AssociateMaintenanceId = maintenanceUpdateDto.AssociateMaintenanceId;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Associate with ID {id} maintenance association updated to {maintenanceUpdateDto.AssociateMaintenanceId}.");
                return NoContent(); // 204 No Content is typically returned to indicate successful update without returning data.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the maintenance association of associate with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
