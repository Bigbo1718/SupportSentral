using SupportSentral.Api.Data;
using SupportSentral.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("SupportCentral");

builder.Services.AddSqlite<SupportContext>(connectionString);

app.MapToUserEndpoint();

app.Run();
