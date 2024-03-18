using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteMaintenanceController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<NoteMaintenanceController> _logger;

        public NoteMaintenanceController(ShelterDbContext dbContext, ILogger<NoteMaintenanceController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteMaintenance>>> GetAllNoteMaintenanceAsync()
        {
            try
            {
                var noteMaintenance = await _dbContext.NoteMaintenances.ToListAsync();

                if (noteMaintenance == null || !noteMaintenance.Any())
                {
                    _logger.LogWarning("No note maintenance records found.");
                    return NotFound("No note maintenance records found.");
                }

                return Ok(noteMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all note maintenance records.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteMaintenance>> GetNoteMaintenanceByIdAsync(int id)
        {
            try
            {
                var noteMaintenance = await _dbContext.NoteMaintenances.FindAsync(id);

                if (noteMaintenance == null)
                {
                    _logger.LogWarning($"Note maintenance record with ID {id} not found.");
                    return NotFound($"Note maintenance record with ID {id} not found.");
                }

                return Ok(noteMaintenance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving note maintenance record with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
