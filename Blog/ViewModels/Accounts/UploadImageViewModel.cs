using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts
{
    // Classe usada para receber/enviar o envio da imagem pelo/para front poderia ser recebido em Array de byte[]
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem inválida")]
        public string Base64Image { get; set; }
    }
}
