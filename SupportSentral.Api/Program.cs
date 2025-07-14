using SupportSentral.Api.Data;
using SupportSentral.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRepositories(builder.Configuration);

var app = builder.Build();

app.MapToUserEndpoint();
app.MapToTicketEndpoint();
await app.MigrateDbAsync();

app.Run();
