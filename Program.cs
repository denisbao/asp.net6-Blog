using Blog.Data;
using Blog.Services;

var builder = WebApplication.CreateBuilder(args);
builder
  .Services
  .AddControllers()
  .ConfigureApiBehaviorOptions(options =>
  {
    options.SuppressModelStateInvalidFilter = true;
  });
builder.Services.AddDbContext<BlogDataContext>();

// TOKEN SERVICE LIFETIME:
// .AddTransient();  // gera uma nova instância do objeto a cada chamada do [FromService]
// .AddScoped();     // disponível por todo escopo do método que chamou o [FromService]
// .AddSingleton();  // cria uma instância do objeto na execução da aplicação.
builder.Services.AddTransient<TokenService>();

var app = builder.Build();
app.MapControllers();


app.Run();
