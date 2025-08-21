using Microsoft.OpenApi.Models;
using TaskManagement.API;
using TaskManagement.Application;
using TaskManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "A simple task management API demonstrating Clean Architecture with DDD and CQRS"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();