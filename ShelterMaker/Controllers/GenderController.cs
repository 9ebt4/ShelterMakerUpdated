using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<GenderController> _logger;

        public GenderController(ShelterDbContext dbContext, ILogger<GenderController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Gender>> GetGenderById(int id)
        {
            try
            {
                var gender = await _dbContext.Genders.FindAsync(id);

                if (gender == null)
                {
                    _logger.LogInformation($"Gender with ID {id} not found.");
                    return NotFound($"Gender with ID {id} not found.");
                }

                return Ok(gender);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving gender with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetAllGenders()
        {
            try
            {
                var genders = await _dbContext.Genders.ToListAsync();
                if (genders == null || !genders.Any())
                {
                    _logger.LogInformation("No genders found.");
                    return NotFound("No genders found.");
                }

                return Ok(genders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all genders.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
