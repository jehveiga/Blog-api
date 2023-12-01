using Blog.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.LoadConfiguration();
builder.ConfigureAuthentication();
builder.ConfigureMvc();
builder.ConfigureServices();

builder.Services.AddEndpointsApiExplorer(); // Adicionar servi�o do Swagger a aplica��o
builder.Services.AddSwaggerGen(); // Respons�vel por gerar o c�digo da interface do Swagger

// Add service Data Base Context

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Habilitando as configura��es do pipeline em modo de desenvolvimento
{
    app.UseSwagger(); // Se estiver no ambiente referido acima estar� executando o servi�o do Swagger
    app.UseSwaggerUI(); // Informando para ser usado o servi�o da interface do Swagger
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Adicionar o Middleware de autentica��o - Quem �?
app.UseAuthorization(); // Adicionar o Middleware de autoriza��o - Oque pode fazer? Onde tem acesso nos endpoints da API

// Adicionando a compress�o ao pipeline da aplica��o
app.UseResponseCompression();

app.UseStaticFiles();

app.MapControllers();

app.Run();

