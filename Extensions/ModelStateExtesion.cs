using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
  public static class ModelStateExtension
  {
    // Extension Method para o ModelState que cria uma lista de strings com
    // os erros apontados pela validação executada pelo ModelState.
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
      var result = new List<string>();

      // Selecionando as mensagens de erro dentro da estrutura JSON do ModelState:
      foreach (var item in modelState.Values)
      {
        // Expression LINQ:
        result.AddRange(item.Errors.Select(error => error.ErrorMessage));

        // Equivalente ao Expression LINQ:
        // foreach (var error in item.Errors)
        // {
        //   result.Add(error.ErrorMessage);
        // }
      }
      return result;
    }
  }
}