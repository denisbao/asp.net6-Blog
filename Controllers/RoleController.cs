using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
  [ApiController]
  public class RomeController : ControllerBase
  {
    [HttpGet("v1/roles")]
    public async Task<IActionResult> GetAsync(
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var roles = await context.Roles.ToListAsync();
        return Ok(new ResultViewModel<List<Role>>(roles));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<List<Role>>("COD012: Falha interna do servidor."));
      }
    }

    [HttpGet("v1/roles/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
          return NotFound(new ResultViewModel<string>("COD013: Conteúdo não encontrado."));

        return Ok(new ResultViewModel<Role>(role));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<List<Role>>("COD014: Falha interna do servidor."));
      }
    }


    [HttpPost("v1/roles")]
    public async Task<IActionResult> PostAsync(
      [FromBody] EditorRoleViewModel request,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));

      try
      {
        var role = new Role();
        role.Name = request.Name;
        role.Slug = request.Slug.ToLower();

        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
        return Created($"v1/roles/{role.Id}", new ResultViewModel<Role>(role));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD015: Não foi possível incluir o papel do usuário."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD016: Falha interna do servidor."));
      }
    }

    [HttpPut("v1/roles/{id:int}")]
    public async Task<IActionResult> PutAsync(
      [FromRoute] int id,
      [FromBody] EditorRoleViewModel request,
      [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));

      try
      {
        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
          return NotFound(new ResultViewModel<string>("COD017: Conteúdo não encontrado."));

        role.Name = request.Name;
        role.Slug = request.Slug.ToLower();
        context.Roles.Update(role);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Role>(role));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD018: Não foi possível atualizar o papel do usuário."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD019: Falha interna do servidor."));
      }
    }


    [HttpDelete("v1/roles/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
      [FromRoute] int id,
      [FromServices] BlogDataContext context
    )
    {
      try
      {
        var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
          return NotFound(new ResultViewModel<string>("COD020: Conteúdo não encontrado."));

        context.Roles.Remove(role);
        await context.SaveChangesAsync();
        return Ok(new ResultViewModel<Role>(role));
      }
      catch (DbUpdateException)
      {
        return StatusCode(500, new ResultViewModel<string>("COD021: Não foi possível excluir o papel do usuário."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD022: Falha interna do servidor."));
      }
    }

  }
}