using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleUserController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<GoogleUserController> _logger;

        public GoogleUserController(ShelterDbContext dbContext, ILogger<GoogleUserController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoogleUser([FromBody] GoogleUserCreateDto googleUserDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Create the Person
                var person = new Person
                {
                    FirstName = googleUserDto.FirstName,
                    LastName = googleUserDto.LastName
                };
                _dbContext.People.Add(person);
                await _dbContext.SaveChangesAsync();

                // Create the ContactInfo for the Person
                var contactInfo = new ContactInfo
                {
                    PersonId = person.PersonId,
                    Details = googleUserDto.Email,
                    // Assuming ContactMaintenanceId is predefined, e.g., 1 for Email
                    ContactMaintenanceId = 1
                };
                _dbContext.ContactInfos.Add(contactInfo);

                // Create the GoogleUser
                var googleUser = new GoogleUser
                {
                    GoogleToken = googleUserDto.GoogleToken,
                    IsActive = false,
                    PersonId = person.PersonId
                };
                _dbContext.GoogleUsers.Add(googleUser);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetGoogleUserById), new { id = googleUser.GoogleUserId }, googleUser);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while creating a new GoogleUser.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GoogleUser>> GetGoogleUserById(int id)
        {
            try
            {
                var googleUser = await _dbContext.GoogleUsers.FindAsync(id);

                if (googleUser == null)
                {
                    return NotFound($"GoogleUser with ID {id} not found.");
                }

                return Ok(googleUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching GoogleUser with ID {id}.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoogleUser(int id, [FromBody] GoogleUserUpdateDto googleUserUpdateDto)
        {
            try
            {
                var googleUser = await _dbContext.GoogleUsers.FindAsync(id);
                if (googleUser == null)
                {
                    return NotFound($"GoogleUser with ID {id} not found.");
                }

                // Only update if the value is provided
                if (googleUserUpdateDto.GoogleToken != null)
                {
                    googleUser.GoogleToken = googleUserUpdateDto.GoogleToken;
                }
                if (googleUserUpdateDto.IsActive.HasValue) // Check for nullable boolean
                {
                    googleUser.IsActive = googleUserUpdateDto.IsActive.Value;
                }

                _dbContext.GoogleUsers.Update(googleUser);
                await _dbContext.SaveChangesAsync();

                return NoContent(); // Indicating the update was successful without returning data
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating GoogleUser with ID {id}.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }

    }
}
