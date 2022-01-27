using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet("")]
    public IActionResult Get()
    {
      return Ok(new
      {
        api_version = "v1",
        status = "online"
      });
    }
  }
}