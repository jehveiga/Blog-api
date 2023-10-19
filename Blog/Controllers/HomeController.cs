using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        // Health Check - EndPoint para status se a API est� online
        [HttpGet("")]
        public IActionResult Get() => Ok();


    }
}