using System.Threading.Tasks;
using SanlamFinTechBankAccount.Core.Models;

namespace SanlamFinTechBankAccount.Application.Events
{
    public interface IEventPublisher
    {
        Task PublishAsync(WithdrawalEvent evt);
    }
}
