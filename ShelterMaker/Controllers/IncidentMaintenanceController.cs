using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<IncidentMaintenanceController> _logger;

        public IncidentMaintenanceController(ShelterDbContext dbContext, ILogger<IncidentMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentMaintenance>>> GetAllIncidentMaintenanceAsync()
        {
            try
            {
                var incidentMaintenances = await _dbContext.IncidentMaintenances.ToListAsync();

                if (incidentMaintenances == null || !incidentMaintenances.Any())
                {
                    _logger.LogWarning("No incident maintenance records found.");
                    return NotFound("No incident maintenance records found.");
                }

                return Ok(incidentMaintenances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all incident maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentMaintenance>> GetIncidentMaintenanceByIdAsync(int id)
        {
            try
            {
                var incidentMaintenance = await _dbContext.IncidentMaintenances.FindAsync(id);

                if (incidentMaintenance == null)
                {
                    _logger.LogWarning($"Incident maintenance with ID {id} not found.");
                    return NotFound($"Incident maintenance with ID {id} not found.");
                }

                return Ok(incidentMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching incident maintenance with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
