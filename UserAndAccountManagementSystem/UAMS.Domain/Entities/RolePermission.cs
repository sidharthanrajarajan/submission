namespace UAMS.Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public int Id { get; private set; }
        public int RoleId { get; private set; }
        public int PermissionId { get; private set; }

        public Role Role { get; private set; } = null!;
        public Permission Permission { get; private set; } = null!;

        private RolePermission() { }

        public RolePermission(int roleId, int permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
