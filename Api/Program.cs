using Microsoft.EntityFrameworkCore;
using Api;
using Api.Data;
using Api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
    .ConfigureOptions<DatabaseOptionsSetup>()
    .AddDbContext<DataContext>();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Create the database if it doesn't exist and apply any pending migration.
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.Migrate();
}

app.UseHttpsRedirection();

// Map the RPG endpoints
app.MapRpgEndpoints();

app.Run();