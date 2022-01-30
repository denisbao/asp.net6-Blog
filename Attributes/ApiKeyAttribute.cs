using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class ApiKeyAttribute : Attribute, IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      // Requisição sem uma ApiKey:
      if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractedApiKey))
      {
        context.Result = new ContentResult()
        {
          StatusCode = 401,
          Content = "COD036: ApiKey não encontrada."
        };
        return;
      }

      // Requisição com uma ApiKey inválida:
      if (!Configuration.ApiKey.Equals(extractedApiKey))
      {
        context.Result = new ContentResult()
        {
          StatusCode = 403,
          Content = "COD037 = Acesso não autorizado."
        };
        return;
      }

      await next();

    }
  }
}