using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.WebAPI.Controllers;
[ApiController]
[Route("/")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Api is working");
    }
}
