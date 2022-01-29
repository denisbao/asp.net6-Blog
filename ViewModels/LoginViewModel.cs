using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
  public class LoginViewModel
  {
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Informe a senha")]
    public string Password { get; set; }
  }
}