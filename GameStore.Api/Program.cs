using GameStore.Api.Db;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var connString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite <GameStoreContext>(connString);
var app = builder.Build();

app.MapGamesEndpoints();
app.MapGenresEndpoint();


await app.MigrateDbAsync();



app.Run();
