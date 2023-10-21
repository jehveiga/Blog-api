using Blog.Data;
using Blog.Models;
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

        [HttpGet("v1/categories/{id:int}")] // localhost:PORT/v1/categories/id - id parametro a ser procurado
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(category => category.Id == id);

            if (category is null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost("v1/categories")] // localhost:PORT/v1/categories/
        public async Task<IActionResult> PostAsync(
                [FromBody] Category category,
                [FromServices] BlogDataContext context)
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category); // direcionando para URL referida passando o objeto criado
        }

        [HttpPut("v1/categories/{id:int}")] // localhost:PORT/v1/categories/
        public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] Category categoryModel,
        [FromServices] BlogDataContext context)
        {
            var categoryDatabase = await context.Categories.FirstOrDefaultAsync(categoryModel => categoryModel.Id == id);

            if (categoryDatabase is null)
                return NotFound();

            categoryDatabase.Name = categoryModel.Name;
            categoryDatabase.Slug = categoryModel.Slug;

            context.Categories.Update(categoryDatabase);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{categoryModel.Id}", categoryModel); // direcionando para URL referida passando o objeto alterado

        }
    }
}
