using System.Text.RegularExpressions;
using GameStore.Api.Db;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GameEndpoints
    {
        public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("games")
                .WithParameterValidation();
            const string getGamesEndpointname = "Get Game";


            // GET all games
            group.MapGet("/", async (GameStoreContext dbContext) => 
               await dbContext.Games
                          .Include(game => game.Genre)
                          .Select(game => game.toGameSummaryDto())
                          .AsNoTracking()
                          .ToListAsync()
            );


            // GET games/1
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {
                Game? found = await dbContext.Games.FindAsync(id);
                return found is null ?
                Results.NotFound(new { Message = $"Game with id {id} not found." })
                                    : Results.Ok(found.toGameDetailsDto());
            })
                .WithName(getGamesEndpointname);


            // POST games
            group.MapPost("/", async ([FromBody] CreateGameDto createdGame, GameStoreContext dbContext) =>
            {
                if (!await dbContext.Genres.AnyAsync(g => g.Id == createdGame.GenreId))
                {
                    return Results.BadRequest(new { Message = $"Invalid GenreId: {createdGame.GenreId}" });
                }
                Game game = createdGame.toEntity();
                game.Genre = dbContext.Genres.Find(createdGame.GenreId);
               
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                
                return Results.CreatedAtRoute(getGamesEndpointname,
                    new { id = game.Id },
                    game.toGameDetailsDto()
                    );

            });

            //Update games/1
            group.MapPut("/{id}", async (int id, [FromBody] UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                Game? existingGame = await dbContext.Games.FindAsync(id);
                if (existingGame is null)
                {
                    return Results.NotFound(new { Message = $"Game with id {id} not found." });
                }

                dbContext.Entry(existingGame)
                         .CurrentValues
                         .SetValues(updatedGame.toEntity(id));
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });

            //Delete games/1
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
              await  dbContext.Games
                    .Where(game => game.Id == id)
                    .ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            });
            return group;
        }
    }
}
