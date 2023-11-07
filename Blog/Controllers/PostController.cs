using Blog.Data;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class PostController : ControllerBase
{
    [HttpGet("v1/posts")]
    public async Task<IActionResult> GetAsync(
        [FromServices] BlogDataContext context,
        [FromQuery] int page = 0, // página que está, que pode ser enviado pela Url (chave = valor)
        [FromQuery] int pageSize = 25) // quantidade de itens por página, que pode ser enviado pela Url (chave = valor)
    {
        try
        {
            // Se precisar passar ao frontend a quantidade de registros para calcular o count
            var count = await context.Posts.AsNoTracking().CountAsync();

            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(post => post.Category)
                .Include(post => post.Author)
                .Select(post => new ListPostsViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    LastUpdateDate = post.LastUpdateDate,
                    Category = post.Category.Name,
                    Author = $"{post.Author.Name} ({post.Author.Email})"
                })
                .Skip(page * pageSize) // Quanto que será pulado de dados
                .Take(pageSize) // Quanto que será obtido de dados
                .OrderByDescending(post => post.LastUpdateDate)
                .ToListAsync();
            return Ok(new ResultViewModel<dynamic>(new // Retornando um objeto dinamico para ser passado ao result pode ser criado uma ViewModel
            {
                total = count,
                page,
                pageSize,
                posts
            }));
        }
        catch (Exception)
        {

            return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
        }
    }

    [HttpGet("v1/posts/{id:int}")]
    public async Task<IActionResult> DetailsAsync(
    [FromServices] BlogDataContext context,
    [FromRoute] int id)
    {
        try
        {
            var posts = await context
                .Posts
                .AsNoTracking()
                .Include(post => post.Author)
                .ThenInclude(post => post.Roles)
                .Include(post => post.Category)
                .FirstOrDefaultAsync(post => post.Id == id);

            if (posts == null)
                return NotFound(new ResultViewModel<Post>("Conteúdo não encontrado"));

            return Ok(new ResultViewModel<Post>(posts));

        }
        catch (Exception)
        {

            return StatusCode(500, new ResultViewModel<List<Post>>("05X04 - Falha interna do servidor"));
        }
    }
}



