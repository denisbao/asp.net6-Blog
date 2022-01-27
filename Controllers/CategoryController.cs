using Blog.Data;
using Blog.Models;
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
        return Ok(categories);
      }
      catch
      {
        return StatusCode(500, "COD008: Falha interna do servidor.");
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
          return NotFound();

        return Ok(category);
      }
      catch
      {
        return StatusCode(500, "COD007: Falha interna do servidor.");
      }
    }


    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
      [FromBody] EditorCategoryViewModel model,
      [FromServices] BlogDataContext context
    )
    {
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
        return Created($"v1/categories/{category.Id}", category);
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, "COD001: Não foi possível incluir a categoria.");
      }
      catch
      {
        return StatusCode(500, "COD002: Falha interna do servidor.");
      }
    }


    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
      [FromRoute] int id,
      [FromBody] EditorCategoryViewModel model,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
          return NotFound();

        category.Name = model.Name;
        category.Slug = model.Slug;
        context.Categories.Update(category);
        await context.SaveChangesAsync();
        return Ok(category);
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, "COD003: Não foi possível atualizar a categoria.");
      }
      catch
      {
        return StatusCode(500, "COD004: Falha interna do servidor.");
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
          return NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return Ok(category);
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, "COD005: Não foi possível excluir a categoria.");
      }
      catch
      {
        return StatusCode(500, "COD006: Falha interna do servidor.");
      }
    }
  }
}