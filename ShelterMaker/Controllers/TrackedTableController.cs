using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackedTableController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<TrackedTableController> _logger;

        public TrackedTableController(ShelterDbContext dbContext, ILogger<TrackedTableController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrackedTable([FromBody] TrackedTableCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.TableName))
            {
                _logger.LogWarning("Attempt to create a tracked table with invalid data.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var newTrackedTable = new TrackedTable
                {
                    TableName = dto.TableName
                };

                _dbContext.TrackedTables.Add(newTrackedTable);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Tracked table {dto.TableName} created successfully.");

                // Assuming you have a GetTrackedTableById endpoint
                return CreatedAtAction(nameof(GetTrackedTableById), new { id = newTrackedTable.TrackedTableId }, newTrackedTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new tracked table.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackedTableDetailDto>> GetTrackedTableById(int id)
        {
            try
            {
                var trackedTable = await _dbContext.TrackedTables
                    .Select(tt => new TrackedTableDetailDto
                    {
                        TrackedTableId = tt.TrackedTableId,
                        TableName = tt.TableName
                    })
                    .FirstOrDefaultAsync(tt => tt.TrackedTableId == id );

                if (trackedTable == null)
                {
                    _logger.LogWarning($"Tracked table with ID {id} not found.");
                    return NotFound($"Tracked table with ID {id} not found.");
                }

                return Ok(trackedTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tracked table with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackedTableDetailDto>>> GetAllTrackedTables()
        {
            try
            {
                var trackedTables = await _dbContext.TrackedTables
                    .Select(t => new TrackedTableDetailDto
                    {
                        TrackedTableId = t.TrackedTableId,
                        TableName = t.TableName
                    })
                    .ToListAsync();

                if (!trackedTables.Any())
                {
                    _logger.LogWarning("No tracked tables found.");
                    return NotFound("No tracked tables found.");
                }

                return Ok(trackedTables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all tracked tables.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{trackedTableId}")]
        public async Task<IActionResult> UpdateTrackedTable(int trackedTableId, [FromBody] TrackedTableUpdateDto trackedTableUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trackedTable = await _dbContext.TrackedTables.FindAsync(trackedTableId);
            if (trackedTable == null)
            {
                _logger.LogWarning($"Tracked table with ID {trackedTableId} not found.");
                return NotFound($"Tracked table with ID {trackedTableId} not found.");
            }

            try
            {
                trackedTable.TableName = trackedTableUpdateDto.TableName ?? trackedTable.TableName;
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Tracked table with ID {trackedTableId} updated successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the tracked table with ID {trackedTableId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{trackedTableId}")]
        public async Task<IActionResult> DeleteTrackedTable(int trackedTableId)
        {
            var trackedTable = await _dbContext.TrackedTables.FindAsync(trackedTableId);
            if (trackedTable == null)
            {
                _logger.LogWarning($"Tracked table with ID {trackedTableId} not found.");
                return NotFound($"Tracked table with ID {trackedTableId} not found.");
            }

            try
            {
                _dbContext.TrackedTables.Remove(trackedTable);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Tracked table with ID {trackedTableId} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the tracked table with ID {trackedTableId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
