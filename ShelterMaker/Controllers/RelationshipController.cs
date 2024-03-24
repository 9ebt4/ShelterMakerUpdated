using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelationshipController : ControllerBase
    {

        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<RelationshipController> _logger;

        public RelationshipController(ShelterDbContext dbContext, ILogger<RelationshipController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRelationship([FromBody] RelationshipCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Relationship1))
            {
                _logger.LogWarning("Invalid input data.");
                return BadRequest("Invalid data provided.");
            }

            try
            {
                var newRelationship = new Relationship
                {
                    Relationship1 = dto.Relationship1
                };

                await _dbContext.Relationships.AddAsync(newRelationship);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation($"Relationship created with ID {newRelationship.RelationshipId}.");
                return CreatedAtAction(nameof(GetRelationshipById), new { id = newRelationship.RelationshipId }, newRelationship);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new relationship.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRelationshipById(int id)
        {
            var relationship = await _dbContext.Relationships.FindAsync(id);
            if (relationship == null)
            {
                _logger.LogWarning($"Relationship with ID {id} not found.");
                return NotFound($"Relationship with ID {id} not found.");
            }

            var dto = new RelationshipDto 
            { 
                RelationshipId = relationship.RelationshipId, 
                RelationshipName = relationship.Relationship1 
            };

            return Ok(relationship);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RelationshipDto>>> GetAllRelationships()
        {
            try
            {
                var relationships = await _dbContext.Relationships
                    .Select(r => new RelationshipDto
                    {
                        RelationshipId = r.RelationshipId,
                        RelationshipName = r.Relationship1
                    })
                    .ToListAsync();

                if (relationships == null || !relationships.Any())
                {
                    _logger.LogWarning("No relationships found.");
                    return NotFound("No relationships found.");
                }

                return Ok(relationships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all relationships.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
