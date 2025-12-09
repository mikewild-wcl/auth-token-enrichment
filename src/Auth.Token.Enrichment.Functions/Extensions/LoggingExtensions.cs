using Microsoft.Extensions.Logging;

namespace Auth.Token.Enrichment.Functions.Extensions;

internal static partial class LoggingExtensions
{
    //extension(ILogger logger)
    //{
    //    [LoggerMessage(
    //        EventId = 1,
    //        Level = LogLevel.Information,
    //        Message = "Hello, world from {Class}!"
    //    )]
    //    public static partial void Hello(string @class);

    //    [LoggerMessage(
    //        EventId = 3,
    //        Level = LogLevel.Information,
    //        Message = "Empty request or reguest with no body was received and ignored")]
    //    public static partial void LogEmptyRequest();
    //}

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Hello, world from {Class}!"
    )]
    public static partial void Hello(this ILogger logger, string @class);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Information,
        Message = "Empty request or reguest with no body was received and ignored")]
    public static partial void LogEmptyRequest(this ILogger logger);
}