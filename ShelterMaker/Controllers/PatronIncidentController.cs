using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronIncidentController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<PatronIncidentController> _logger;

        public PatronIncidentController(ShelterDbContext dbContext, ILogger<PatronIncidentController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }


    }
}
