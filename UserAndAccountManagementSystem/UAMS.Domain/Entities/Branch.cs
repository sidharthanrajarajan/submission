namespace UAMS.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public int Id { get; private set; }
        public int BankId { get; private set; }
        public string Name { get; private set; } = null!;
        public string BranchCode { get; private set; } = null!;
        public string Address { get; private set; } = null!;
        public string IFSCCode { get; private set; } = null!;
        public string ContactNumber { get; private set; } = null!;

        public Bank Bank { get; private set; } = null!;
        public ICollection<Account> Accounts { get; private set; } = new List<Account>();

        private Branch() { }

        public Branch(int bankId, string name, string branchCode, string address, string ifscCode, string contactNumber)
        {
            BankId = bankId;
            Name = name;
            BranchCode = branchCode;
            Address = address;
            IFSCCode = ifscCode;
            ContactNumber = contactNumber;
        }
    }
}
