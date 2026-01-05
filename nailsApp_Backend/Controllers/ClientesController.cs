using Microsoft.AspNetCore.Mvc;

namespace nailsApp_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesControllerController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}
