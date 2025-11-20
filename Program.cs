using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // This is already added, just make sure it's here

builder.Services.AddSingleton<QuizDataService>();
builder.Services.AddSingleton<QuizCategoriesService>(sp =>
    new QuizCategoriesService(
        sp.GetRequiredService<QuizDataService>(),
        sp.GetRequiredService<ILogger<QuizCategoriesService>>())); // Add ILogger<QuizCategoriesService> here

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enables Swagger endpoint
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1"); 
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("DevPolicy");
app.MapControllers();

app.Run();
