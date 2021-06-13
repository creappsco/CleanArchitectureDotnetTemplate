namespace Clean.Core.Domain.Base
{
    public abstract class EntityWithTypedId<TId> : IEntityWithTypedId<TId>
    {
        public TId Id { get; set; }
    }
}
