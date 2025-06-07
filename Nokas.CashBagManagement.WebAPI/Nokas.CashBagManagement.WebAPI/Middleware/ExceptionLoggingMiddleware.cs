using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace Nokas.CashBagManagement.WebAPI.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionLoggingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            // Step 1: First, check for the client-provided X-Correlation-ID in the request headers
            var correlationId = context.Request.Headers["X-Correlation-ID"].ToString();

            // Step 2: If the client has not provided it, use the internal one stored in HttpContext.Items
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = context.Items["CorrelationId"]?.ToString() ?? context.TraceIdentifier;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var request = context.Request;
                var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
                var routeData = context.GetRouteData();
                var controller = routeData.Values["controller"]?.ToString() ?? "Unknown";
                var action = routeData.Values["action"]?.ToString() ?? "Unknown";

                // Log the error with the correlation ID
                _logger.LogError(ex,
                    "Unhandled exception in Controller: {Controller}, Action: {Action}, URI: {Uri}, CorrelationId: {CorrelationId}",
                    controller, action, fullUrl, correlationId);

                var isDev = _env.IsDevelopment();

                var error = new ErrorResponse
                {
                    Title = isDev ? "Unhandled exception occurred." : "An unexpected error occurred.",
                    Status = (int)HttpStatusCode.InternalServerError,
                    CorrelationId = correlationId,
                    Controller = isDev ? controller : null,
                    Action = isDev ? action : null,
                    Path = isDev ? fullUrl : null,
                    Exception = isDev ? ex.Message : null,
                    StackTrace = isDev ? ex.StackTrace : null
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Determine requested response format (JSON or XML)
                var acceptHeader = context.Request.Headers["Accept"].ToString().ToLower();
                if (acceptHeader.Contains("application/xml"))
                {
                    context.Response.ContentType = "application/xml";
                    var xmlSerializer = new XmlSerializer(typeof(ErrorResponse));
                    await using var ms = new MemoryStream();
                    xmlSerializer.Serialize(ms, error);
                    ms.Position = 0;
                    await ms.CopyToAsync(context.Response.Body);
                }
                else // Default to JSON
                {
                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(error,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}

public class ErrorResponse
{
    public string Title { get; set; }
    public int Status { get; set; }
    public string CorrelationId { get; set; }

    public string? Controller { get; set; }
    public string? Action { get; set; }
    public string? Path { get; set; }
    public string? Exception { get; set; }
    public string? StackTrace { get; set; }
}
