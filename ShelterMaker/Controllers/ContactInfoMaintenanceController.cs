using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<ContactInfoMaintenanceController> _logger;

        public ContactInfoMaintenanceController(ShelterDbContext dbContext, ILogger<ContactInfoMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreateContactInfoMaintence([FromBody] ContactMaintenanceCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newContactMaintenance = new ContactMaintenance
                {
                    Type = dto.Type,
                };
                _dbContext.ContactMaintenances.Add(newContactMaintenance);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContactMaintenanceByIdAsync), new { id = newContactMaintenance.ContactMaintenanceId }, newContactMaintenance);
            }catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating new contact maintenance of type {dto.Type}.", dto.Type);

                // Return a generic 500 Internal Server Error status code to the client
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMaintenanceDetailDTO>>> GetAllContactMaintenanceAsync()
        {
            try
            {
                var contactMaintenanceList = await _dbContext.ContactMaintenances
                    .Select(cm=>new ContactMaintenanceDetailDTO
                    {
                        id =cm.ContactMaintenanceId,
                        Type = cm.Type,
                    })
                    .ToListAsync();
                if (contactMaintenanceList == null || !contactMaintenanceList.Any())
                {
                    _logger.LogInformation("No contact maintenance entries found.");
                    return NotFound("No contact maintenance entries found.");
                }

                return Ok(contactMaintenanceList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all contact maintenance entries.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactMaintenanceDetailDTO>> GetContactMaintenanceByIdAsync(int id)
        {
            try
            {
                var contactMaintenance = await _dbContext.ContactMaintenances.FindAsync(id);
                if (contactMaintenance == null)
                {
                    _logger.LogInformation("ContactMaintenance with ID {Id} not found.", id);
                    return NotFound($"ContactMaintenance with ID {id} not found.");
                }
                var contactMaintenanceDTO = new ContactMaintenanceDetailDTO 
                { 
                    id = contactMaintenance.ContactMaintenanceId, 
                    Type=contactMaintenance.Type 
                };
                return Ok(contactMaintenanceDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the contact maintenance entry with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
