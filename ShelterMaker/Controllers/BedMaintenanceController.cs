using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<BedMaintenanceController> _logger;

        public BedMaintenanceController(ShelterDbContext dbContext, ILogger<BedMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BedMaintenance>>> GetAllBedMaintenanceAsync()
        {
            try
            {
                var bedMaintenances = await _dbContext.BedMaintenances.ToListAsync();
                if (bedMaintenances == null || bedMaintenances.Count == 0)
                {
                    _logger.LogInformation("No bed maintenance records found.");
                    return NotFound("No bed maintenance records found.");
                }

                return Ok(bedMaintenances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all bed maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
