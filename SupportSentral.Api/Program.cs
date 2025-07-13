using SupportSentral.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("SupportCentral");


app.MapToUserEndpoint();

app.Run();
