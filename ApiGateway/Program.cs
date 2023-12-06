using ApiGateway.Aggregator;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

//builder.Logging.AddConsole();
//builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();

builder.Services
    .AddOcelot(builder.Configuration)
    .AddSingletonDefinedAggregator<DatabaseWeatherAggregator>();

builder.Services.AddSwaggerForOcelot(builder.Configuration, options =>
{
    options.GenerateDocsForAggregates = true;
});

//enable CORS for development
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

if (app.Environment.IsDevelopment())
{
    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
}

await app.UseOcelot();

app.Run();
