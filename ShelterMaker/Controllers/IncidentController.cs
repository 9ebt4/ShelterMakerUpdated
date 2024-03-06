using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<IncidentController> _logger;

        public IncidentController(ShelterDbContext dbContext, ILogger<IncidentController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Incident>> CreateIncident([FromBody] IncidentCreateDto incidentDto)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var newIncident = new Incident
                    {
                        AssociateId = incidentDto.AssociateId,
                        DateCreated = DateTime.UtcNow, // Set to current UTC date-time
                        IncidentDate = incidentDto.IncidentDate,
                        IncidentMaintenanceId = incidentDto.IncidentMaintenanceId,
                        ActionTaken = incidentDto.ActionTaken,
                        EmergencyServices = incidentDto.EmergencyServices
                    };

                    // Create PatronIncident associations
                    foreach (var patronId in incidentDto.PatronIds)
                    {
                        newIncident.PatronIncidents.Add(new PatronIncident { PatronId = patronId });
                    }

                    _dbContext.Incidents.Add(newIncident);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetIncidentByIdAsync), new { id = newIncident.IncidentId }, newIncident);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error creating incident");
                    return StatusCode(500, "An error occurred while creating the incident");
                }
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IncidentDetailDto>> GetIncidentByIdAsync(int id)
        {
            try
            {
                var incidentQuery = _dbContext.Incidents
                .Where(i => i.IncidentId == id)
                .Select(i => new IncidentDetailDto
                {
                    IncidentId = i.IncidentId,
                    DateCreated = i.DateCreated,
                    Content = i.Content,
                    IncidentDate = i.IncidentDate,
                    ActionTaken = i.ActionTaken,
                    EmergencyServices = i.EmergencyServices,
                    AssociateDetails = new AssociateDetailDto
                    {
                        AssociateId = i.Associate.AssociateId, // Assuming Associate is always present. If not, further null checks are required.
                        FirstName = i.Associate.GoogleUser.Person.FirstName, // Ensuring the navigation properties are correctly chained.
                        LastName = i.Associate.GoogleUser.Person.LastName
                    },
                    PatronDetails = i.PatronIncidents.Select(pi => new PatronDetailDto
                    {
                        PatronId = pi.PatronId.Value, // Ensuring that PatronId is unwrapped safely.
                        FirstName = pi.Patron.Person.FirstName, // Assuming that each Patron has a Person. Null checks may be needed.
                        LastName = pi.Patron.Person.LastName
                    }).ToList(),
                    IncidentCategory = i.IncidentMaintenance.Category // Assuming Category is not null.
                });

                var incidentDto = await incidentQuery.FirstOrDefaultAsync();

                if (incidentDto == null)
                {
                    _logger.LogWarning($"Incident with ID {id} not found.");
                    return NotFound($"Incident with ID {id} not found.");
                }

                return Ok(incidentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating incident");
                return StatusCode(500, "An error occurred while creating the incident");
            }

        }

        [HttpGet("by-facility/{facilityId}")]
        public async Task<ActionResult<IEnumerable<IncidentDetailDto>>> GetIncidentsByFacilityAsync(int facilityId, [FromQuery] DateTime? dateMin, [FromQuery] DateTime? dateMax, [FromQuery] bool? emergencyServices, [FromQuery] int? incidentMaintenanceId)
        {
            try
            {
                var query = _dbContext.Incidents.AsQueryable();
                
                query = query.Where(i => (i.Associate != null && i.Associate.FacilityId == facilityId) ||
                                 (i.PatronIncidents.Any(pi => pi.Patron.FacilityId == facilityId)));
                 
                if (dateMin.HasValue)
                {
                    query = query.Where(i => i.IncidentDate >= dateMin.Value);
                }
                if (dateMax.HasValue)
                {
                    query = query.Where(i => i.IncidentDate <= dateMax.Value);
                }
                if (emergencyServices.HasValue)
                {
                    query = query.Where(i => i.EmergencyServices == emergencyServices.Value);
                }

                if (incidentMaintenanceId.HasValue)
                {
                    query = query.Where(i => i.IncidentMaintenanceId == incidentMaintenanceId.Value);
                }

                var incidents =await query.Select(i => new IncidentDetailDto
                {
                    IncidentId = i.IncidentId,
                    DateCreated = i.DateCreated,
                    Content = i.Content,
                    IncidentDate = i.IncidentDate,
                    ActionTaken = i.ActionTaken,
                    EmergencyServices = i.EmergencyServices,
                    AssociateDetails = new AssociateDetailDto
                    {
                        AssociateId = i.Associate.AssociateId, // Assuming Associate is always present. If not, further null checks are required.
                        FirstName = i.Associate.GoogleUser.Person.FirstName, // Ensuring the navigation properties are correctly chained.
                        LastName = i.Associate.GoogleUser.Person.LastName
                    },
                    PatronDetails = i.PatronIncidents.Select(pi => new PatronDetailDto
                    {
                        PatronId = pi.PatronId.Value, // Ensuring that PatronId is unwrapped safely.
                        FirstName = pi.Patron.Person.FirstName, // Assuming that each Patron has a Person. Null checks may be needed.
                        LastName = pi.Patron.Person.LastName
                    }).ToList(),
                    IncidentCategory = i.IncidentMaintenance.Category // Assuming Category is not null.
                })
                    .ToListAsync();

                if (!incidents.Any())
                {
                    return NotFound($"No incidents found for facility with ID {facilityId}.");
                }

                return Ok(incidents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching incidents for facility with ID {facilityId}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIncidentAsync(int id, [FromBody] IncidentUpdateDto updateDto)
        {
            var incident = await _dbContext.Incidents.FindAsync(id);
            if (incident == null)
            {
                _logger.LogWarning($"Incident with ID {id} not found.");
                return NotFound($"Incident with ID {id} not found.");
            }

            // Update properties if a new value is provided
            if (!string.IsNullOrWhiteSpace(updateDto.Content))
            {
                incident.Content = updateDto.Content;
            }
            if (updateDto.IncidentDate.HasValue)
            {
                incident.IncidentDate = updateDto.IncidentDate.Value;
            }
            if (updateDto.IncidentMaintenanceId.HasValue)
            {
                incident.IncidentMaintenanceId = updateDto.IncidentMaintenanceId.Value;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.ActionTaken))
            {
                incident.ActionTaken = updateDto.ActionTaken;
            }
            if (updateDto.EmergencyServices.HasValue)
            {
                incident.EmergencyServices = updateDto.EmergencyServices.Value;
            }

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Incident with ID {id} updated.");
                return NoContent(); // 204 No Content is typically returned for successful updates
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating incident with ID {id}.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }
}
