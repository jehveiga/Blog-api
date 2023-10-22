using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    // Classe para ser usada para tranferência de dados entre o modelo que chega da requisição e também o modelo de resposta da controllers
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O slug é obrigatório")]
        public string Slug { get; set; }
    }
}
