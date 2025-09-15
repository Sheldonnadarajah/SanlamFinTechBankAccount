using System.Threading.Tasks;
using SanlamFinTechBankAccount.Application.Models;

namespace SanlamFinTechBankAccount.Application.Services
{
    public interface IBankAccountService
    {
        Task<WithdrawalResult> WithdrawAsync(long accountId, decimal amount);
    }
}
