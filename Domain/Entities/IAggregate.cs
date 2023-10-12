namespace Domain.Entities
{
    public interface IAggregate<out T>
    {
        T Root { get; }
    }
}
