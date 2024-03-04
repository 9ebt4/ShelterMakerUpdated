using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmergencyContactController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public EmergencyContactController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmergencyContact([FromBody] EmergencyContactCreateDto dto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Create and save the Person
                var person = new Person { FirstName = dto.FirstName, LastName = dto.LastName };
                _dbContext.People.Add(person);
                await _dbContext.SaveChangesAsync();

                // Create and save the ContactInfo linked to the Person
                var contactInfo = new ContactInfo
                {
                    PersonId = person.PersonId,
                    Details = dto.ContactDetail,
                    ContactMaintenanceId = dto.ContactMaintenanceId
                };
                _dbContext.ContactInfos.Add(contactInfo);
                await _dbContext.SaveChangesAsync();

                // Create and save the EmergencyContact
                var emergencyContact = new EmergencyContact
                {
                    PersonId = person.PersonId,
                    RelationshipId = dto.RelationshipId,
                    AssociateId = dto.AssociateId,
                    PatronId = dto.PatronId
                };
                _dbContext.EmergencyContacts.Add(emergencyContact);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetEmergencyContactById), new { id = emergencyContact.EmergencyContactId }, emergencyContact);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating an emergency contact.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmergencyContactDetailsDto>> GetEmergencyContactById(int id)
        {
            try
            {
                var emergencyContact = await _dbContext.EmergencyContacts
                    .Include(ec => ec.Person)
                        .ThenInclude(p => p.ContactInfos)
                            .ThenInclude(ci => ci.ContactMaintenance)
                    .Include(ec => ec.Relationship)
                    .Where(ec => ec.EmergencyContactId == id)
                    .Select(ec => new EmergencyContactDetailsDto
                    {
                        EmergencyContactId = ec.EmergencyContactId,
                        FirstName = ec.Person.FirstName,
                        LastName = ec.Person.LastName,
                        Relationship = ec.Relationship.Relationship1,
                        ContactDetails = ec.Person.ContactInfos.Select(ci => new ContactInfoDto
                        {
                            Detail = ci.Details,
                            Type = ci.ContactMaintenance.Type
                        }).ToList(),
                        AssociateId = ec.AssociateId,
                        PatronId = ec.PatronId
                    })
                    .FirstOrDefaultAsync();

                if (emergencyContact == null)
                {
                    _logger.LogInformation("EmergencyContact with ID {Id} not found.", id);
                    return NotFound($"EmergencyContact with ID {id} not found.");
                }

                return Ok(emergencyContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving emergency contact with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("associate/{associateId}")]
        public async Task<ActionResult<IEnumerable<EmergencyContactDetailsDto>>> GetEmergencyContactsByAssociateId(int associateId)
        {
            try
            {
                var emergencyContacts = await _dbContext.EmergencyContacts
                    .Where(ec => ec.AssociateId == associateId)
                    .Include(ec => ec.Person)
                        .ThenInclude(p => p.ContactInfos)
                            .ThenInclude(ci => ci.ContactMaintenance)
                    .Include(ec => ec.Relationship)
                    .Select(ec => new EmergencyContactDetailsDto
                    {
                        EmergencyContactId = ec.EmergencyContactId,
                        FirstName = ec.Person.FirstName,
                        LastName = ec.Person.LastName,
                        Relationship = ec.Relationship.Relationship1,
                        ContactDetails = ec.Person.ContactInfos.Select(ci => new ContactInfoDto
                        {
                            Detail = ci.Details,
                            Type = ci.ContactMaintenance.Type
                        }).ToList(),
                        AssociateId = ec.AssociateId,
                        PatronId = ec.PatronId
                    })
                    .ToListAsync();

                if (!emergencyContacts.Any())
                {
                    _logger.LogInformation("No emergency contacts found for associate with ID {AssociateId}.", associateId);
                    return NotFound($"No emergency contacts found for associate with ID {associateId}.");
                }

                return Ok(emergencyContacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving emergency contacts for associate with ID {AssociateId}.", associateId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("patron/{associateId}")]
        public async Task<ActionResult<IEnumerable<EmergencyContactDetailsDto>>> GetEmergencyContactsByPatronIdId(int patronId)
        {
            try
            {
                var emergencyContacts = await _dbContext.EmergencyContacts
                    .Where(ec => ec.PatronId == patronId)
                    .Include(ec => ec.Person)
                        .ThenInclude(p => p.ContactInfos)
                            .ThenInclude(ci => ci.ContactMaintenance)
                    .Include(ec => ec.Relationship)
                    .Select(ec => new EmergencyContactDetailsDto
                    {
                        EmergencyContactId = ec.EmergencyContactId,
                        FirstName = ec.Person.FirstName,
                        LastName = ec.Person.LastName,
                        Relationship = ec.Relationship.Relationship1,
                        ContactDetails = ec.Person.ContactInfos.Select(ci => new ContactInfoDto
                        {
                            Detail = ci.Details,
                            Type = ci.ContactMaintenance.Type
                        }).ToList(),
                        AssociateId = ec.AssociateId,
                        PatronId = ec.PatronId
                    })
                    .ToListAsync();

                if (!emergencyContacts.Any())
                {
                    _logger.LogInformation("No emergency contacts found for associate with ID {AssociateId}.", patronId);
                    return NotFound($"No emergency contacts found for associate with ID {patronId}.");
                }

                return Ok(emergencyContacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving emergency contacts for associate with ID {AssociateId}.", patronId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmergencyContact(int id, [FromBody] EmergencyContactSimpleUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emergencyContact = await _dbContext.EmergencyContacts.FindAsync(id);
            if (emergencyContact == null)
            {
                return NotFound($"EmergencyContact with ID {id} not found.");
            }

            try
            {
                // Assuming EmergencyContact contains fields like IsActive or RelationshipId that might need updating
                // Update only the fields that are relevant and provided in the updateDto
                
                if (updateDto.RelationshipId.HasValue)
                {
                    emergencyContact.RelationshipId = updateDto.RelationshipId.Value;
                }

                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the emergency contact with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergencyContact(int id)
        {
            var emergencyContact = await _dbContext.EmergencyContacts.FindAsync(id);
            if (emergencyContact == null)
            {
                return NotFound($"EmergencyContact with ID {id} not found.");
            }

            try
            {
                _dbContext.EmergencyContacts.Remove(emergencyContact);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the emergency contact with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
