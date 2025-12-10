using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    ///https://stackoverflow.com/questions/50262561/correlation-failed-in-net-core-asp-net-identity-openid-connect
    //options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;

    // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
    //options.HandleSameSiteCookieCompatibility();
});

//Debugging
// Check config
var b2cConfogSection = builder.Configuration.GetRequiredSection("AzureAdB2C");
var clientId = b2cConfogSection.GetValue<string>("ClientId");

// Configuration to sign-in users with Azure AD B2C
//builder.Services
//    .AddMicrosoftIdentityWebAppAuthentication(
//    builder.Configuration, 
//    "AzureAdB2C");
///From another working project...
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.GetSection("AzureAdB2C").Bind(options);

        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add(options.ClientId);

        options.ErrorPath = "/error";
        //options.ClaimActions.Add(new CorrelationClaimAction());
        options.Events.OnRemoteFailure = context =>
        {
            // Workaround to preserve invitation link if user cancels creating account in Azure AD B2C
            if (context.Failure.Message.Contains("AADB2C90091:", StringComparison.Ordinal) //&&
                                                                                           //context.Request.PathBase.HasValue
                                                                                           //? context.Properties.RedirectUri.StartsWith($"{context.Request.PathBase}/{PagePath.Invitation}/")
                                                                                           //: context.Properties.RedirectUri.StartsWith($"/{PagePath.Invitation}/")
                                                                )
            {
                context.Response.Redirect(context.Properties.RedirectUri);
            }

            return Task.CompletedTask;
        };
    }, options =>
    {
        //options.Cookie.Name = configuration.GetValue<string>("CookieOptions:AuthenticationCookieName");
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.GetValue<int>("CookieOptions:AuthenticationExpiryInMinutes"));
        options.SlidingExpiration = true;
        options.Cookie.Path = "/";
        //options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.HttpOnly = true;
    })
    .EnableTokenAcquisitionToCallDownstreamApi([]) // new string[] {configuration.GetValue<string>("FacadeAPI:DownstreamScope")})
    .AddDistributedTokenCaches()
    ;
// end

builder.Services.Configure<OpenIdConnectOptions>(
    OpenIdConnectDefaults.AuthenticationScheme,
    options =>
    {
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add(clientId);
    });

//builder.Services.RegisterWebComponents(builder.Configuration);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    //var forwardedHeadersOptions = configuration
    //    .GetSection("ForwardedHeaders")
    //    .Get<ForwardedHeadersOptions>();
    /*
          "ForwardedHeaders": {
            "ForwardedHostHeaderName": "X-Original-Host",
            "OriginalHostHeaderName": "X-Initial-Host",
            "AllowedHosts": "localhost"
          }
     */
    //var allowedHosts = configuration.GetValue<string>("ForwardedHeaders:AllowedHosts");

    options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
    //options.ForwardedHostHeaderName = forwardedHeadersOptions.ForwardedHostHeaderName;
    //options.OriginalHostHeaderName = forwardedHeadersOptions.OriginalHostHeaderName;
    options.ForwardedHostHeaderName = "X-Original-Host";
    options.OriginalHostHeaderName = "X-Initial-Host";

    //options.AllowedHosts = forwardedHeadersOptions.AllowedHosts;
    options.AllowedHosts = ["localhost", "localhost:7112"];
});

/////

//builder.Services.AddControllersWithViews()
//    .AddMicrosoftIdentityUI();

// Add services to the container.
builder.Services.AddRazorPages()
    //Add authentication
    //.AddMvcOptions(options =>
     //{
     //    var policy = new AuthorizationPolicyBuilder()
     //        .RequireAuthenticatedUser()
     //        .Build();
     //    options.Filters.Add(new AuthorizeFilter(policy));
     //})
    //Add UI for login and logout
    .AddMicrosoftIdentityUI()
    ;

var app = builder.Build();

// Add forwarded headers to see if this helps with login
app.UseForwardedHeaders();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Add the Microsoft Identity Web cookie policy
app.UseCookiePolicy();

app.UseRouting();

// Add the ASP.NET Core authentication service
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Next line needed to get sign in and out UI to work
// https://stackoverflow.com/questions/65551895/asp-net-core-azuread-authentication-works-but-the-signin-and-signout-dont
app.MapControllers();

await app.RunAsync();
