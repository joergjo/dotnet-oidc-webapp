using AadClaimsViewer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        var reverseProxyBaseUri = builder.Configuration["ReverseProxyBaseUri"];
        if (!string.IsNullOrEmpty(reverseProxyBaseUri))
        {
            options.Events ??= new OpenIdConnectEvents();
            options.Events.OnRedirectToIdentityProvider += context =>
            {
                var uriBuilder = new UriBuilder(reverseProxyBaseUri)
                {
                    Path = options.CallbackPath
                };
                context.ProtocolMessage.RedirectUri = uriBuilder.ToString();
                return Task.CompletedTask;
            };
        }
    });

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy.
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Only loopback proxies are allowed by default. Clear that restriction because forwarders are
    // being enabled by explicit configuration.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddHealthChecks().AddCheck(
    "liveness_readiness",
    () => HealthCheckResult.Healthy("OK"),
    new string[] { });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseForwardedHeaders();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

int probePort = builder.Configuration.GetValue<int>("Health:ProbePort", 0);
app.MapProbes(probePort);
app.MapRazorPages();
app.MapControllers();

app.Run();
