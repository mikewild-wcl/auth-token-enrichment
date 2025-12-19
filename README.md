# auth-token-enrichment
Token enrichment for Entra ID Azure B2C using a function app to read a database

## DevTunnel

A dev tunnel has been added to the app host to make it easy to test. See [Aspire dev tunnels integration](https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/dev-tunnels-integration?tabs=dotnet-cli).

To use dev tunnels you need to install the cli tool – see [Create and host a tunnel](https://learn.microsoft.com/en-gb/azure/developer/dev-tunnels/get-started?tabs=windows#install).

Make sure the devtunnel installation is on your path. Run this if necessary, noting that  – the last part will be different on different installations:

```
setx PATH "%PATH%;C:\Users\<user-name>\AppData\Local\Microsoft\WinGet\Packages\Microsoft.devtunnel_Microsoft.Winget.Source_8wekyb3d8bbwe"
```
