using Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.LoadConfiguration();
builder.ConfigureAuthentication();
builder.ConfigureMvc();
builder.ConfigureServices();

builder.Services.AddEndpointsApiExplorer(); // Adicionar serviço do Swagger a aplicação
builder.Services.AddSwaggerGen(); // Responsável por gerar o código da interface do Swagger

// Add service Data Base Context

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Habilitando as configurações do pipeline em modo de desenvolvimento
{
    app.UseSwagger(); // Se estiver no ambiente referido acima estará executando o serviço do Swagger
    app.UseSwaggerUI(); // Informando para ser usado o serviço da interface do Swagger
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Adicionar o Middleware de autenticação - Quem é?
app.UseAuthorization(); // Adicionar o Middleware de autorização - Oque pode fazer? Onde tem acesso nos endpoints da API

// Adicionando a compressão ao pipeline da aplicação
app.UseResponseCompression();

app.UseStaticFiles();

app.MapControllers();

app.Run();

