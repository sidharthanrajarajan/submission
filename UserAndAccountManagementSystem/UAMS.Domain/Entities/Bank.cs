namespace UAMS.Domain.Entities
{
    public class Bank : BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Code { get; private set; } = null!;
        public string HeadOfficeAddress { get; private set; } = null!;
        public string ContactNumber { get; private set; } = null!;

        public ICollection<Branch> Branches { get; private set; } = new List<Branch>();

        private Bank() { }

        public Bank(string name, string code, string headOfficeAddress, string contactNumber)
        {
            Name = name;
            Code = code;
            HeadOfficeAddress = headOfficeAddress;
            ContactNumber = contactNumber;
        }
    }
}
