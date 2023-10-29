using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)] // Adicionando atributo para ser usado tanto na classes como nos métodos
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        // Interromperá a requisição alvo e fará uma verificação para dar sequência no próximo fluxo ou invalidar a mesma
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // Tentará buscar na requisição pela query da mesma se contém a Key 'ApiKeyName', se sucesso enviará o valor a varíavel
            if (!context.HttpContext.Request.Query.TryGetValue(Configuration.ApiKeyName, out var extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 401,
                    Content = "ApiKey não encontrada"
                };

                return;
            }

            // Verificará se a Key que está na configuração é a mesma obtida da requisição
            if (!Configuration.ApiKey.Equals(extractedApiKey))
            {
                context.Result = new ContentResult
                {
                    StatusCode = 403,
                    Content = "Acesso não autorizado"
                };

                return;
            }

            // Se passar pelas duas condições retornará para o fluxo do método invocado
            await next();
        }
    }
}
