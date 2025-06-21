using System.Text;

namespace Nokas.CashBagManagement.WebAPI.Helpers
{
    public static class ArchiveHelper
    {
        public static async Task<string> ReadRawBodyAsStringAsync(this HttpRequest request)
        {
            request.EnableBuffering(); // Allows the body to be read multiple times
            request.Body.Position = 0; // Reset position before reading

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0; // Reset again so ASP.NET can re-read it (for model binding)
            return body;
        }
    }
}
