using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<BanController> _logger;

        public BanController(ShelterDbContext dbContext, ILogger<BanController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ban>> GetBanById(int id)
        {
            var ban = await _dbContext.Bans
                .Include(b => b.BanMaintenance) // Optionally include related BanMaintenance data
                .FirstOrDefaultAsync(b => b.BanId == id);

            if (ban == null)
            {
                _logger.LogInformation($"Ban with ID {id} not found.");
                return NotFound($"Ban with ID {id} not found.");
            }

            return Ok(ban);
        }

        [HttpPost]
        public async Task<ActionResult<Ban>> CreateBan([FromBody] BanCreateDto banDto)
        {
            // ASP.NET Core's model binding automatically validates the DTO against the [Required] annotations.
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, a BadRequest response is returned with the validation errors.
                return BadRequest(ModelState);
            }

            // Check for the existence of the IncidentReport and BanMaintenance entities to ensure data integrity.
            var incidentExists = await _dbContext.Incidents.AnyAsync(i => i.IncidentId == banDto.IncidentReportId);
            if (!incidentExists)
            {
                return NotFound($"Incident with ID {banDto.IncidentReportId} not found.");
            }

            var banMaintenanceExists = await _dbContext.BanMaintenances.AnyAsync(bm => bm.BanMaintenanceId == banDto.BanMaintenanceId);
            if (!banMaintenanceExists)
            {
                return NotFound($"BanMaintenance with ID {banDto.BanMaintenanceId} not found.");
            }

            // Create the new Ban entity.
            var newBan = new Ban
            {
                StartDate = banDto.StartDate,
                EndDate = banDto.EndDate,
                IsActive = true, // IsActive is always true upon creation.
                IncidentReportId = banDto.IncidentReportId,
                BanMaintenanceId = banDto.BanMaintenanceId,
                // PatronBans will be populated in the loop below.
            };

            // Associate the Ban with the provided PatronIds.
            foreach (var patronId in banDto.PatronIds)
            {
                newBan.PatronBans.Add(new PatronBan { PatronId = patronId });
            }

            _dbContext.Bans.Add(newBan);
            try
            {
                await _dbContext.SaveChangesAsync();
                // Return a 201 Created response with the location header pointing to the newly created Ban.
                return CreatedAtAction(nameof(GetBanById), new { id = newBan.BanId }, newBan);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the new ban.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBan(int id, [FromBody] BanUpdateDto updateDto)
        {
            var ban = await _dbContext.Bans.FindAsync(id);
            if (ban == null)
            {
                return NotFound($"Ban with ID {id} not found.");
            }

            // Check if there's an update for IncidentReportId and if the new IncidentReport exists
            if (updateDto.IncidentReportId.HasValue && !await _dbContext.Incidents.AnyAsync(i => i.IncidentId == updateDto.IncidentReportId.Value))
            {
                return NotFound($"Incident with ID {updateDto.IncidentReportId.Value} not found.");
            }

            // Check if there's an update for BanMaintenanceId and if the new BanMaintenance exists
            if (updateDto.BanMaintenanceId.HasValue && !await _dbContext.BanMaintenances.AnyAsync(bm => bm.BanMaintenanceId == updateDto.BanMaintenanceId.Value))
            {
                return NotFound($"BanMaintenance with ID {updateDto.BanMaintenanceId.Value} not found.");
            }

            // Update fields if provided in the DTO
            if (updateDto.IncidentReportId.HasValue)
            {
                ban.IncidentReportId = updateDto.IncidentReportId.Value;
            }
            if (updateDto.IsActive.HasValue)
            {
                ban.IsActive = updateDto.IsActive.Value;
            }
            if (updateDto.BanMaintenanceId.HasValue)
            {
                ban.BanMaintenanceId = updateDto.BanMaintenanceId.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent(); // 204 No Content is appropriate for a successful PUT
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating ban with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
