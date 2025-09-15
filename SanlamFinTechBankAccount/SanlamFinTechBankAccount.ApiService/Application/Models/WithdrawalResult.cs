namespace SanlamFinTechBankAccount.Application.Models
{
    public class WithdrawalResult
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
