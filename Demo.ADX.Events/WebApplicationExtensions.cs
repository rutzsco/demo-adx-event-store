using Microsoft.AspNetCore.Mvc;

namespace Demo.ADX.Events
{
    public static class WebApplicationExtensions
    {

        internal static WebApplication MapApi(this WebApplication app)
        {
            var api = app.MapGroup("api");
            api.MapPost("log/direct", SendDirectLog);
            api.MapPost("log/eh", SendEventHubLog);
            api.MapGet("log/{keyword}", QueryADX);
            return app;
        }

        private static async Task<IResult> SendEventHubLog(LogMessage LogMessage, CancellationToken cancellationToken)
        {
            await EventHubLogger.SendMessageToEventHubAsync(LogMessage.message);
            return Results.NoContent();
        }
        private static async Task<IResult> SendDirectLog(LogMessage LogMessage, CancellationToken cancellationToken)
        {
            NearRealTimeLogger.LogEventInline(LogMessage.message);
            return Results.NoContent();
        }

        private static async Task<IResult> QueryADX(string keyword, CancellationToken cancellationToken)
        {
            var result = QueryClient.SearchLogs(keyword);
            return Results.Ok(result);
        }
    }
}
