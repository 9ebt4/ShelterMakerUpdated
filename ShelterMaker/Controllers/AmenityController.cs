using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<AmenityController> _logger;

        public AmenityController(ShelterDbContext dbContext, ILogger<AmenityController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Amenity>>> GetAllAmenitiesAsync()
        {
            try
            {
                var amenities = await _dbContext.Amenities.ToListAsync();
                if (amenities == null || amenities.Count == 0)
                {
                    _logger.LogInformation("No amenities found.");
                    return NotFound("No amenities found.");
                }
                return Ok(amenities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all amenities.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
