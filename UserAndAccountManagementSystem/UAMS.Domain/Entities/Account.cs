namespace UAMS.Domain.Entities
{
    public class Account : BaseEntity
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int BranchId { get; private set; }
        public int? PowerOfAttorneyUserId { get; private set; }
        public string AccountNumber { get; private set; } = null!;
        public string AccountType { get; private set; } = null!;
        public string Currency { get; private set; } = null!;
        public decimal Balance { get; private set; }
        public bool IsMinor { get; private set; }
        public bool IsActive { get; private set; }

        public User User { get; private set; } = null!;
        public Branch Branch { get; private set; } = null!;
        public User? PowerOfAttorneyUser { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        private Account() { }

        public Account(int userId, int branchId, int? powerOfAttorneyUserId, string accountNumber, string accountType, string currency, bool isMinor)
        {
            UserId = userId;
            BranchId = branchId;
            PowerOfAttorneyUserId = powerOfAttorneyUserId;
            AccountNumber = accountNumber;
            AccountType = accountType;
            Currency = currency;
            IsMinor = isMinor;
            IsActive = true;
            Balance = 0;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.");
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be positive.");
            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
        }

        public void CloseAccount() => IsActive = false;
    }
}
