using Blog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Adicionando end-points usando a nomeclatura API Rest
        [HttpGet("v1/categories")] // localhost:PORT/v1/categories
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}
