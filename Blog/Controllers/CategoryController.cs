using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        // Adicionando end-points usando a nomeclatura API Rest
        [HttpGet("v1/categories")] // localhost:PORT/v1/categories
        public async Task<IActionResult> GetAsync(
                [FromServices] IMemoryCache memoryCache,
                [FromServices] BlogDataContext context)
        {
            try
            {
                // Tentará obter a listagem pelo cache em memoria se não houver irá criar o cache com a lista retornada do método
                var categories = memoryCache.GetOrCreate("CategoriesCache", async entry =>
                {
                    // Adicionando o tempo de expiração do cache da lista buscada
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return await GetCategories(context);
                });

                return Ok(new ResultViewModel<List<Category>>(await categories));
            }
            catch
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<List<Category>>("05X04 - Falha interna no servidor"));
            }
        }

        private async Task<List<Category>> GetCategories(BlogDataContext context)
        {
            return await context.Categories.ToListAsync();
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05X05 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")] // localhost:PORT/v1/categories/
        public async Task<IActionResult> PostAsync(
                [FromBody] EditorCategoryViewModel categoryViewModel,
                [FromServices] BlogDataContext context)
        {
            // Manipulando a validação do ModelState
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = categoryViewModel.Name,
                    Slug = categoryViewModel.Slug.ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category)); // direcionando para URL referida passando o objeto criado
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05XE9 - Não foi possível incluir a categoria"));
            }
            catch
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05X10 - Falha interna no servidor"));
            }

        }

        [HttpPut("v1/categories/{id:int}")] // localhost:PORT/v1/categories/id - id parametro a ser alterado
        public async Task<IActionResult> PutAsync(
                [FromRoute] int id,
                [FromBody] EditorCategoryViewModel categoryViewModel,
                [FromServices] BlogDataContext context)
        {

            try
            {
                var categoryDatabase = await context.Categories.FirstOrDefaultAsync(categoryModel => categoryModel.Id == id);

                if (categoryDatabase is null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                categoryDatabase.Name = categoryViewModel.Name;
                categoryDatabase.Slug = categoryViewModel.Slug;

                context.Categories.Update(categoryDatabase);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(categoryDatabase));
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível alterar a categoria"));
            }
            catch
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05X11 - Falha interna no servidor"));
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
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(categoryDatabase);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(categoryDatabase));
            }
            catch (DbUpdateException)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05XE7 - Não foi possível excluir a categoria"));
            }
            catch (Exception)
            {
                // código do início identifica o erro no caso criado na regra de negócio do projeto para identificaçào encontrar onde foi o erro mais facilmente
                return StatusCode(500, new ResultViewModel<Category>("05X12 - Falha interna no servidor"));
            }

        }
    }
}
