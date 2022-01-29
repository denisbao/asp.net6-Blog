using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Extensions;
using Blog.Models;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Services
{
  public class TokenService
  {
    // dotnet add package Microsoft.AspNetCore.Authentication
    // dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    public string GenerateToken(User user)
    {
      // Criação do token handler, responsável por criar o token:
      var tokenHandler = new JwtSecurityTokenHandler();

      // Conversão da key em Configuration.cs para um array de bites:
      var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);

      // Conversão dos dados do usário autenticado em Claims (Extension Method):
      var claims = user.GetClaims();

      // Configurações do token:
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(2),
        SigningCredentials = new SigningCredentials
        (
          new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature
        ),
      };

      // Criação do token, baseado na configuração:
      var token = tokenHandler.CreateToken(tokenDescriptor);

      // Retorno do token como string:
      return tokenHandler.WriteToken(token);
    }
  }
}