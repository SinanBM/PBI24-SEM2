using Microsoft.AspNetCore.Mvc;

namespace Nexttech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("API is healthy");
    }
}
