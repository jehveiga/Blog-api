using Blog.Data;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Adicionando end-points usando a nomeclatura API Rest
        [HttpGet("v1/categories")] // localhost:PORT/v1/categories
        public IActionResult Get(
            [FromServices] BlogDataContext context)
        {
            var categories = context.Categories.ToList();
            return Ok(categories);
        }
    }
}
