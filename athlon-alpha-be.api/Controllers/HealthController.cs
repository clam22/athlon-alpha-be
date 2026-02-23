using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace athlon_alpha_be.api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController(HealthCheckService _health) : ControllerBase
{

    [HttpGet("/health")]
    public async Task<IActionResult> GetAsync()
    {
        HealthReport report = await _health.CheckHealthAsync();

        if (report.Status == HealthStatus.Healthy)
        {
            return Ok("Healthy");
        }

        return StatusCode(503, "Unhealthy");
    }

}
