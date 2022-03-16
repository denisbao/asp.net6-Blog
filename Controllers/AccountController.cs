using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels.Accounts;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace Blog.Controllers
{
  [ApiController]
  public class AccountController : ControllerBase
  {
    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
      [FromBody] LoginViewModel request,
      [FromServices] BlogDataContext context,
      [FromServices] TokenService tokenService
    )
    {
      if (!ModelState.IsValid)
        return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

      // VERIFICANDO USUÁRIO:
      var user = await context.Users
        .AsNoTracking()
        .Include(x => x.Roles)
        .FirstOrDefaultAsync(x => x.Email == request.Email);

      if (user == null)
        return StatusCode(401, new ResultViewModel<string>("Usuáiro ou senha inválidos."));

      // VERIFICANDO SENHA: (package SecureIdentity)
      if (!PasswordHasher.Verify(user.Password, request.Password))
        return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

      try
      {
        var token = tokenService.GenerateToken(user);
        return Ok(new ResultViewModel<string>(token, null)); // se for passado só o token (string), o ViewModel vai achar que é um erro.
      }
      catch
      {
        return StatusCode(500, new ResultViewModel<string>("COD036: Falha interna no servidor."));
      }
    }


    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post(
    [FromBody] RegisterViewModel request,
    [FromServices] BlogDataContext context,
    [FromServices] EmailService emailService
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

        emailService.Send(user.Name, user.Email, "Bem vindo ao blog!", $"Sua senha é <strong>{password}</strong>");

        return Ok(new ResultViewModel<dynamic>(new
        {
          user = user.Email,
          password  // retornando a senha apenas para testes
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

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
      [FromBody] UploadImageViewModel model,
      [FromServices] BlogDataContext context
    )
    {
      // ...geração do nome aleatório para o arquivo:
      var fileName = $"{Guid.NewGuid().ToString()}.jpg";
      // ...recuperando a imagem da requisicção e eliminando informações não desejadas:
      var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image,"");
      // ...conversão da imagem para bytes:
      var bytes = Convert.FromBase64String(data);

      // ...salvando imagem em disco:
      try
      {
        await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
      }
      catch (Exception ex)
      {
         return StatusCode(500, new ResultViewModel<string>("COD036: Falha interna no servidor"));
      }

      // ... atualizando o usuário:
      var user = await context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
      if (user == null)
        return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

      user.Image = $"https://localhost:7200/images/{filename}";
      try
      {
        context.Users.Update(user);
        await context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         return StatusCode(500, new ResultViewModel<string>("COD037: Falha interna no servidor"));
      }

      return Ok(new ResultViewModel<string>("Imagem alterada com sucesso", null));
    }
  }
}
