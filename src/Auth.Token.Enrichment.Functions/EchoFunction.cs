using Auth.Token.Enrichment.Functions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Auth.Token.Enrichment.Functions;

public class EchoFunction(ILogger<EchoFunction> logger)
{
    private static readonly Action<ILogger, Exception?> _logEchoFunctionTrigger =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(0, nameof(EchoFunction)),
            "EchoFunction triggered");

    [Function("EchoFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function,
                    "get",
                    Route = "echo/{message}")]
        HttpRequest _,
        string message)
    {
        _logEchoFunctionTrigger(logger, null);

        return new OkObjectResult(message);
    }
}