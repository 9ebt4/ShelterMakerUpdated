using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<NoteController> _logger;

        public NoteController(ShelterDbContext dbContext, ILogger<NoteController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Note>> CreateNoteAsync(NoteCreateDto noteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newNote = new Note
            {
                AssociateId = noteDto.AssociateId,
                Content = noteDto.Content,
                NoteMaintenanceId = noteDto.NoteMaintenanceId,
                DateCreated = DateTime.UtcNow // Current date and time
            };

            try
            {
                await _dbContext.Notes.AddAsync(newNote);
                await _dbContext.SaveChangesAsync();

                // Associate note with patrons if any patron IDs are provided
                foreach (var patronId in noteDto.PatronIds)
                {
                    var patronNote = new PatronNote
                    {
                        NoteId = newNote.NoteId,
                        PatronId = patronId
                    };
                    await _dbContext.PatronNotes.AddAsync(patronNote);
                }

                if (noteDto.PatronIds.Any())
                {
                    await _dbContext.SaveChangesAsync();
                }

                // Assuming GetNoteByIdAsync exists for fetching a specific note details
                return CreatedAtAction(nameof(GetNoteByIdAsync), new { id = newNote.NoteId }, newNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new note.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDetailsDto>> GetNoteByIdAsync(int id)
        {
            try
            {
                var note = await _dbContext.Notes
                    .Where(n => n.NoteId == id)
                    .Select(n => new NoteDetailsDto
                    {
                        NoteId = n.NoteId,
                        AssociateId = n.AssociateId,
                        AssociateName = n.Associate != null ? n.Associate.GoogleUser.Person.FirstName + " " + n.Associate.GoogleUser.Person.LastName : "",
                        Content = n.Content,
                        DateCreated = n.DateCreated,
                        NoteMaintenanceCategory = n.NoteMaintenance != null ? n.NoteMaintenance.Category : "",
                        PatronIds = n.PatronNotes.Select(pn => pn.PatronId).ToList(),
                        PatronNames = n.PatronNotes.Select(pn => pn.Patron != null ? pn.Patron.Person.FirstName + " " + pn.Patron.Person.LastName : "").ToList()
                    })
                    .FirstOrDefaultAsync();

                if (note == null)
                {
                    return NotFound($"Note with ID {id} not found.");
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching note with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //[HttpGet("by-facility/{facilityId}")]
        //public async Task<ActionResult<IEnumerable<NoteDetailsDto>>> GetNotesByFacilityAsync(int facilityId, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        //{
        //    var query = _dbContext.Notes.AsQueryable();

        //    // Filter by notes linked to associates and patrons of the facility
        //    query = query.Where(n => n.Associate.FacilityId == facilityId || n.PatronNotes.Any(pn => pn.Patron.FacilityId == facilityId));

        //    // Optional: filter by date range
        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(n => n.DateCreated >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(n => n.DateCreated <= endDate.Value);
        //    }

        //    var notes = await query
        //        .Select(n => new NoteDetailsDto
        //        {
        //            NoteId = n.NoteId,
        //            AssociateId = n.AssociateId,
        //            AssociateName = n.Associate != null ? $"{n.Associate.GoogleUser.Person.FirstName} {n.Associate.GoogleUser.Person.LastName}" : "",
        //            Content = n.Content,
        //            DateCreated = n.DateCreated,
        //            NoteMaintenanceCategory = n.NoteMaintenance.Category,
        //            PatronIds = n.PatronNotes.Select(pn => pn.PatronId).ToList(),
        //            PatronNames = n.PatronNotes.Select(pn => $"{pn.Patron.Person.FirstName} {pn.Patron.Person.LastName}").ToList(),
        //        })
        //        .ToListAsync();

        //    if (!notes.Any())
        //    {
        //        return NotFound("No notes found for the specified facility.");
        //    }

        //    return Ok(notes);
        //}

        [HttpPut("{noteId}")]
        public async Task<IActionResult> UpdateNoteAsync(int noteId, [FromBody] UpdateNoteDto updateDto)
        {
            var note = await _dbContext.Notes.FindAsync(noteId);
            if (note == null)
            {
                return NotFound($"Note with ID {noteId} not found.");
            }

            // Update content if provided
            if (!string.IsNullOrEmpty(updateDto.Content))
            {
                note.Content = updateDto.Content;
            }

            // Update NoteMaintenanceId if provided
            if (updateDto.NoteMaintenanceId.HasValue)
            {
                note.NoteMaintenanceId = updateDto.NoteMaintenanceId.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent(); // Indicate successful update without returning data
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the note with ID {noteId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
