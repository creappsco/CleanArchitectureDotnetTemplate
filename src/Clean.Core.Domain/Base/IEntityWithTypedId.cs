namespace Clean.Core.Domain.Base
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
