using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatronBanController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<PatronBanController> _logger;

        public PatronBanController(ShelterDbContext dbContext, ILogger<PatronBanController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }

    }
}
