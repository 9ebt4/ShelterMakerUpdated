using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronInfoReleaseController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<PatronInfoReleaseController> _logger;

        public PatronInfoReleaseController(ShelterDbContext dbContext, ILogger<PatronInfoReleaseController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatronInfoReleaseWithPersonAndContact([FromBody] PatronInfoReleaseCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create Person
                var person = new Person
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDay = dto.Age,
                };
                _dbContext.People.Add(person);
                await _dbContext.SaveChangesAsync();

                // Create ContactInfo for this Person
                var contactInfo = new ContactInfo
                {
                    PersonId = person.PersonId,
                    Details = dto.ContactDetail,
                    ContactMaintenanceId = dto.ContactMaintenanceId
                };
                _dbContext.ContactInfos.Add(contactInfo);
                await _dbContext.SaveChangesAsync();

                // Finally, create the PatronInfoRelease
                var patronInfoRelease = new PatronInfoRelease
                {
                    PersonId = person.PersonId,
                    RelationshipId = dto.RelationshipId,
                    PatronId = dto.PatronId
                };
                _dbContext.PatronInfoReleases.Add(patronInfoRelease);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                // Assuming you have a Get method to retrieve PatronInfoRelease by ID
                return CreatedAtAction(nameof(GetPatronInfoReleaseById), new { id = patronInfoRelease.PatronInfoReleaseId }, patronInfoRelease);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception
                return StatusCode(500, "An error occurred while creating the patron info release and associated entities.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatronInfoReleaseDetailDto>> GetPatronInfoReleaseById(int id)
        {
            var patronInfoRelease = await _dbContext.PatronInfoReleases
                .Where(pir => pir.PatronInfoReleaseId == id)
                .Select(pir => new PatronInfoReleaseDetailDto
                {
                    PatronInfoReleaseId = pir.PatronInfoReleaseId,
                    PersonID= pir.PersonId,
                    FirstName = pir.Person.FirstName,
                    LastName = pir.Person.LastName,
                    Birthday = pir.Person.BirthDay.Value, // Assuming Birthday is no longer nullable
                    Relationship = pir.Relationship.Relationship1,
                    ContactInfos = pir.Person.ContactInfos.Select(ci => new ContactInfoDto
                    {
                        Id = ci.ContactInfoId,
                        Detail = ci.Details,
                        Type = ci.ContactMaintenance.Type
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (patronInfoRelease == null)
            {
                return NotFound($"PatronInfoRelease with ID {id} not found.");
            }

            return Ok(patronInfoRelease);
        }


        [HttpGet("by-patron/{patronId}")]
        public async Task<ActionResult<List<PatronInfoReleaseDetailDto>>> GetAllPatronInfoReleasesByPatronId(int patronId)
        {
            try
            {
                var patronInfoReleases = await _dbContext.PatronInfoReleases
                    .Where(pir => pir.PatronId == patronId)
                    .Select(pir => new PatronInfoReleaseDetailDto
                    {
                        FirstName = pir.Person.FirstName,
                        LastName = pir.Person.LastName,
                        Birthday = pir.Person.BirthDay.Value, // Assuming Birthday is no longer nullable
                        Relationship = pir.Relationship.Relationship1,
                        ContactInfos = pir.Person.ContactInfos.Select(ci => new ContactInfoDto
                        {
                            Detail = ci.Details,
                            Type = ci.ContactMaintenance.Type
                        }).ToList()
                    })
                    .ToListAsync();

                if (!patronInfoReleases.Any())
                {

                    return NotFound($"No info releases found for Patron ID {patronId}.");
                }

                return Ok(patronInfoReleases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching patron info release with patronID {patronId}.", patronId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{patronInfoReleaseId}")]
        public async Task<IActionResult> DeletePatronInfoRelease(int patronInfoReleaseId)
        {
            var patronInfoRelease = await _dbContext.PatronInfoReleases
                .Include(pir => pir.Person)
                    .ThenInclude(p => p.ContactInfos)
                .FirstOrDefaultAsync(pir => pir.PatronInfoReleaseId == patronInfoReleaseId);

            if (patronInfoRelease == null)
            {
                return NotFound($"Patron info release with ID {patronInfoReleaseId} not found.");
            }

            try
            {
                // Assuming ContactInfos is a collection that will be cascade deleted with Person
                _dbContext.PatronInfoReleases.Remove(patronInfoRelease);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while deleting patron info release with ID {PatronInfoReleaseId}.", patronInfoReleaseId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
