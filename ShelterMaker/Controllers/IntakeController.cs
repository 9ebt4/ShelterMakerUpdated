using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterMaker.Models;

namespace ShelterMaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntakeController : ControllerBase
    {
        private readonly ShelterDbContext _dbContext;
        private readonly ILogger<IntakeController> _logger;

        public IntakeController(ShelterDbContext dbContext, ILogger<IntakeController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger;
        }


    }
}
