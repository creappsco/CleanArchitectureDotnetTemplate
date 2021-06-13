using Clean.Core.Application.Contracts.Persistence;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Domain.Base;

namespace Clean.Infraestructure.Persistence.Repositories
{
    public class Repository<T, TId> : RepositoryWithTypedId<T, TId>, IRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
    {
        public Repository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
