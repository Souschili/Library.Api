using Library.Domain.Interfaces;

namespace Library.Domain.Entities.Basic
{
    public class BaseEntity : BaseEntity<int>
    {
        protected BaseEntity()
        {
        }

        protected BaseEntity(int id) : base(id)
        {
        }
    }

    public class BaseEntity<T> : IEntity<T>
    {
        public T Id { get; protected set; }

        protected BaseEntity()
        {
        }

        protected BaseEntity(T id)
        {
            Id = id;
        }
    }
}
