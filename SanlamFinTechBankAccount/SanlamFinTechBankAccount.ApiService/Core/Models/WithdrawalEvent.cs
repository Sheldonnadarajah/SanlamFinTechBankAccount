namespace SanlamFinTechBankAccount.Core.Models
{
    public class WithdrawalEvent
    {
        public decimal Amount { get; }
        public long AccountId { get; }
        public string Status { get; }

        public WithdrawalEvent(decimal amount, long accountId, string status)
        {
            Amount = amount;
            AccountId = accountId;
            Status = status;
        }
    }
}
