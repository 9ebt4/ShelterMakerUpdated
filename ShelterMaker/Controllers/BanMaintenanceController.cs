using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public BanMaintenanceController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BanMaintenance>>> GetAllBanMaintenanceAsync()
        {
            try
            {
                var banMaintenances = await _dbContext.BanMaintenances.ToListAsync();
                if (banMaintenances == null || banMaintenances.Count == 0)
                {
                    _logger.LogInformation("No ban maintenance records found.");
                    return NotFound("No ban maintenance records found.");
                }

                return Ok(banMaintenances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all ban maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
