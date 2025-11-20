using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Quizme API", Version = "v1" });
});

builder.Services.AddSingleton<QuizDataService>();
builder.Services.AddSingleton<QuizCategoriesService>(sp =>
    new QuizCategoriesService(
        sp.GetRequiredService<QuizDataService>(),
        sp.GetRequiredService<ILogger<QuizCategoriesService>>()));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ENABLE SWAGGER IN ALL ENVIRONMENTS (especially Production on Render)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quizme API V1");
    options.RoutePrefix = string.Empty; // Swagger UI loads at root URL
});

app.UseHttpsRedirection();
app.UseCors("DevPolicy");
app.MapControllers();

app.Run();
