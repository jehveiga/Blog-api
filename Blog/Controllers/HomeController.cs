using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        // Health Check - EndPoint para status se a API está online
        [HttpGet("")]
        [ApiKey] // Atributo criado caso tenha - Exemplo: um robo de configuração para um endpoint específico como leitura de artigos etc 
        public IActionResult Get()
        {
            return Ok();
        }


    }
}