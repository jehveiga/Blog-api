using Blog.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
       .AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.SuppressModelStateInvalidFilter = true; // Definir que o comportamento de validação dos modelos das requições não será feito automático pelos Asp.Net
       });

builder.Services.AddDbContext<BlogDataContext>();

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
