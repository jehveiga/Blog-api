namespace Blog.ViewModels
{
    // Classe genérica para informar o resultado da requisição da API para ter uma padronização
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data, List<string> errors) // Recebe os dados e a listagem contendo os erros da requisição
        {
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data) // Recebe os dados no caso de sucesso
        {
            Data = data;
        }

        public ResultViewModel(List<string> errors) // Recebe a listagem contendo os erros da requisição
        {
            Errors = errors;
        }

        public ResultViewModel(string erro) // Recebe um erro contendo o erro da requisição e adicionar a listagem de erros
        {
            Errors.Add(erro);
        }

        public T Data { get; private set; }
        public List<string> Errors { get; private set; } = new();

    }
}
