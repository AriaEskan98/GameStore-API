using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Mapping
{
    public static class GameMapping
    {
        public static Game toEntity(this CreateGameDto game)
        {
            return new Game()
            {
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate

            };
        }
        public static Game toEntity(this UpdateGameDto game, int id)
        {
            return new Game()
            {
                Id = id,
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate

            };
        }
        public static GameSummaryDto toGameSummaryDto(this Game game)
        {
            return new(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate
                    );
        }

        public static GameDetailsDto toGameDetailsDto(this Game game)
        {
            return new(
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate
                    );
        }
    }
}
