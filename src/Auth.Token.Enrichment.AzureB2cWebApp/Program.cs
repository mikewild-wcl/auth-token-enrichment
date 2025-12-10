using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    // https://stackoverflow.com/questions/50262561/correlation-failed-in-net-core-asp-net-identity-openid-connect
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services
    .AddMicrosoftIdentityWebAppAuthentication(
    builder.Configuration,
    "AzureAdB2C");

builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme,
    options =>
    {
        var b2cConfogSection = builder.Configuration.GetRequiredSection("AzureAdB2C");
        var clientId = b2cConfogSection.GetValue<string>("ClientId");
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add(clientId);
    });

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

app.MapDefaultEndpoints();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Next line needed to get sign in and out UI to work
// https://stackoverflow.com/questions/65551895/asp-net-core-azuread-authentication-works-but-the-signin-and-signout-dont
app.MapControllers();

await app.RunAsync();
