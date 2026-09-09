namespace Library.Domain.Entities.Basic.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; }
    }
}
