namespace UAMS.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
        public string CreatedBy { get; private set; } = null!;
        public DateTime? UpdatedOn { get; private set; }
        public string? UpdatedBy { get; private set; }
        public bool IsDeleted { get; private set; }

        public void MarkCreated(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }

        public void MarkUpdated(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }

        public void SoftDelete(string updatedBy)
        {
            IsDeleted = true;
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}
