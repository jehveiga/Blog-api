using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions
{
    public static class ModelStateExtension
    {
        // Método de extensão que retorna a listagem de errosda requisiçào
        public static List<string> GetErrors(this ModelStateDictionary modelState)
        {

            var result = new List<string>();
            foreach (var item in modelState.Values)
                // Adicionando os erros da validação da modelState para uma nova lista de retorno
                result.AddRange(item.Errors.Select(error => error.ErrorMessage));

            return result;
        }
    }
}
