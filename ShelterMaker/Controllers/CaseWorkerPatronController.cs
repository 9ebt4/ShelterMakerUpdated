using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;
using static ShelterMaker.DTOs.CaseWorkerPatronDTO;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseWorkerPatronController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<CaseWorkerPatronController> _logger;

        public CaseWorkerPatronController(ShelterDbContext dbContext, ILogger<CaseWorkerPatronController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet("caseworker/{caseWorkerId}")]
        public async Task<ActionResult<IEnumerable<Patron>>> GetCaseWorkerPatronsByCaseWorker(int caseWorkerId)
        {
            // Check if the caseworker exists
            var caseWorkerExists = await _dbContext.People.AnyAsync(cw => cw.PersonId == caseWorkerId);
            if (!caseWorkerExists)
            {
                return NotFound($"CaseWorker with ID {caseWorkerId} not found.");
            }

            // Retrieve all patrons associated with the given caseworker
            var patrons = await _dbContext.CaseWorkerPatrons
                .Where(cwp => cwp.CaseWorkerId == caseWorkerId)
                .Include(cwp => cwp.Patron) // Ensure navigation property is included
                .Select(cwp => cwp.Patron) // Select only the Patron part of the relationship
                .ToListAsync();

            if (patrons == null || patrons.Count == 0)
            {
                _logger.LogInformation($"No patrons found for CaseWorker with ID {caseWorkerId}.");
                return NotFound($"No patrons found for CaseWorker with ID {caseWorkerId}.");
            }

            return Ok(patrons);
        }

        [HttpGet("by-patron/{patronId}")]
        public async Task<ActionResult<IEnumerable<Person>>> GetCaseWorkerPatronsByPatronId(int patronId)
        {
            var associations = await _dbContext.CaseWorkerPatrons
                .Where(cwp => cwp.PatronId == patronId)
                .Include(cwp => cwp.CaseWorker)
                .Select(cwp => cwp.CaseWorker)
                .ToListAsync();

            if (!associations.Any())
            {
                return NotFound($"No associations found for Patron ID {patronId}.");
            }

            return Ok(associations);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCaseWorkerPatronRelationship([FromBody] CaseWorkerPatronCreateDto dto)
        {
            if (!await _dbContext.Patrons.AnyAsync(p => p.PatronId == dto.PatronId) ||
                !await _dbContext.People.AnyAsync(c => c.PersonId == dto.CaseWorkerId))
            {
                return NotFound("Patron or CaseWorker not found.");
            }

            var relationship = new CaseWorkerPatron
            {
                PatronId = dto.PatronId,
                CaseWorkerId = dto.CaseWorkerId
            };

            _dbContext.CaseWorkerPatrons.Add(relationship);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCaseWorkerPatronsByCaseWorker), new { caseWorkerId = dto.CaseWorkerId }, null);
        }

        [HttpPut("update-association")]
        public async Task<IActionResult> UpdateCaseWorkerPatronAssociation(int caseWorkerPatronId, [FromBody] UpdateAssociationDto dto)
        {
            var caseWorkerPatron = await _dbContext.CaseWorkerPatrons
                .FirstOrDefaultAsync(cwp => cwp.CaseWorkerPatronId == caseWorkerPatronId);

            if (caseWorkerPatron == null)
            {
                return NotFound($"Association with ID {caseWorkerPatronId} not found.");
            }

            // Assuming the DTO allows specifying either a new CaseWorkerId or a new PatronId
            if (dto.NewCaseWorkerId.HasValue)
            {
                caseWorkerPatron.CaseWorkerId = dto.NewCaseWorkerId.Value;
            }
            if (dto.NewPatronId.HasValue)
            {
                caseWorkerPatron.PatronId = dto.NewPatronId.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "An error occurred while updating the association.");
            }
        }

        [HttpDelete("{caseWorkerPatronId}")]
        public async Task<IActionResult> DeleteCaseWorkerPatronAssociation(int caseWorkerPatronId)
        {
            var caseWorkerPatron = await _dbContext.CaseWorkerPatrons
                .FindAsync(caseWorkerPatronId);

            if (caseWorkerPatron == null)
            {
                return NotFound($"Association with ID {caseWorkerPatronId} not found.");
            }

            _dbContext.CaseWorkerPatrons.Remove(caseWorkerPatron);

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent(); // Successfully deleted
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "An error occurred while deleting the association.");
            }
        }


    }

}
