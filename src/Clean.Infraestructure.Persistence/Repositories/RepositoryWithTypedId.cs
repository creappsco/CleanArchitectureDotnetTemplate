using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<T>> AddRangeAsync(IList<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
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

        public async Task<int> SaveChangesAsync() =>
            await _dbContext.SaveChangesAsync();


        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Begins the Db transaction
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction() =>
            _dbContext.Database.BeginTransaction();
    }
}
