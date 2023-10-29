using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // Obtendo a chave JWT configurada

// Add services to the container.

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
