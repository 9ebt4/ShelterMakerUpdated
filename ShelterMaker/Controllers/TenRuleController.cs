using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenRuleController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<TenRuleController> _logger;

        public TenRuleController(ShelterDbContext dbContext, ILogger<TenRuleController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenRule([FromBody] TenRuleCreateDto tenRuleDto)
        {
            try
            {
                var newTenRule = new TenRule
                {
                    Completed = tenRuleDto.Completed ?? false,
                    Confirmed = tenRuleDto.Confirmed ?? false
                };

                _dbContext.TenRules.Add(newTenRule);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("TenRule created with ID {TenRulesId}.", newTenRule.TenRulesId);
                return CreatedAtAction(nameof(GetTenRuleById), new { id = newTenRule.TenRulesId }, newTenRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new TenRule.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TenRuleDetailDto>> GetTenRuleById(int id)
        {
            try
            {
                var tenRule = await _dbContext.TenRules
                    .Select(tr => new TenRuleDetailDto
                    {
                        TenRulesId = tr.TenRulesId,
                        Completed = tr.Completed,
                        Confirmed = tr.Confirmed
                    })
                    .FirstOrDefaultAsync(t => t.TenRulesId == id);

                if (tenRule == null)
                {
                    _logger.LogWarning("TenRule with ID {TenRulesId} not found.", id);
                    return NotFound($"TenRule with ID {id} not found.");
                }

                return Ok(tenRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching TenRule with ID {TenRulesId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenRule(int id, [FromBody] TenRuleUpdateDto tenRuleUpdateDto)
        {
            if (tenRuleUpdateDto == null)
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                var tenRule = await _dbContext.TenRules.FindAsync(id);

                if (tenRule == null)
                {
                    _logger.LogWarning("TenRule with ID {TenRulesId} not found.", id);
                    return NotFound($"TenRule with ID {id} not found.");
                }

                // Only update the properties if they are provided (i.e., not null)
                if (tenRuleUpdateDto.Completed.HasValue)
                {
                    tenRule.Completed = tenRuleUpdateDto.Completed.Value;
                }

                if (tenRuleUpdateDto.Confirmed.HasValue)
                {
                    tenRule.Confirmed = tenRuleUpdateDto.Confirmed.Value;
                }

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Updated TenRule with ID {TenRulesId}.", id);

                return NoContent(); // 204 No Content is a common response for successful PUT requests
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating TenRule with ID {TenRulesId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
