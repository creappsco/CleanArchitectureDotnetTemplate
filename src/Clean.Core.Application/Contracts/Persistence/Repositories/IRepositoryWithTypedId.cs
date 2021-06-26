namespace Clean.Core.Application.Contracts.Persistence.Base
{
    using Clean.Core.Domain.Base;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Primitives;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface with the Basic Contract for Persistence
    /// </summary>
    /// <typeparam name="T">Model class where the CRUD operations will be executed</typeparam>
    /// <typeparam name="TId">Class Identificator type</typeparam>
    public interface IRepositoryWithTypedId<T, TId> where T : class, IEntityWithTypedId<TId>
    {
        /// <summary>
        /// Allows the Query actions
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();

        /// <summary>
        /// Allow Get a Entity By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(TId id);

        /// <summary>
        /// Add a new Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity, CancellationToken token = default);

        /// <summary>
        /// Add a new Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IList<T>> AddRangeAsync(IList<T> entities, CancellationToken token = default);

        /// <summary>
        /// Update a existing Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity, CancellationToken token = default);

        /// <summary>
        /// Delete a existing
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity, CancellationToken token = default);

        /// <summary>
        /// Save model changes asynchronously
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken token = default);

        /// <summary>
        /// Begins the transaction
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();
    }
}
