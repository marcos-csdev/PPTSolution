
public class RouteMapper
{
    private readonly Serilog.ILogger _logger;
    public RouteMapper(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    public IResult MapRoutes(WebApplication app)
    {
        try
        {
            app.MapGet("/avatar", (string userIdentifier, IImageFetcherService imageFetcherService) =>
            {
                if (string.IsNullOrEmpty(userIdentifier))
                    return Results.BadRequest(new { message = "User identifier is required" });

                if (imageFetcherService == null)
                    return Results.Problem("Could not acquire image fetcher service.");

                var imageUrl = imageFetcherService.FetchImage(userIdentifier);

                return Results.Ok(new { url = imageUrl });
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Error mapping routes: {ex.Message}", ex);
        }

        return Results.Problem("An error occurred while processing your request. Please try again later.");
    }
}