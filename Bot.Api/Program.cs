using Bot.Api.Controllers;
using Bot.Api.Services;
using Bot.Application;
using Bot.Application.Models;
using Bot.Infrastructure;
using Bot.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ApplicationLayerServices(builder.Configuration);
builder.Services.InfrastructureLayerServices(builder.Configuration);
builder.Services.WebApiLayerServices(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    var controllerName = typeof(BotController).Name.Replace("Controller", "", StringComparison.Ordinal);
    var actionName = typeof(BotController).GetMethods()[0].Name;

    string? pattern = app.Configuration.GetSection(BotConfiguration.RouteSection).Value;

    endpoints.MapControllerRoute(
            name: "e-commerce",
            pattern: pattern ?? "/api/bot",
            defaults: new { controller = controllerName, action = actionName });

    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    // Apply all pending migrations
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
