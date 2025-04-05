using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIAirline.Data;

namespace SIAirline.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase 
    {
        private readonly ApplicationDbContext _context;

        public HealthController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpGet("api/health")]
        public IActionResult GetHealth() {
            // Check if the database is reachable
            try {
                _context.Database.CanConnect();
                return Ok(new {
                    status = "Healthy",
                    message = "Database connection is healthy"
                });
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database connection failed: {ex.Message}");
            }
        }

    }
}
