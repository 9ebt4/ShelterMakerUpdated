using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociateMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public AssociateMaintenanceController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociateMaintenance>>> GetAllAssociateMaintenanceAsync()
        {
            try
            {
                var associateMaintenances = await _dbContext.AssociateMaintenances
                                                             .Include(am => am.Associates) // Optional: Include if you need associate details along with maintenance records
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
    }
}
