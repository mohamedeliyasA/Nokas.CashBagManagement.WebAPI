using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using Nokas.CashBagManagement.WebAPI.Helpers;

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
            var correlationId = context.Request.Headers["X-Correlation-ID"].ToString();
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

                var isDev = _env.IsDevelopment();
                int statusCode;
                string title;

                if (ex is DuplicateBagNumberException duplicateEx)
                {
                    statusCode = (int)HttpStatusCode.Conflict;
                    title = duplicateEx.Message;

                    _logger.LogWarning("CorrelationId: {CorrelationId} - Duplicate bag number error: {Message}", correlationId, title);
                }
                else if (ex is DbUpdateException dbEx)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    var raw = dbEx.GetBaseException()?.Message ?? dbEx.Message;
                    string field = null;

                    if (raw.Contains("String or binary data would be truncated"))
                    {
                        var match = Regex.Match(raw, @"column '(?<column>[^']+)'");
                        if (match.Success)
                            field = match.Groups["column"].Value?.Split('_').LastOrDefault();

                        title = field != null
                            ? $"The field '{field}' exceeds the allowed length."
                            : "One of the values is too long for the database field.";
                    }
                    else if (raw.Contains("FOREIGN KEY constraint"))
                    {
                        var match = Regex.Match(raw, @"FOREIGN KEY constraint ""(?<constraint>[^""]+)""");
                        var constraint = match.Success ? match.Groups["constraint"].Value : null;
                        title = constraint != null
                            ? $"Invalid reference related to constraint '{constraint}'."
                            : "Invalid reference. A related record does not exist.";
                    }
                    else if (raw.Contains("Cannot insert the value NULL into column"))
                    {
                        var match = Regex.Match(raw, @"column '(?<column>[^']+)'");
                        field = match.Success ? match.Groups["column"].Value?.Split('_').LastOrDefault() : null;

                        title = field != null
                            ? $"Missing required value for '{field}'."
                            : "Missing required field.";
                    }
                    else if (raw.Contains("UNIQUE KEY constraint"))
                    {
                        var match = Regex.Match(raw, @"UNIQUE KEY constraint '(?<constraint>[^']+)'");
                        var constraint = match.Success ? match.Groups["constraint"].Value : null;

                        title = constraint != null
                            ? $"Duplicate entry detected. Violates uniqueness constraint '{constraint}'."
                            : "This value must be unique. A record with the same value already exists.";
                    }
                    else if (raw.Contains("CHECK constraint"))
                    {
                        var match = Regex.Match(raw, @"CHECK constraint '(?<constraint>[^']+)'");
                        var constraint = match.Success ? match.Groups["constraint"].Value : null;

                        title = constraint != null
                            ? $"Invalid value. Violates rule '{constraint}'."
                            : "The value provided violates a business rule.";
                    }
                    else
                    {
                        title = "Invalid data submitted. Please check your input.";
                    }

                    _logger.LogWarning(dbEx, "Validation error: {Message}", raw);
                }
                else
                {
                    title = isDev ? "Unhandled exception occurred." : "An unexpected error occurred.";
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    _logger.LogError(ex,
                        "Unhandled exception in Controller: {Controller}, Action: {Action}, URI: {Uri}, CorrelationId: {CorrelationId}",
                        controller, action, fullUrl, correlationId);
                }

                var error = new ErrorResponse
                {
                    Title = title,
                    Status = statusCode,
                    CorrelationId = correlationId,
                    Controller = isDev ? controller : null,
                    Action = isDev ? action : null,
                    Path = isDev ? fullUrl : null,
                    Exception = isDev ? ex.Message : null,
                    StackTrace = isDev ? ex.StackTrace : null
                };

                context.Response.StatusCode = statusCode;

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
                else
                {
                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(error,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    await context.Response.WriteAsync(json);
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
}
