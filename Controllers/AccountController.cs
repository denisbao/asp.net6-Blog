using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers
{
  [ApiController]
  public class AccountController : ControllerBase
  {
    [HttpPost("v1/accounts/login")]
    public IActionResult Login(
      [FromServices] TokenService _tokenService
    )
    {
      var token = _tokenService.GenerateToken(null);
      return Ok(token);
    }


    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
    [FromBody] RegisterViewModel request,
    [FromServices] BlogDataContext context
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

      var user = new User
      {
        Name = request.Name,
        Email = request.Email,
        Slug = request.Email.Replace("@", "-").Replace(".", "-")
      };

      // GERAÇÃO AUTOMÁTICA DE SENHA E ENCRIPTAÇÃO:
      // dotnet add package SecureIdentity
      var password = PasswordGenerator.Generate(25);
      user.Password = PasswordHasher.Hash(password);

      try
      {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return Ok(new ResultViewModel<dynamic>(new
        {
          user = user.Email,
          password  // APENAS PARA TESTES
        }));
      }
      catch (DbUpdateException)
      {
        return StatusCode(400, new ResultViewModel<string>("COD034: Este e-mail já está cadastrado."));
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD035: Falha interna no servidor"));
      }

    }
  }
}
