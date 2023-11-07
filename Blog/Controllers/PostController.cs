using Blog.Data;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("v1/posts")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
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
                .ToListAsync();
            return Ok(posts);
        }

    }
}
