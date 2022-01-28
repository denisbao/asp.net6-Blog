using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
  [ApiController]
  public class TagController : ControllerBase
  {
    [HttpGet("v1/tags")]
    public async Task<IActionResult> GetAsync(
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var tags = await context.Tags.ToListAsync();
        return Ok(new ResultViewModel<List<Tag>>(tags));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<List<Tag>>("COD023: Falha interna do servidor."));
      }
    }


    [HttpGet("v1/tags/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

        if (tag == null)
          return NotFound(new ResultViewModel<string>("COD024: Conteúdo não encontrado"));

        return Ok(new ResultViewModel<Tag>(tag));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD025: Falha interna do servidor."));
      }
    }


    [HttpPost("v1/tags")]
    public async Task<IActionResult> PostAsync(
      [FromBody] EditorTagViewModel request,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

      try
      {
        var tag = new Tag();
        tag.Name = request.Name;
        tag.Slug = request.Slug.ToLower();

        await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
        return Created($"v1/tags/{tag.Id}", new ResultViewModel<Tag>(tag));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD026: Não foi possível incluir a tag."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD027: Falha interna do servidor."));
      }
    }


    [HttpPut("v1/tags/{id:int}")]
    public async Task<IActionResult> PutAsync(
      [FromRoute] int id,
      [FromBody] EditorTagViewModel request,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

      try
      {
        var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

        if (tag == null)
          return NotFound(new ResultViewModel<string>("COD028: Conteúdo não encontrado"));

        tag.Name = request.Name;
        tag.Slug = request.Slug;
        context.Tags.Update(tag);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Tag>(tag));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD029: Não foi possível atualizar a tag."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD030: Falha interna do servidor."));
      }
    }


    [HttpDelete("v1/tags/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var tag = await context.Tags.FirstOrDefaultAsync(x => x.Id == id);

        if (tag == null)
          return NotFound(new ResultViewModel<string>("COD031: Conteúdo não encontrado."));

        context.Tags.Remove(tag);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Tag>(tag));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD032: Não foi possível excluir a tag."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD033: Falha interna do servidor."));
      }
    }
  }
}