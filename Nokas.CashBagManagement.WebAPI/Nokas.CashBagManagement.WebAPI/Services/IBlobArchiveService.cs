namespace Nokas.CashBagManagement.WebAPI.Services
{
    public interface IBlobArchiveService
    {
        Task ArchivePayloadAsync(string blobName, string content);
    }
}
