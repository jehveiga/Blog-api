using Blog.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "admin")]
    [Authorize(Roles = "user")] // Somente usuários que tiverem a regra informada pode acessar o método direcionado abaixo
    [HttpGet("v1/user")]
    public IActionResult GetUser() => Ok(User.Identity.Name);

    [Authorize(Roles = "author")] // Somente usuários que tiverem a regra informada pode acessar o método direcionado abaixo
    [HttpGet("v1/author")]
    public IActionResult GetAuthor() => Ok(User.Identity.Name);

    [Authorize(Roles = "admin")] // Somente usuários que tiverem a regra informada pode acessar o método direcionado abaixo
    [HttpGet("v1/admin")]
    public IActionResult GetAdmin() => Ok(User.Identity.Name);
}

