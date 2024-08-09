namespace GameStore.api.Endpoints;

using System.Data.Common;
using GameStore.api.Data;
using GameStore.api.Dtos;
using GameStore.api.Entities;
using GameStore.api.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

public static class GamesEndpoints 
{
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
                       .WithParameterValidation();

        const string GetGameEndpointName = "GetGame";
        // GET /games 
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                     .Include(game => game.Genre)
                     .Select(game => game.ToDto())
                     .AsNoTracking()
                     .ToListAsync());

        // GET /games/id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {
            Game? game = await dbContext.Games.FindAsync(id);
            return game is null ? 
                Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
        }).WithName(GetGameEndpointName);

        // POST  /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => 
        {

            Game game = newGame.ToEntity();


            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game.ToGameDetailsDto());
        });


        // PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = await dbContext.Games.FindAsync(id);



            if (existingGame is null) 
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                     .CurrentValues
                     .SetValues(updatedGame.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {

            await dbContext.Games.Where(game => game.Id == id)
                           .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        return group; 
    }

}