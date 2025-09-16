using System.Transactions;
using System.Threading.Tasks;
using SanlamFinTechBankAccount.Application.Repositories;
using SanlamFinTechBankAccount.Application.Events;
using SanlamFinTechBankAccount.Core.Models;
using SanlamFinTechBankAccount.Application.Models;
using Microsoft.Extensions.Logging;

namespace SanlamFinTechBankAccount.Application.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBalanceRepository _repository;
        private readonly IEventPublisher _publisher;
        private readonly ILogger<BankAccountService> _logger;

        public BankAccountService(IBalanceRepository repository, IEventPublisher publisher, ILogger<BankAccountService> logger)
        {
            _repository = repository;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<WithdrawalResult> WithdrawAsync(long accountId, decimal amount)
        {
            if (amount <= 0)
            {
                _logger.LogWarning("Withdrawal failed: Invalid amount {Amount} for AccountId {AccountId}", amount, accountId);
                return new WithdrawalResult { Success = false, ErrorCode = "InvalidAmount", Message = "Withdrawal amount must be positive." };
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var balance = _repository.GetBalance(accountId);
            if (balance == null || balance < amount)
            {
                _logger.LogWarning("Withdrawal failed: Insufficient funds for AccountId {AccountId}. Requested: {Amount}, Available: {Balance}", accountId, amount, balance);
                return new WithdrawalResult { Success = false, ErrorCode = "InsufficientFunds", Message = "Insufficient funds for withdrawal." };
            }

            var updated = _repository.UpdateBalance(accountId, amount);
            if (updated == 0)
            {
                _logger.LogError("Withdrawal failed: Database update failed for AccountId {AccountId} with Amount {Amount}", accountId, amount);
                return new WithdrawalResult { Success = false, ErrorCode = "UpdateFailed", Message = "Withdrawal failed." };
            }

            var evt = new WithdrawalEvent(amount, accountId, "SUCCESSFUL");
            await _publisher.PublishAsync(evt);

            scope.Complete();
            _logger.LogInformation("Withdrawal successful for AccountId {AccountId} with Amount {Amount}", accountId, amount);
            return new WithdrawalResult { Success = true, Message = "Withdrawal successful." };
        }
    }
}
