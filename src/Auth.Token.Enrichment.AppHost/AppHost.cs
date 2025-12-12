using Aspire.Hosting.DevTunnels;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Auth_Token_Enrichment_AzureB2cWebApp>("auth-token-enrichment-azureb2cwebapp");

var functions = builder.AddAzureFunctionsProject<Projects.Auth_Token_Enrichment_Functions>("auth-token-enrichment-functions");

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

await builder.Build().RunAsync();
