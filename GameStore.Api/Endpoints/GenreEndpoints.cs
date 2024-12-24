using System.Runtime.CompilerServices;
using GameStore.Api.Db;
using GameStore.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GenreEndpoints
    {
        public static RouteGroupBuilder MapGenresEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("genres")
                .WithParameterValidation();
            group.MapGet("/", async (GameStoreContext dbContext) =>
                    await dbContext.Genres
                                   .Select(genre => genre.toDto())
                                   .AsNoTracking()
                                   .ToListAsync()
                                   );
            return group;
        }
    }
}
