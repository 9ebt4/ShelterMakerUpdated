using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<IntakeController> _logger;

        public ItemController(ShelterDbContext dbContext, ILogger<IntakeController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Item>> CreateItem([FromBody] ItemCreateDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newItem = new Item
            {
                Content = itemDto.Content,
                IsChecked = false, // Default to false when creating a new item
                CheckListId = itemDto.CheckListId
            };

            try
            {
                _dbContext.Items.Add(newItem);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemById), new { id = newItem.ItemId }, newItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the item.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id)
        {
            try
            {
                var item = await _dbContext.Items.FindAsync(id);

                if (item == null)
                {
                    return NotFound($"Item with ID {id} not found.");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, "An error occurred while fetching item with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("by-checklist/{checklistId}")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItemsByChecklistId(int checklistId)
        {
            try
            {
                var items = await _dbContext.Items
                    .Where(i => i.CheckListId == checklistId)
                    .ToListAsync();

                if (!items.Any())
                {
                    return NotFound($"No items found for checklist ID {checklistId}.");
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging purposes
                _logger.LogError(ex, $"An error occurred while fetching items for checklist ID {checklistId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] ItemUpdateDto itemUpdateDto)
        {
            var item = await _dbContext.Items.FindAsync(id);
            if (item == null)
            {
                _logger.LogWarning($"Item with ID {id} not found.");
                return NotFound($"Item with ID {id} not found.");
            }

            // Only update properties that are not null (i.e., were included in the request)
            if (itemUpdateDto.Content != null)
            {
                item.Content = itemUpdateDto.Content;
            }
            if (itemUpdateDto.IsChecked.HasValue)
            {
                item.IsChecked = itemUpdateDto.IsChecked.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Item with ID {id} updated.");
                return NoContent(); // Standard response for a successful PUT request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating item with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemAsync(int id)
        {
            var item = await _dbContext.Items.FindAsync(id);
            if (item == null)
            {
                _logger.LogWarning($"Item with ID {id} not found.");
                return NotFound($"Item with ID {id} not found.");
            }

            try
            {
                _dbContext.Items.Remove(item);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Item with ID {id} deleted.");
                return NoContent(); // Standard response for a successful DELETE request
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting item with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
