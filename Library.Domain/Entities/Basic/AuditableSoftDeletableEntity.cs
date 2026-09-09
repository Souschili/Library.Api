using Library.Domain.Interfaces;

namespace Library.Domain.Entities.Basic
{
    public class AuditableSoftDeletableEntity : AuditableSoftDeletableEntity<int>
    {
        protected AuditableSoftDeletableEntity()
        {
        }

        protected AuditableSoftDeletableEntity(int id) : base(id)
        {
        }
    }

    public class AuditableSoftDeletableEntity<T> : BaseEntity<T>, IAuditable, ISoftDeletable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        protected AuditableSoftDeletableEntity()
        {
        }

        protected AuditableSoftDeletableEntity(T id) : base(id)
        {
        }
    }
}
