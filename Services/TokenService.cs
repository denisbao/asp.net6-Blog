using System.IdentityModel.Tokens.Jwt;
using System.Text;
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

      // Configurações do token:
      var tokenDescriptor = new SecurityTokenDescriptor();

      // Criação do token, baseado na configuração:
      var token = tokenHandler.CreateToken(tokenDescriptor);

      // Retorno do token como string:
      return tokenHandler.WriteToken(token);
    }
  }
}