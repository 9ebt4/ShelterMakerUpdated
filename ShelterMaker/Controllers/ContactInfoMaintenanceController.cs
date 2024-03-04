using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public ContactInfoMaintenanceController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactMaintenance>>> GetAllContactMaintenanceAsync()
        {
            try
            {
                var contactMaintenanceList = await _dbContext.ContactMaintenances.ToListAsync();
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
        public async Task<ActionResult<ContactMaintenance>> GetContactMaintenanceByIdAsync(int id)
        {
            try
            {
                var contactMaintenance = await _dbContext.ContactMaintenances.FindAsync(id);
                if (contactMaintenance == null)
                {
                    _logger.LogInformation("ContactMaintenance with ID {Id} not found.", id);
                    return NotFound($"ContactMaintenance with ID {id} not found.");
                }

                return Ok(contactMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the contact maintenance entry with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
