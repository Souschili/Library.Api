using Library.Domain.Interfaces;

namespace Library.Domain.Entities.Basic
{
    public class SoftDeletableEntity : SoftDeletableEntity<int>
    {
        protected SoftDeletableEntity()
        {
        }

        protected SoftDeletableEntity(int id) : base(id)
        {
        }
    }

    public class SoftDeletableEntity<T> : BaseEntity<T>, ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        protected SoftDeletableEntity()
        {
        }

        protected SoftDeletableEntity(T id) : base(id)
        {
        }
    }
}
