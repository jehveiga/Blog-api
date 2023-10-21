namespace Blog.ViewModels
{
    // Classe para ser usada para tranferência de dados entre o modelo que chega da requisição e também o modelo de resposta da controllers
    public record EditorCategoryViewModel(string Name, string Slug);
}
