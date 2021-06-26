using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Clean.Infraestructure.Persistence.Repositories
{
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class, IEntityWithTypedId<TId>
    {
        /// <summary>
        /// Database context
        /// </summary>
        protected DbContext _dbContext;

        /// <summary>
        /// T Data in database
        /// </summary>
        protected DbSet<T> DbSet;

        /// <summary>
        /// Builds the object with the specified context
        /// </summary>
        /// <param name="context">Database context</param>
        public RepositoryWithTypedId(DbContext context)
        {
            _dbContext = context;
            DbSet = _dbContext.Set<T>();
        }

        /// <summary>
        /// Add a new Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IList<T>> AddRangeAsync(IList<T> entities, CancellationToken token = default)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities, token);
            await _dbContext.SaveChangesAsync(token);

            return entities;
        }

        /// <summary>
        /// Add a new Entity object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity, CancellationToken token = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);

            return entity;
        }

        /// <summary>
        /// Delete a existing
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task DeleteAsync(T entity, CancellationToken token = default)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync(token);
        }

        /// <summary>
        /// Allow Get a Entity By Id
        /// </summary>
        /// <param name="id">Entity Id (Unique Identificator)</param>
        /// <returns></returns>
        public async Task<T> GetByIdAsync(TId id) =>
            await _dbContext.Set<T>().FindAsync(id);

        /// <summary>
        /// Allows the Query actions
        /// </summary>
        /// <returns>Queryable object</returns>
        public IQueryable<T> Query() =>
          DbSet;

        /// <summary>
        /// Save model changes asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken token = default) =>
            await _dbContext.SaveChangesAsync(token);

        /// <summary>
        /// Update a existing Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity, CancellationToken token = default)
        {
            //_dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync(token);
        }

        /// <summary>
        /// Begins the Db transaction
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction() =>
            _dbContext.Database.BeginTransaction();
         
    }
}
