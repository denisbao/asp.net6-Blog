using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels.Categories;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
  [ApiController]
  public class CategoryController : ControllerBase
  {
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var categories = await context.Categories.ToListAsync();
        return Ok(new ResultViewModel<List<Category>>(categories));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<List<Category>>("COD008: Falha interna do servidor."));
      }
    }


    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
          return NotFound(new ResultViewModel<string>("COD009: Conteúdo não encontrado"));

        return Ok(new ResultViewModel<Category>(category));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD007: Falha interna do servidor."));
      }
    }


    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
      [FromBody] EditorCategoryViewModel model,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

      try
      {
        var category = new Category
        {
          Id = 0,
          Name = model.Name,
          Slug = model.Slug.ToLower()
        };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
        return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD001: Não foi possível incluir a categoria."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD002: Falha interna do servidor."));
      }
    }


    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
      [FromRoute] int id,
      [FromBody] EditorCategoryViewModel model,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
          return NotFound(new ResultViewModel<string>("COD010: Conteúdo não encontrado"));

        category.Name = model.Name;
        category.Slug = model.Slug;
        context.Categories.Update(category);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD003: Não foi possível atualizar a categoria."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD004: Falha interna do servidor."));
      }
    }


    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
          return NotFound(new ResultViewModel<string>("COD011: Conteúdo não encontrado."));

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Category>(category));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD005: Não foi possível excluir a categoria."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD006: Falha interna do servidor."));
      }
    }
  }
}