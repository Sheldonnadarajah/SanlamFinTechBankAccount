using Dapper;
using System.Data;
using SanlamFinTechBankAccount.Application.Repositories;
using SanlamFinTechBankAccount.Core.Models;

namespace SanlamFinTechBankAccount.Infrastructure.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly IDbConnection _db;

        public BalanceRepository(IDbConnection db)
        {
            _db = db;
        }

        public decimal? GetBalance(long accountId)
        {
            const string sql = "SELECT balance FROM accounts WHERE id = @Id";
            return _db.QuerySingleOrDefault<decimal?>(sql, new { Id = accountId });
        }

        public int UpdateBalance(long accountId, decimal amount)
        {
            const string sql = @"UPDATE accounts 
                                 SET balance = balance - @Amount 
                                 WHERE id = @Id AND balance >= @Amount";
            return _db.Execute(sql, new { Id = accountId, Amount = amount });
        }
    }
}
