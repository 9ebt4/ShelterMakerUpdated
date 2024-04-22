using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociateMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AssociateMaintenanceController> _logger;

        public AssociateMaintenanceController(ShelterDbContext dbContext, ILogger<AssociateMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        [HttpPost]

        public async Task<ActionResult> CreateAssociateMaintenaceAsync([FromBody] AssociateMaintenanceCreateDTO CreateDto)
        {
            try
            {
                var associateMaintenance = new AssociateMaintenance
                {
                    Role = CreateDto.role
                };
                _dbContext.AssociateMaintenances.Add(associateMaintenance);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAssociateMaintenanceByIdAsync),new {id = associateMaintenance.AssociateMaintenanceId, associateMaintenance});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an associate maintenance object.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociateMaintenance>>> GetAllAssociateMaintenanceAsync()
        {
            try
            {
                var associateMaintenances = await _dbContext.AssociateMaintenances
                    .Select(am=>new AssociateMaintenanceDetailDTO
                    {
                        id = am.AssociateMaintenanceId,
                        role = am.Role
                    })
                    .ToListAsync();
                if (associateMaintenances == null || !associateMaintenances.Any())
                {
                    _logger.LogInformation("No associate maintenance records found.");
                    return NotFound("No associate maintenance records found.");
                }

                return Ok(associateMaintenances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all associate maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<IEnumerable<AssociateMaintenance>>> GetAssociateMaintenanceByIdAsync(int id)
        {
            try
            {
                var associateMaintenance = await _dbContext.AssociateMaintenances
                    .Where(am=> am.AssociateMaintenanceId == id)
                    .Select(am => new AssociateMaintenanceDetailDTO
                    {
                        id = am.AssociateMaintenanceId,
                        role = am.Role
                    })
                    .FirstOrDefaultAsync();
                if (associateMaintenance == null)
                {
                    _logger.LogInformation("No associate maintenance records found.");
                    return NotFound("No associate maintenance records found.");
                }

                return Ok(associateMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all associate maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
