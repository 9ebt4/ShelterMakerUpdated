using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<ChecklistController> _logger;

        public ChecklistController(ShelterDbContext dbContext, ILogger<ChecklistController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet("by-facility/{facilityId}")]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetAllChecklistsByFacility(int facilityId)
        {
            try
            {
                var checklists = await _dbContext.Checklists
                    .Where(c => c.FacilityId == facilityId)
                    .ToListAsync();

                if (!checklists.Any())
                {
                    return NotFound($"No checklists found for Facility with ID {facilityId}.");
                }

                return Ok(checklists);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Checklist>> GetChecklistById(int id)
        {
            try
            {
                var checklist = await _dbContext.Checklists
                    .Where(c => c.ChecklistId == id)
                    .Include(c => c.Items) // Include associated items
                    .FirstOrDefaultAsync();

                if (checklist == null)
                {
                    return NotFound($"Checklist with ID {id} not found.");
                }

                return Ok(checklist);
            }
            catch (Exception ex)
            {
                // Log the exception here. Replace with your logging mechanism.
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("active-now")]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetActiveChecklists()
        {
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            try
            {
                var activeChecklists = await _dbContext.Checklists
                    .Where(c => c.StartTime <= currentTime && (c.EndTime == null || c.EndTime >= currentTime))
                    .Include(c => c.Items) // Include associated items if relevant
                    .ToListAsync();

                if (!activeChecklists.Any())
                {
                    return NotFound("No active checklists found at the current time.");
                }

                return Ok(activeChecklists);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Checklist>> CreateChecklist(ChecklistCreateDto checklistDto)
        {
            var newChecklist = new Checklist
            {
                FacilityId = checklistDto.FacilityId,
                StartTime = checklistDto.StartTime,
                EndTime = checklistDto.EndTime,
                Options = checklistDto.Options,
            };

            try
            {
                _dbContext.Checklists.Add(newChecklist);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetChecklistById), new { id = newChecklist.ChecklistId }, newChecklist);
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while creating the checklist.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChecklist(int id, [FromBody] ChecklistUpdateDto checklistDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var checklist = await _dbContext.Checklists.FindAsync(id);
            if (checklist == null)
            {
                return NotFound($"No checklist found with the ID {id}.");
            }

            // Update properties if they are not null in the DTO
            checklist.StartTime = checklistDto.StartTime ?? checklist.StartTime;
            checklist.EndTime = checklistDto.EndTime ?? checklist.EndTime;
            checklist.Options = checklistDto.Options ?? checklist.Options;

            try
            {
                _dbContext.Checklists.Update(checklist);
                await _dbContext.SaveChangesAsync();
                return NoContent(); // 204 No Content is typically returned when an update is successful
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while updating the checklist.");
            }
        }

    }
}
