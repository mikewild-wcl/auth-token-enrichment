var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Auth_Token_Enrichment_AzureB2cWebApp>("auth-token-enrichment-azureb2cwebapp");

await builder.Build().RunAsync();
