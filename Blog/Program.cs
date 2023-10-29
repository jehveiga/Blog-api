using Blog;
using Blog.Data;
using Blog.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // Obtendo a chave JWT configurada

// Add services to the container.

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

builder.Services
       .AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.SuppressModelStateInvalidFilter = true; // Definir que o comportamento de validação dos modelos das requições não será feito automático pelos Asp.Net
       });

builder.Services.AddDbContext<BlogDataContext>(); // adicionando o serviço de injeção de dependência do Contexto que representará o banco na aplicação
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
