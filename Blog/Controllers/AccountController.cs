using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    //private readonly TokenService _tokenService; // Exemplo de um modo mais verboso ou pode ser usado através da Anotação [FromServices] que dar na mesma da forma usada com construtor

    //public AccountController(TokenService tokenService) // criando a dependência através do construtor da controller para recebimento do tokeService
    //{
    //    _tokenService = tokenService;
    //}

    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25); // Gerar senha através da biblioteca SecureIdentity
        user.PasswordHash = PasswordHasher.Hash(password); // Gerando um novo hash usando a senha passada por parametro

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Chamada do envio do e-mail quando sucesso de criação do usuário (Precisa do serviço configurado)
            //emailService.Send(
            //    user.Name,
            //    user.Email,
            //    "Bem vindo ao blog",
            //    $"Sua senha é {password}");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password // O password em aplicações reais o ideal seria enviar por email.
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("O5X99 - Este E-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }


    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model, // no model virá uma senha sem o hash que está no banco
        [FromServices] BlogDataContext context,
        [FromServices] TokenService tokenService) // injeção de dependência do TokenService
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context.Users // a senha do usuário que virá pelo banco, estará com hash conforme a regra de negócios adotada
                                .AsNoTracking()
                                .Include(user => user.Roles)
                                .FirstOrDefaultAsync(user => user.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

        // Método que verificará se o hash coincide com password passado no modelo pelo que está no banco
        // Internamente realizará a extração do hash para comparar o passwords
        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválido"));

        // Se passar pela verificação do password irá efetuar a geração do token para autenticação da aplicação
        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {

            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

    }

}

