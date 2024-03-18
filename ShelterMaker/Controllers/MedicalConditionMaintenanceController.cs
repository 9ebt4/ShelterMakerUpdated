using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalConditionMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<MedicalConditionMaintenanceController> _logger;

        public MedicalConditionMaintenanceController(ShelterDbContext dbContext, ILogger<MedicalConditionMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalConditionMaintenance>>> GetAllMedicalConditionMaintenanceAsync()
        {
            try
            {
                var medicalConditionMaintenances = await _dbContext.MedicalConditionMaintenances.ToListAsync();
                if (!medicalConditionMaintenances.Any())
                {
                    return NotFound("No medical condition maintenance categories found.");
                }

                return Ok(medicalConditionMaintenances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all medical condition maintenance categories.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalConditionMaintenance>> GetMedicalConditionMaintenanceByIdAsync(int id)
        {
            try
            {
                var medicalConditionMaintenance = await _dbContext.MedicalConditionMaintenances.FindAsync(id);
                if (medicalConditionMaintenance == null)
                {
                    return NotFound($"Medical condition maintenance category with ID {id} not found.");
                }

                return Ok(medicalConditionMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the medical condition maintenance category with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
