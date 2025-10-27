namespace UAMS.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public int Id { get; private set; }
        public int FromAccountId { get; private set; }
        public int ToAccountId { get; private set; }
        public decimal Amount { get; private set; }
        public string Currency { get; private set; } = null!;
        public string TransactionType { get; private set; } = null!;
        public string Status { get; private set; } = null!;
        public DateTime TransactionDate { get; private set; }
        public string? Remarks { get; private set; }

        public Account FromAccount { get; private set; } = null!;
        public Account ToAccount { get; private set; } = null!;

        private Transaction() { }

        public Transaction(int fromAccountId, int toAccountId, decimal amount, string currency, string type, string? remarks)
        {
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Currency = currency;
            TransactionType = type;
            Remarks = remarks;
            Status = "Pending";
            TransactionDate = DateTime.UtcNow;
        }

        public void MarkCompleted() => Status = "Completed";
        public void MarkFailed() => Status = "Failed";
    }
}
