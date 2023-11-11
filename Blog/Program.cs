using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

builder.Services.AddEndpointsApiExplorer(); // Adicionar servi�o do Swagger a aplica��o
builder.Services.AddSwaggerGen(); // Respons�vel por gerar o c�digo da interface do Swagger

// Add service Data Base Context

var app = builder.Build();
LoadConfiguration(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Habilitando as configura��es do pipeline em modo de desenvolvimento
{
    app.UseSwagger(); // Se estiver no ambiente referido acima estar� executando o servi�o do Swagger
    app.UseSwaggerUI(); // Informando para ser usado o servi�o da interface do Swagger
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Adicionando a compress�o ao pipeline da aplica��o
app.UseResponseCompression();

app.UseStaticFiles();

app.MapControllers();

app.Run();

void LoadConfiguration(WebApplication app)
{
    // Obtendo as informa��es da configura��o da aplica��o do appsettings.json
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

    // Obtendo as informa��es da configura��o da aplica��o do appsettings.json do servi�o de Smtp
    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp); // Efetua o mapeamento dos valores da se��o para o objeto criado da classe do Smtp
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // Obtendo a chave JWT configurada

    // Confirando o esquema que ser� usado na autentica��o do usu�rio do sistema
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Configura��o do esquema da autentica��o
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Configura��o do desafio da autentica��o
    }).AddJwtBearer(x => // Informando o tipo usado da autentica��o como ser� lidado com o token obtido
    {
        x.TokenValidationParameters = new TokenValidationParameters // Configurando como ser� encriptado e desecriptado o token e etc
        {
            ValidateIssuerSigningKey = true, // Validar a chave da assinatura
            IssuerSigningKey = new SymmetricSecurityKey(key), // Como ser� validado a chave desta assinatura
            ValidateIssuer = false, // Se usar o servi�o de multiplas API
            ValidateAudience = false, // Se usar o servi�o de multiplas API
        };

    });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache(); // Adicionando suporte ao servi�o de cache para aplica��o

    // Adicionando o servi�o de compress�o de dados para o response
    builder.Services.AddResponseCompression(options =>
    {
        //options.Providers.Add<BrotliCompressionProvider>(); (outro tipo de compress�o)
        options.Providers.Add<GzipCompressionProvider>();
    });


    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Optimal; // Configurar o nivel de compress�o escolhido
    });

    builder
       .Services
       .AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.SuppressModelStateInvalidFilter = true; // Definir que o comportamento de valida��o dos modelos das requi��es n�o ser� feito autom�tico pelos Asp.Net
       })
       .AddJsonOptions(jsonOptions =>
       {
           // Manipulador de refer�ncia para ser serializado, ir� fazer ser ignorado os ciclos subsequentes do objeto s� descendo ao primeiro n� do objeto requerido
           jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
           // Quando houver um objeto nulo n�o ir� renderizar o objeto na tela
           jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
       });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obter a connection string configuradas pelo nome configurado
    builder.Services.AddDbContext<BlogDataContext>(options =>
        options.UseSqlServer(connectionString)); // adicionando o servi�o de inje��o de depend�ncia do Contexto que representar� o banco na aplica��o

    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<EmailService>();
}
