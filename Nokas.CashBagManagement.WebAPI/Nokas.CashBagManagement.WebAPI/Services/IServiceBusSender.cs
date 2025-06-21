namespace Nokas.CashBagManagement.WebAPI.Services
{
    public interface IServiceBusSender
    {
        Task SendMessageAsync(string messageBody, string subject);
    }
}
