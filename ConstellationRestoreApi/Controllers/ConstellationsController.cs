using ConstellationRestoreApi.Dto;
using ConstellationRestoreApi.Services;
using ConstellationRestoreApi.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ConstellationRestoreApi.Controllers;

[ApiController]
[Route("api/constellations")]
public sealed class ConstellationsController : ControllerBase
{
    private readonly IConstellationRestoreService _restoreService;

    public ConstellationsController(IConstellationRestoreService restoreService)
    {
        _restoreService = restoreService;
    }

    [HttpPost("restore")]
    public IActionResult Restore([FromBody] ConstellationRestoreRequest request)
    {
        var errors = RestoreRequestValidator.Validate(request);

        if (errors.Any())
        {
            return BadRequest(new { errors });
        }

        var response = _restoreService.Restore(request);

        return Ok(response);
    }

    [HttpGet("examples")]
    public IActionResult Examples()
    {
        return Ok(new[]
        {
            new { reference = "DRAGON", damaged = "GOD", expected = new { missingSymbols = "ANR", isComplete = false } },
            new { reference = "VEGA", damaged = "GAVE", expected = new { missingSymbols = string.Empty, isComplete = true } },
            new { reference = "LYRA", damaged = "LRA", expected = new { missingSymbols = "Y", isComplete = false } },
            new { reference = "CYGNUS", damaged = string.Empty, expected = new { missingSymbols = "CGNSUY", isComplete = false } }
        });
    }
}