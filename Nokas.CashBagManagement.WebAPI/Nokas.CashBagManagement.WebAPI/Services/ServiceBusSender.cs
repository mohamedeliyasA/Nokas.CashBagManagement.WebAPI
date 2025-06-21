using Azure.Messaging.ServiceBus;

namespace Nokas.CashBagManagement.WebAPI.Services
{
    public class ServiceBusSender : IServiceBusSender
    {
        private readonly ServiceBusClient _client;
        private readonly string _topicName;

        public ServiceBusSender(IConfiguration config)
        {
            _client = new ServiceBusClient(config["ServiceBus:ConnectionString"]);
            _topicName = config["ServiceBus:TopicName"];
        }

        public async Task SendMessageAsync(string messageBody, string subject)
        {
            var sender = _client.CreateSender(_topicName);
            var message = new ServiceBusMessage(messageBody)
            {
                Subject = subject,
                ContentType = "application/json"
            };

            await sender.SendMessageAsync(message);
        }
    }
}