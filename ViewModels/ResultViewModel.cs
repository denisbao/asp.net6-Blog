namespace Blog.ViewModels
{
  public class ResultViewModel<T>
  {

    // Retorno em caso de sucesso:
    public ResultViewModel(T data)
    {
      Data = data;
    }

    // Retorno em caso de vários erros:
    public ResultViewModel(List<string> errors)
    {
      Errors = errors;
    }

    // Retorno em caso de apenas um erro:
    public ResultViewModel(string error)
    {
      Errors.Add(error);
    }

    // Retorno em caso de sucesso e erros:
    public ResultViewModel(T data, List<string> errors)
    {
      Data = data;
      Errors = errors;
    }

    public T Data { get; private set; }
    public List<string> Errors { get; private set; } = new(); //C#10

  }
}