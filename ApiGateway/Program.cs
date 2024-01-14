using ApiGateway.Aggregator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

string authority = builder.Configuration["Auth0:Domain"]!;
string audience = builder.Configuration["Auth0:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Auth0Key", options =>
{
    options.Authority = authority;
    options.Audience = audience;
});

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

    //Add bearer token input for swagger UI for Aggregates
    options.GenerateDocsDocsForGatewayItSelf(o => 
    {
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = @"Bearer token",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        o.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
              {
                  new OpenApiSecurityScheme
                  {
                      Reference = new OpenApiReference
                      {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header,
                  },
                  new List<string>()
              }
          });
    });
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

await app.UseAuthentication().UseOcelot();

app.Run();
