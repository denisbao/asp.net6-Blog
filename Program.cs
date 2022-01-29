using System.Text;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURAÇÃO DA AUTENTICAÇÃO PELO TOKENSERVICE:
var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
builder.Services.AddAuthentication(x =>
{
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
  x.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});

// CONFIGURAÇÃO DE CONTROLLERS:
builder
  .Services
  .AddControllers()
  .ConfigureApiBehaviorOptions(options =>
  {
    options.SuppressModelStateInvalidFilter = true;
  });

// CONFIGURAÇÃO DATABASE CONTEXT:
builder.Services.AddDbContext<BlogDataContext>();

// TOKEN SERVICE LIFETIME:
// .AddTransient();  // gera uma nova instância do objeto a cada chamada do [FromService]
// .AddScoped();     // disponível por todo escopo do método que chamou o [FromService]
// .AddSingleton();  // cria uma instância do objeto na execução da aplicação.
builder.Services.AddTransient<TokenService>();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
