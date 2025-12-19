using Aspire.Hosting.Azure;

var builder = DistributedApplication.CreateBuilder(args);

//This only works if an Azure subscription is present

var b2cWebApp = builder.AddProject<Projects.Auth_Token_Enrichment_AzureB2cWebApp>("auth-token-enrichment-azureb2cwebapp");

var functions = builder.AddAzureFunctionsProject<Projects.Auth_Token_Enrichment_Functions>("auth-token-enrichment-functions");

//var appInsights = builder.AddAzureApplicationInsights("auth-token-enrichment-app-insights");
//b2cWebApp.WithReference(appInsights);
//functions.WithReference(appInsights);

/*
// Add your Azure Function App
var functionApp = builder.AddAzureFunctionApp("my-function-app")
    .WithRuntime("dotnet-isolated");

// Add Application Insights
var appInsights = builder.AddAzureApplicationInsights("my-appinsights");

// Link Function App to Application Insights
functionApp.WithReference(appInsights);
*/
//https://learn.microsoft.com/en-us/dotnet/aspire/deployment/aspire-deploy/application-insights
// Provision an Application Insights resource
//nuget - Aspire.Hosting.Azure.ApplicationInsights

if (!builder.ExecutionContext.IsPublishMode)
{
    builder.AddDevTunnel("public-rest-api")
       .WithReference(functions)
       .WithAnonymousAccess();

    //builder.AddDevTunnel(
    //        name: "fun",
    //        tunnelId: "auth-extension",
    //        options: new DevTunnelOptions
    //        {
    //            AllowAnonymous = true,
    //            Description = "Function app tunnel for REST access",
    //            Labels = ["auth", "fun"]
    //        }
    //    )
    //    .WithReference(functions);
}

await builder.Build().RunAsync();
