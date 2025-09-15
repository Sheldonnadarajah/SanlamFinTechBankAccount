using System.Data;

namespace SanlamFinTechBankAccount.Application.Repositories
{
    public interface IBalanceRepository
    {
        decimal? GetBalance(long accountId);
        int UpdateBalance(long accountId, decimal amount);
    }
}
