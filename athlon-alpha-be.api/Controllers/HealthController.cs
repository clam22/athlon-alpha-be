using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace athlon_alpha_be.api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _health;

    public HealthController(HealthCheckService health)
    {
        _health = health;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var report = await _health.CheckHealthAsync();

        if (report.Status == HealthStatus.Healthy)
            return Ok("Healthy");

        return StatusCode(503, "Unhealthy");
    }

}
