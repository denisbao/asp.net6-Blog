using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
  public class EditorTagViewModel
  {
    [Required(ErrorMessage = "O campo nome é obrigarório.")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "O campo deve ter entre 3 e 40 caracteres.")]
    public string Name { get; set; }


    [Required(ErrorMessage = "O campo slug é obrigarório.")]
    public string Slug { get; set; }

  }
}