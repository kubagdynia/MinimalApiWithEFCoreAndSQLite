using Microsoft.EntityFrameworkCore;
using Api;
using Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options => options.CustomSchemaIds(t => t.FullName?.Replace('+', '.')))
    .AddDbContext<DataContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    
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

//
// Endpoint routing
//

// CRUD operations for RpgCharacter

var rpgCharacterRoute = app.MapGroup("api/rpgCharacters").WithTags("RpgCharacters");

// POST /api/rpgCharacters
rpgCharacterRoute.MapPost("", async (DataContext dataContext, RpgCharacter rpgCharacter) =>
{
    dataContext.RpgCharacters.Add(rpgCharacter);
    await dataContext.SaveChangesAsync();
    return Results.Created($"/api/rpgCharacters/{rpgCharacter.Id}", rpgCharacter);
})
.WithSummary("Creates a new RpgCharacter");

// GET /api/rpgCharacters
rpgCharacterRoute.MapGet("", async (DataContext dataContext) =>
{
    return Results.Ok(await dataContext.RpgCharacters.ToListAsync());
})
.WithSummary("Gets all RpgCharacters");

// GET /api/rpgCharacters/{id}
rpgCharacterRoute.MapGet("{id:int}", async (DataContext dataContext, int id) =>
{
    var rpgCharacter = await dataContext.RpgCharacters.FindAsync(id);
    
    if (rpgCharacter is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(rpgCharacter);
})
.WithSummary("Gets a specific RpgCharacter");

// PUT /api/rpgCharacters/{id}
rpgCharacterRoute.MapPut("{id:int}", async (DataContext dataContext, int id, RpgCharacter rpgCharacter) =>
{
    if (id != rpgCharacter.Id)
    {
        return Results.BadRequest();
    }

    dataContext.Entry(rpgCharacter).State = EntityState.Modified;

    try
    {
        await dataContext.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (await dataContext.RpgCharacters.FindAsync(id) is null)
        {
            return Results.NotFound();
        }

        throw;
    }

    return Results.NoContent();
})
.WithSummary("Updates a specific RpgCharacter");

// DELETE /api/rpgCharacters/{id}
rpgCharacterRoute.MapDelete("{id:int}", async (DataContext dataContext, int id) =>
{
    var rpgCharacter = await dataContext.RpgCharacters.FindAsync(id);
    if (rpgCharacter is null)
    {
        return Results.NotFound();
    }

    dataContext.RpgCharacters.Remove(rpgCharacter);
    await dataContext.SaveChangesAsync();

    return Results.NoContent();
})
.WithSummary("Deletes a specific RpgCharacter");

app.Run();