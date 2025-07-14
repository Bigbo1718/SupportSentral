using SupportSentral.Api.Data;
using SupportSentral.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SupportSentral");

builder.Services.AddSqlite<SupportContext>(connectionString);

var app = builder.Build();


app.MapToUserEndpoint();
app.MapToTicketEndpoint();
await app.MigrateDbAsync();

app.Run();
