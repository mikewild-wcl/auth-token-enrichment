var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Auth_Token_Enrichment_AzureB2cWebApp>("auth-token-enrichment-azureb2cwebapp");

builder.AddAzureFunctionsProject<Projects.Auth_Token_Enrichment_Functions>("auth-token-enrichment-functions");

await builder.Build().RunAsync();
