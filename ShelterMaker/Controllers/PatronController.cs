using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterMaker.DTOs;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<PatronController> _logger;

        public PatronController(ShelterDbContext dbContext, ILogger<PatronController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Patron>> CreatePatron([FromBody] PatronCreateDto dto)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Person
                    var newPerson = new Person
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        MiddleName = dto.MiddleInitial,
                        BirthDay = dto.DateOfBirth
                    };
                    _dbContext.People.Add(newPerson);
                    await _dbContext.SaveChangesAsync();

                    // Create Initial, SexualOffender, Requirements, TenRules, WorkPass with default values
                    var newInitial = new Initial { Locations = false, Medical = false, Covid = false, InitialAgreement = false };
                    var newSexualOffender = new SexualOffender { Completed = false, IsOffender = dto.IsSexualOffender };
                    var newRequirements = new Requirement { Completed = false, Confirmed = false };
                    var newTenRules = new TenRule { Completed = false, Confirmed = false };
                    var newWorkPass = new WorkPass { Needed = false, Confirmed = false };

                    // Create Intake and link the above objects
                    var newIntake = new Intake
                    {
                        Initial = newInitial,
                        SexualOffender = newSexualOffender,
                        Requirements = newRequirements,
                        TenRules = newTenRules
                    };
                    _dbContext.Intakes.Add(newIntake);
                    await _dbContext.SaveChangesAsync();

                    // Finally, create the Patron and link the created objects
                    var newPatron = new Patron
                    {
                        PersonId = newPerson.PersonId,
                        IntakeId = newIntake.IntakeId,
                        WorkPass = newWorkPass,
                        IsActive = true,
                        LastCheckIn = DateTime.UtcNow,
                        FacilityId = dto.FacilityId
                    };
                    _dbContext.Patrons.Add(newPatron);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _logger.LogInformation($"Patron created with ID {newPatron.PatronId}.");
                    return CreatedAtAction(nameof(GetPatronDetailById), new { id = newPatron.PatronId }, newPatron);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "An error occurred while creating a new patron.");
                    return StatusCode(500, "An error occurred while processing your request.");
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatronDetailDto>> GetPatronDetailById(int id)
        {
            var patron = await _dbContext.Patrons
                .Where(p => p.PatronId == id)
                .Select(p => new PatronDetailDto
                {
                    FirstName = p.Person.FirstName,
                    LastName = p.Person.LastName,
                    Age = DateTime.Today.Year - p.Person.BirthDay.Value.Year, // Simplified age calculation
                    Gender = p.Person.Gender.Category, // Assuming Gender is a navigation property in Person
                    PassPhrase = p.PassPhrase,
                    Bed = p.Bed != null ? new BedNameDTO
                    {
                        Id = p.Bed.BedId,
                        Name = p.Bed.Name,
                    } : null,
                    WorkPass = p.WorkPass != null ? new WorkPassDto
                    {
                        Id = p.WorkPass.WorkPassId,
                        Needed = p.WorkPass.Needed,
                        Confirmed = p.WorkPass.Confirmed,
                    } : null,
                    Initial = p.Intake.Initial != null ? new InitialCheckDto
                    {
                        Id = p.Intake.Initial.InitialId,
                        complete = p.Intake.Initial.InitialAgreement == true
                                   && p.Intake.Initial.Covid == true
                                   && p.Intake.Initial.Locations == true
                                   && p.Intake.Initial.Medical == true,
                    } : null,
                    Requirements = p.Intake.Requirements != null ? new RequirementsDto
                    {
                        Id = p.Intake.Requirements.RequirementsId,
                        Completed = p.Intake.Requirements.Completed,
                        Confirmed = p.Intake.Requirements.Confirmed,
                    } : null,
                    SexOffender = p.Intake.SexualOffender != null ? new SexualOffenderDto
                    {
                        Id = p.Intake.SexualOffender.SexualOffenderId,
                        Complete= p.Intake.SexualOffender.Completed,
                        isOffender = p.Intake.SexualOffender.IsOffender,
                    }:null,
                    TenRules = p.Intake.TenRules != null ? new TenRulesDto
                    {
                        Id = p.Intake.TenRulesId,
                        Completed = p.Intake.TenRules.Completed,
                        Confirmed = p.Intake.TenRules.Confirmed,
                    }:null,
                    MedicalConditions = p.MedicalConditions.Select(mc => new MedicalConditionDetailsDto
                    {
                        MedicalConditionId = mc.MedicalConditionId,
                        PatronId = mc.PatronId,
                        Details = mc.Details,
                        MedicalConditionMaintenanceId = mc.MedicalConditionMaintenanceId,
                        MedicalConditionMaintenanceCategory= mc.MedicalConditionMaintenance.Category,
                    }).ToList(),
                    BanDetails = p.PatronBans.Select(b => new BanDetailDto
                    {
                        Id = b.Ban.BanId,
                        StartDate = b.Ban.StartDate,
                        EndDate = b.Ban.EndDate,
                        IsActive = b.Ban.IsActive,
                        IncidentReportID = b.Ban.IncidentReportId,
                        BanMaintenanceId = b.Ban.BanMaintenanceId,
                        Category = b.Ban.BanMaintenance.Category,
                    }).ToList(),
                    ContactInfos = p.Person.ContactInfos.Select(ci => createContactDTO(ci)).ToList(),
                    EmergencyContacts = p.EmergencyContacts.Select(emc => new EmergencyContactDetailsDto
                    {
                        EmergencyContactId = emc.EmergencyContactId,
                        AssociateId= null,
                        FirstName = emc.Person.FirstName,
                        LastName = emc.Person.LastName,
                        PatronId = emc.PatronId,
                        ContactDetails = emc.Person.ContactInfos.Select(ci => createContactDTO(ci)).ToList(),
                    }).ToList(),
                    CaseWorkers = p.CaseWorkerPatrons.Select(cw => new CaseWorkerInfoDto
                    {
                        Id = cw.CaseWorkerPatronId,
                        FirstName = cw.CaseWorker.FirstName,
                        LastName = cw.CaseWorker.LastName,
                        ContactDetails= cw.CaseWorker.ContactInfos.Select(ci=>createContactDTO(ci)).ToList(),
                    }).ToList(),
                    InfoReleases = p.InfoReleases.Select(ir=> new InfoReleaseDto
                    {
                       Id=ir.Id,
                       FirstName = ir.Person.FirstName,
                       LastName = ir.Person.LastName,
                       BirthDay = ir.Person.BirthDay,
                       ContactInfos = ir.Person.ContactInfos.Select(ci=> createContactDTO(ci)).ToList(),
                    }).ToList(),
                    
                })
                .FirstOrDefaultAsync();

            if (patron == null)
            {
                return NotFound();
            }

            return Ok(patron);
        }

        public static ContactInfoDto createContactDTO(ContactInfo info)
        {
            return new ContactInfoDto
            {
                Id = info.ContactInfoId,
                Detail = info.Details,
                Type = info.ContactMaintenance.Type,
            };
        }


    }
}
