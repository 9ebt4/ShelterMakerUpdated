using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<PersonController> _logger;

        public PersonController(ShelterDbContext dbContext, ILogger<PersonController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] PersonCreateDto personDto)
        {
            try
            {
                var newPerson = new Person
                {
                    FirstName = personDto.FirstName,
                    LastName = personDto.LastName,
                    BirthDay = personDto.BirthDay,
                    GenderId = personDto.GenderId,
                    MiddleName = personDto.MiddleName
                   
                };

                _dbContext.People.Add(newPerson);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"A new person was created with ID {newPerson.PersonId}.");
                return CreatedAtAction("GetPerson", new { id = newPerson.PersonId }, newPerson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new person.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDetailDto>> GetPersonById(int id)
        {
            try
            {
                var person = await _dbContext.People
                    .Where(p => p.PersonId == id)
                    .Select(p => new PersonDetailDto
                    {
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        BirthDay = p.BirthDay,
                        Gender = p.Gender != null ? p.Gender.Category : "Not Specified", // Assuming the Gender entity has a 'Category' property
                        MiddleName = p.MiddleName
                    })
                    .FirstOrDefaultAsync();

                if (person == null)
                {
                    _logger.LogInformation($"Person with ID {id} not found.");
                    return NotFound($"Person with ID {id} not found.");
                }

                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retrieving person with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
