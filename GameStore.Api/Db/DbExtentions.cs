using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Db
{
    public static class DbExtentions
    {
        public static async Task MigrateDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            Console.WriteLine("Migrating database...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Migration complete.");
        }
    }
}
