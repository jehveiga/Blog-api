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
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05X04 - Falha interna no servidor");
            }
        }

        [HttpGet("v1/categories/{id:int}")] // localhost:PORT/v1/categories/id - id parametro a ser procurado
        public async Task<IActionResult> GetByIdAsync(
                [FromRoute] int id,
                [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(category => category.Id == id);

                if (category is null)
                    return NotFound();

                return Ok(category);
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05X05 - Falha interna no servidor");
            }
        }

        [HttpPost("v1/categories")] // localhost:PORT/v1/categories/
        public async Task<IActionResult> PostAsync(
                [FromBody] Category category,
                [FromServices] BlogDataContext context)
        {
            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", category); // direcionando para URL referida passando o objeto criado
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05XE9 - Não foi possível incluir a categoria"); 
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05X10 - Falha interna no servidor");
            }

        }

        [HttpPut("v1/categories/{id:int}")] // localhost:PORT/v1/categories/id - id parametro a ser alterado
        public async Task<IActionResult> PutAsync(
                [FromRoute] int id,
                [FromBody] Category categoryModel,
                [FromServices] BlogDataContext context)
        {

            try
            {
                var categoryDatabase = await context.Categories.FirstOrDefaultAsync(categoryModel => categoryModel.Id == id);

                if (categoryDatabase is null)
                    return NotFound();

                categoryDatabase.Name = categoryModel.Name;
                categoryDatabase.Slug = categoryModel.Slug;

                context.Categories.Update(categoryDatabase);
                await context.SaveChangesAsync();

                return Ok(categoryModel);
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05XE8 - Não foi possível alterar a categoria");
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05X11 - Falha interna no servidor");
            }

        }

        [HttpDelete("v1/categories/{id:int}")] // localhost:PORT/v1/categories/id - id parametro a ser removido
        public async Task<IActionResult> DeleteAsync(
                [FromRoute] int id,
                [FromServices] BlogDataContext context)
        {
            try
            {
                var categoryDatabase = await context.Categories.FirstOrDefaultAsync(categoryModel => categoryModel.Id == id);

                if (categoryDatabase is null)
                    return NotFound();

                context.Categories.Remove(categoryDatabase);
                await context.SaveChangesAsync();

                return Ok(categoryDatabase);
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05XE7 - Não foi possível excluir a categoria");
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, "05X12 - Falha interna no servidor");
            }

        }
    }
}
