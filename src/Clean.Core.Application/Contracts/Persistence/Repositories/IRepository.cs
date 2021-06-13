using Clean.Core.Domain.Base;

namespace Clean.Core.Application.Contracts.Persistence.Base
{
    /// <summary>
    /// Interface with the Basic Contract for Persistence
    /// </summary>
    /// <typeparam name="T">Model class where the CRUD operations will be executed</typeparam>
    /// <typeparam name="TId">Class Identificator type</typeparam>
    public interface IRepository<T, TId> : IRepositoryWithTypedId<T, TId>
        where T : class, IEntityWithTypedId<TId>
    {
    }
}
