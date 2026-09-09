namespace Library.Domain.Interfaces
{
    public interface IEntity<T>
    {
        T Id { get; }
    }
}
