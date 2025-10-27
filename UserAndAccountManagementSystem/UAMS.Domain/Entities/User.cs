namespace UAMS.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; private set; }
        public string FullName { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string PasswordHash { get; private set; } = null!;
        public string UserType { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;
        public bool IsActive { get; private set; }

        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();
        public ICollection<Account> Accounts { get; private set; } = new List<Account>();
        public ICollection<Account> PowerOfAttorneyAccounts { get; private set; } = new List<Account>();

        private User() { } // EF Core

        public User(string fullName, string email, string passwordHash, string userType, string phoneNumber)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            UserType = userType;
            PhoneNumber = phoneNumber;
            IsActive = true;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
