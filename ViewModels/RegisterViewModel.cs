using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
  public class RegisterViewModel
  {
    [Required(ErrorMessage = "O Nome é obrigatório.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O E-mail informado é inválido.")]
    public string Email { get; set; }

  }
}