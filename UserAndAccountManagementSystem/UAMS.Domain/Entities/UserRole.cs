namespace UAMS.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int RoleId { get; private set; }

        public User User { get; private set; } = null!;
        public Role Role { get; private set; } = null!;

        private UserRole() { }

        public UserRole(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
