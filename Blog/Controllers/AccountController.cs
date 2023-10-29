using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    //private readonly TokenService _tokenService; // Exemplo de um modo mais verboso ou pode ser usado através da Anotação [FromServices] que dar na mesma da forma usada com construtor

    //public AccountController(TokenService tokenService) // criando a dependência através do construtor da controller para recebimento do tokeService
    //{
    //    _tokenService = tokenService;
    //}


    [HttpPost("v1/login")]
    public IActionResult Login([FromServices] TokenService tokenService) // injeção de dependência do TokenService
    {
        var token = tokenService.GenerateToken(null);

        return Ok(token);
    }
}

