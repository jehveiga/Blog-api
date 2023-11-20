using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

namespace Blog.Extensions
{
    public static class AppExtensions
    {
        public static void LoadConfiguration(this WebApplicationBuilder builder)
        {
            // Obtendo as informações da configuração da aplicação do appsettings.json
            Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
            Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
            Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");

            // Obtendo as informações da configuração da aplicação do appsettings.json do serviço de Smtp
            var smtp = new Configuration.SmtpConfiguration();
            builder.Configuration.GetSection("Smtp").Bind(smtp); // Efetua o mapeamento dos valores da seção para o objeto criado da classe do Smtp
            Configuration.Smtp = smtp;
        }

        public static void ConfigureAuthentication(this WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // Obtendo a chave JWT configurada

            // Confirando o esquema que será usado na autenticação do usuário do sistema
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Configuração do esquema da autenticação
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Configuração do desafio da autenticação
            }).AddJwtBearer(x => // Informando o tipo usado da autenticação como será lidado com o token obtido
            {
                x.TokenValidationParameters = new TokenValidationParameters // Configurando como será encriptado e desecriptado o token e etc
                {
                    ValidateIssuerSigningKey = true, // Validar a chave da assinatura
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Como será validado a chave desta assinatura
                    ValidateIssuer = false, // Se usar o serviço de multiplas API
                    ValidateAudience = false, // Se usar o serviço de multiplas API
                };

            });
        }

        public static void ConfigureMvc(this WebApplicationBuilder builder)
        {
            builder.Services.AddMemoryCache(); // Adicionando suporte ao serviço de cache para aplicação

            // Adicionando o serviço de compressão de dados para o response
            builder.Services.AddResponseCompression(options =>
            {
                //options.Providers.Add<BrotliCompressionProvider>(); (outro tipo de compressão)
                options.Providers.Add<GzipCompressionProvider>();
            });


            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal; // Configurar o nivel de compressão escolhido
            });

            builder
               .Services
               .AddControllers()
               .ConfigureApiBehaviorOptions(options =>
               {
                   options.SuppressModelStateInvalidFilter = true; // Definir que o comportamento de validação dos modelos das requições não será feito automático pelos Asp.Net
               })
               .AddJsonOptions(jsonOptions =>
               {
                   // Manipulador de referência para ser serializado, irá fazer ser ignorado os ciclos subsequentes do objeto só descendo ao primeiro nó do objeto requerido
                   jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                   // Quando houver um objeto nulo náo irá renderizar o objeto na tela
                   jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
               });
        }

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obter a connection string configuradas pelo nome configurado
            builder.Services.AddDbContext<BlogDataContext>(options =>
                options.UseSqlServer(connectionString)); // adicionando o serviço de injeção de dependência do Contexto que representará o banco na aplicação

            builder.Services.AddTransient<TokenService>();
            builder.Services.AddTransient<EmailService>();
        }
    }
}
