using System.Text;

namespace Nokas.CashBagManagement.WebAPI.Helpers
{
    public static class ArchiveHelper
    {
        public static async Task<string> ReadRawBodyAsStringAsync(this HttpRequest request)
        {
            request.EnableBuffering(); 
            request.Body.Position = 0; 

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;
            return body;
        }
    }
}
