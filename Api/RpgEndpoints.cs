using Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class RpgEndpoints
{
    // Map the CRUD operations for RpgCharacter
    public static void MapRpgEndpoints(this IEndpointRouteBuilder app)
    {
        // CRUD operations for RpgCharacter
        var rpgCharacterRoute = app.MapGroup("api/rpgCharacters").WithTags("RpgCharacters");
        
        // POST /api/rpgCharacters
        CreateNewRpgCharacter(rpgCharacterRoute);

        // GET /api/rpgCharacters
        GetAllRpgCharacters(rpgCharacterRoute);

        // GET /api/rpgCharacters/{id}
        GetRpgCharacter(rpgCharacterRoute);

        // PUT /api/rpgCharacters/{id}
        UpdateRpgCharacter(rpgCharacterRoute);

        // DELETE /api/rpgCharacters/{id}
        DeleteRpgCharacter(rpgCharacterRoute);
    }

    private static void CreateNewRpgCharacter(RouteGroupBuilder rpgCharacterRoute)
    {
        // POST /api/rpgCharacters
        rpgCharacterRoute.MapPost("", async (DataContext dataContext, RpgCharacter rpgCharacter) =>
            {
                dataContext.RpgCharacters.Add(rpgCharacter);
                await dataContext.SaveChangesAsync();
                return Results.Created($"/api/rpgCharacters/{rpgCharacter.Id}", rpgCharacter);
            })
            .WithSummary("Creates a new RpgCharacter")
            .Produces<Created>(StatusCodes.Status201Created);
    }

    private static void GetAllRpgCharacters(RouteGroupBuilder rpgCharacterRoute)
    {
        // GET /api/rpgCharacters
        rpgCharacterRoute.MapGet("",
                async (DataContext dataContext) => Results.Ok(await dataContext.RpgCharacters.ToListAsync()))
            .WithSummary("Gets all RpgCharacters")
            .Produces<Ok>();
    }

    private static void GetRpgCharacter(RouteGroupBuilder rpgCharacterRoute)
    {
        // GET /api/rpgCharacters/{id}
        rpgCharacterRoute.MapGet("{id:int}", async (DataContext dataContext, int id) =>
            {
                var rpgCharacter = await dataContext.RpgCharacters.FindAsync(id);
                return rpgCharacter is null ? Results.NotFound() : Results.Ok(rpgCharacter);
            })
            .WithSummary("Gets a specific RpgCharacter")
            .Produces<Ok>()
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    private static void UpdateRpgCharacter(RouteGroupBuilder rpgCharacterRoute)
    {
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
            .WithSummary("Updates a specific RpgCharacter")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status400BadRequest)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    private static void DeleteRpgCharacter(RouteGroupBuilder rpgCharacterRoute)
    {
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
            .WithSummary("Deletes a specific RpgCharacter")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }
}