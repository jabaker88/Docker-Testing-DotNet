using BlazorApp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<Auth0AuthorizationMessageHandler>();

builder.Services.AddHttpClient("Gateway",
      client => client.BaseAddress = new Uri($"{builder.Configuration["Auth0:Audience"]}"))
    .AddHttpMessageHandler<Auth0AuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
  .CreateClient("Gateway"));

//add oidc authentication for auth0
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});

//configure logging
//builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

//add appsettings.json from wwwroot
var settings = new AppSettings();
builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

var app = builder.Build();
Config.Init(app.Configuration);

await app.RunAsync();

public class Auth0AuthorizationMessageHandler : AuthorizationMessageHandler
{
    public Auth0AuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation)
        : base(provider, navigation)
    {
        ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:5003" }//,
            //scopes: new[] { "openid", "profile", "email" }
        );
    }
}