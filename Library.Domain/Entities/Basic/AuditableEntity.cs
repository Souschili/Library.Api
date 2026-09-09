using Library.Domain.Interfaces;

namespace Library.Domain.Entities.Basic
{
    public class AuditableEntity : AuditableEntity<int>
    {
        protected AuditableEntity()
        {
        }

        protected AuditableEntity(int id) : base(id)
        {
        }
    }

    public class AuditableEntity<T> : BaseEntity<T>, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        protected AuditableEntity()
        {
        }

        protected AuditableEntity(T id) : base(id)
        {
        }
    }
}
