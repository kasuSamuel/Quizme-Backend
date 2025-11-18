using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()          // remove this line later and put exact URLs
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===========================================================

var app = builder.Build();

// ==================== MIDDLEWARE ORDER (IMPORTANT) ====================

// Swagger works in BOTH dev and production now
app.UseSwagger();
app.UseSwaggerUI();

// CORS — MUST be before MapControllers()
app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.MapControllers();

// ====================================================================

app.Run();
