using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactInfoController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public ContactInfoController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost("for-person")]
        public async Task<IActionResult> CreatePersonContactInfo([FromBody] PersonContactInfoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newContactInfo = new ContactInfo
                {
                    ContactMaintenanceId = dto.ContactMaintenanceId,
                    PersonId = dto.PersonId,
                    Details = dto.Details,
                    // FacilityId is intentionally left out for this specific use case
                };

                _dbContext.ContactInfos.Add(newContactInfo);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetContactInfoById), new { id = newContactInfo.ContactInfoId }, newContactInfo);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, "An error occurred while creating new contact info for person ID {PersonId}.", dto.PersonId);

                // Return a generic 500 Internal Server Error status code to the client
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }


        [HttpPost("for-facility")]
        public async Task<IActionResult> CreateFacilityContactInfo([FromBody] FacilityContactInfoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newContactInfo = new ContactInfo
                {
                    ContactMaintenanceId = dto.ContactMaintenanceId,
                    FacilityId = dto.FacilityId,
                    Details = dto.Details,
                    // PersonId is intentionally left null
                };

                _dbContext.ContactInfos.Add(newContactInfo);
                await _dbContext.SaveChangesAsync();

                // Return the created ContactInfo with a 201 Created response
                // Adjust the route name and values if necessary
                return CreatedAtAction(nameof(GetContactInfoById), new { id = newContactInfo.ContactInfoId }, newContactInfo);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An error occurred while creating new contact info for facility ID {FacilityId}.", dto.FacilityId);

                // Return a generic 500 Internal Server Error response
                return StatusCode(500, "An error occurred while processing your request. Please try again later.");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactInfoById(int id)
        {
            try
            {
                var contactInfo = await _dbContext.ContactInfos.FindAsync(id);

                if (contactInfo == null)
                {
                    _logger.LogWarning("ContactInfo with ID {Id} not found.", id);
                    return NotFound($"ContactInfo with ID {id} not found.");
                }

                return Ok(contactInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ContactInfo with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("by-person/{personId}")]
        public async Task<IActionResult> GetAllContactInfoByPersonId(int personId)
        {
            try
            {
                var contacts = await _dbContext.ContactInfos
                                    .Where(ci => ci.PersonId == personId)
                                    .Include(ci => ci.ContactMaintenance)
                                    .ToListAsync();

                if (contacts == null || contacts.Count == 0)
                {
                    _logger.LogWarning("No contact information found for Person with ID {PersonId}.", personId);
                    return NotFound($"No contact information found for Person with ID {personId}.");
                }

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching contact information for Person with ID {PersonId}.", personId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("by-facility/{facilityId}")]
        public async Task<IActionResult> GetAllContactInfoByFacilityId(int facilityId)
        {
            try
            {
                var contacts = await _dbContext.ContactInfos
                                    .Where(ci => ci.FacilityId == facilityId)
                                    .Include(ci => ci.ContactMaintenance)
                                    .ToListAsync();

                if (!contacts.Any())
                {
                    _logger.LogWarning("No contact information found for Facility with ID {FacilityId}.", facilityId);
                    return NotFound($"No contact information found for Facility with ID {facilityId}.");
                }

                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching contact information for Facility with ID {FacilityId}.", facilityId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactInfo(int id, [FromBody] ContactInfoUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactInfo = await _dbContext.ContactInfos.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound($"No contact information found with ID {id}.");
            }

            try
            {
                // Update properties if provided
                if (dto.ContactMaintenanceId.HasValue)
                    contactInfo.ContactMaintenanceId = dto.ContactMaintenanceId;
                if (!string.IsNullOrWhiteSpace(dto.Details))
                    contactInfo.Details = dto.Details;
                if (dto.PersonId.HasValue)
                    contactInfo.PersonId = dto.PersonId;
                if (dto.FacilityId.HasValue)
                    contactInfo.FacilityId = dto.FacilityId;

                await _dbContext.SaveChangesAsync();
                return NoContent(); // Standard response for a successful PUT request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating contact information with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactInfo(int id)
        {
            var contactInfo = await _dbContext.ContactInfos.FindAsync(id);
            if (contactInfo == null)
            {
                _logger.LogInformation("Delete operation requested for non-existing ContactInfo with ID {Id}.", id);
                return NotFound($"ContactInfo with ID {id} not found.");
            }

            try
            {
                _dbContext.ContactInfos.Remove(contactInfo);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("ContactInfo with ID {Id} has been successfully deleted.", id);
                return NoContent(); // Standard response for a successful DELETE request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to delete ContactInfo with ID {Id}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
