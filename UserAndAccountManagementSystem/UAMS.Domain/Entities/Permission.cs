namespace UAMS.Domain.Entities
{
    public class Permission : BaseEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; private set; } = new List<RolePermission>();

        private Permission() { }

        public Permission(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
