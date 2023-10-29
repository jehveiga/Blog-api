using Blog.Data;
using Blog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
       .AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.SuppressModelStateInvalidFilter = true; // Definir que o comportamento de valida��o dos modelos das requi��es n�o ser� feito autom�tico pelos Asp.Net
       });

builder.Services.AddDbContext<BlogDataContext>(); // adicionando o servi�o de inje��o de depend�ncia do Contexto que representar� o banco na aplica��o
builder.Services.AddTransient<TokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add service Data Base Context

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
