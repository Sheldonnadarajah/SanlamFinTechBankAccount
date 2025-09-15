using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SanlamFinTechBankAccount.Core.Models;
using SanlamFinTechBankAccount.Application.Events;

namespace SanlamFinTechBankAccount.Infrastructure.Events
{
    public class SnsEventPublisher : IEventPublisher
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly string _topicArn;

        public SnsEventPublisher(IAmazonSimpleNotificationService snsClient, IConfiguration config)
        {
            _snsClient = snsClient;
            _topicArn = config["Aws:Sns:WithdrawalTopicArn"]!;
        }

        public async Task PublishAsync(WithdrawalEvent evt)
        {
            var message = JsonSerializer.Serialize(evt);
            var request = new PublishRequest
            {
                TopicArn = _topicArn,
                Message = message
            };

            await _snsClient.PublishAsync(request);
        }
    }
}
