using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    // Classe para ser usada para tranferência de dados para ser o modelo que chega da requisição e transferir os dados para modelo alvo persistido no banco
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O slug é obrigatório")]
        public string Slug { get; set; }
    }
}
