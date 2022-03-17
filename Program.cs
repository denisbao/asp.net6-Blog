using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;
using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);
ConfigureAuthenticantion(builder);
ConfigureMvc(builder);
ConfigureServices(builder);


var app = builder.Build();
LoadConfiguration(app);
// CONFIGURAÇÕES DE AUTENTICAÇÃO E AUTORIZAÇÃO:
app.UseAuthentication();
app.UseAuthorization();
// HABILITA A COMPRESSÃO DOS DADOS DE RESPOSTA:
app.UseResponseCompression();
// HABILITA A RENDERIZAÇÃO DE ARQUIVOS ESTÁTICOS (imagen, pdf, ...) NO DIRETÓRIO "WWWROOT":
app.UseStaticFiles();
// HABILITA O USO DOS CONTROLLERS:
app.MapControllers();


app.Run();


// CARREGAMENTO DAS CONFIGURAÇÕES DO APPSETTINGS
void LoadConfiguration(WebApplication app)
{
  Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
  Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
  Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

  var smtp = new Configuration.SmtpConfiguration();
  app.Configuration.GetSection("Smtp").Bind(smtp);
  Configuration.Smtp = smtp;
}

// CONFIGURAÇÃO DA AUTENTICAÇÃO PELO TOKENSERVICE:
void ConfigureAuthenticantion(WebApplicationBuilder builder)
{
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
}

// CONFIGURAÇÃO DE CONTROLLERS:
void ConfigureMvc(WebApplicationBuilder builder)
{
  builder.Services.AddMemoryCache();
  builder.Services.AddResponseCompression(options =>
  {
    // options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    // options.Providers.Add<CustomCompressionProvider>();
  });
  builder.Services.Configure<GzipCompressionProviderOptions>(options =>
  {
    options.Level = CompressionLevel.Optimal;
  });
  builder
    .Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
      options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(x =>
    {
      x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
      x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    });
}

// CONFIGURAÇÃO DE SERVIÇOS:
void ConfigureServices(WebApplicationBuilder builder)
{
  // Database Context:
  builder.Services.AddDbContext<BlogDataContext>();
  // Serviço de envio de email:
  builder.Services.AddTransient<EmailService>();
  // Token Lifetime:
  builder.Services.AddTransient<TokenService>();
  // .AddTransient();  // gera uma nova instância do objeto a cada chamada do [FromService]
  // .AddScoped();     // disponível por todo escopo do método que chamou o [FromService]
  // .AddSingleton();  // cria uma instância do objeto na execução da aplicação.
  
}

