using System.Transactions;
using System.Threading.Tasks;
using SanlamFinTechBankAccount.Application.Repositories;
using SanlamFinTechBankAccount.Application.Events;
using SanlamFinTechBankAccount.Core.Models;
using SanlamFinTechBankAccount.Application.Models;

namespace SanlamFinTechBankAccount.Application.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBalanceRepository _repository;
        private readonly IEventPublisher _publisher;

        public BankAccountService(IBalanceRepository repository, IEventPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task<WithdrawalResult> WithdrawAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
                return new WithdrawalResult { Success = false, ErrorCode = "InvalidAmount", Message = "Withdrawal amount must be positive." };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var balance = _repository.GetBalance(accountId);
            if (balance == null || balance < amount)
                return new WithdrawalResult { Success = false, ErrorCode = "InsufficientFunds", Message = "Insufficient funds for withdrawal." };

            var updated = _repository.UpdateBalance(accountId, amount);
            if (updated == 0)
                return new WithdrawalResult { Success = false, ErrorCode = "UpdateFailed", Message = "Withdrawal failed." };

            var evt = new WithdrawalEvent(amount, accountId, "SUCCESSFUL");
            await _publisher.PublishAsync(evt);

            scope.Complete();
            return new WithdrawalResult { Success = true, Message = "Withdrawal successful." };
        }
    }
}
