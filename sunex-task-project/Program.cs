using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using sunex_task_project.AppDataContext;
using sunex_task_project.Data;
using sunex_task_project.Hubs;
using sunex_task_project.Middleware;
using sunex_task_project.Services;

var builder = WebApplication.CreateBuilder(args);

// Injections
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TaskDb"));
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskHubClient, TaskHubClient>();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3001")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseHttpsRedirection();

// Use Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root of the application
    });
}
app.UseCors();
app.UseRouting();
// For Custom exceptions. Handle exceptions with middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.MapHub<TaskHub>("/taskHub");
app.Run();
