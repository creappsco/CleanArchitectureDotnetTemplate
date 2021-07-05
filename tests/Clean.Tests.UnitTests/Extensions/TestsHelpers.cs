namespace Clean.Tests.UnitTests.Utilities
{
    using Moq;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System;
    using Clean.Tests.UnitTests.Extensions.MocksUtils;
    using Clean.Core.Domain.Base;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Clean.Core.Application.Contracts.Persistence.Base;
    using Clean.Infraestructure.Persistence;
    using Clean.Infraestructure.Persistence.Repositories;

    public static class TestsHelpers
    {

        internal static Mock<DbSet<T>> GetMockDbSet<T, TId>(IList<T> entities) where T : class, IEntityWithTypedId<TId>
        {
            return PrepareDbSet<T, TId>(entities);
        }
        internal static Mock<DbSet<T>> GetMockDbSet<T, TId>(T entity) where T : class, IEntityWithTypedId<TId>
        {
            var entities = new List<T> { entity };
            return PrepareDbSet<T, TId>(entities);
        }

        internal static IRepository<T, TId> GetRepository<T, TId>(IList<T> entities) where T : class, IEntityWithTypedId<TId>
        {
            Mock<ApplicationDbContext> context = new Mock<ApplicationDbContext>();

            var dbSetMock = TestsHelpers.GetMockDbSet<T, TId>(entities);
            context.Setup(x => x.Set<T>()).Returns(dbSetMock.Object);

            return new Repository<T, TId>(context.Object);
        }

        internal static IRepository<T, TId> GetRepository<T, TId>(T entity) where T : class, IEntityWithTypedId<TId>
        {
            Mock<ApplicationDbContext> context = new Mock<ApplicationDbContext>();

            var dbSetMock = TestsHelpers.GetMockDbSet<T, TId>(entity);
            context.Setup(x => x.Set<T>()).Returns(dbSetMock.Object);

            return new Repository<T, TId>(context.Object);
        }

        private static Mock<DbSet<T>> PrepareDbSet<T, TId>(IList<T> entities) where T : class, IEntityWithTypedId<TId>
        {
            var mockSet = new Mock<DbSet<T>>();
            var enumerable = new MockAsyncEnumerable<T>(entities);

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(enumerable);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());

            mockSet.As<IAsyncEnumerable<T>>()
                            .Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                            .Returns(new MockAsyncEnumerator<T>(entities.GetEnumerator()));


            mockSet.Setup(_ => _.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                           .Callback((T model, CancellationToken token) => { entities.Add(model); });


            mockSet.Setup(m => m.AddRangeAsync(It.IsAny<IEnumerable<T>>(), It.IsAny<CancellationToken>()))
                                   .Callback((IEnumerable<T> model, CancellationToken token) =>
                                   {
                                       foreach (var item in model)
                                       {
                                           entities.Add(item);
                                       }
                                   });


            mockSet.Setup(m => m.Remove(It.IsAny<T>()))
                          .Callback((T model) => { entities.Remove(model); });

            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
                         .Callback((IEnumerable<T> model) =>
                         {
                             foreach (var item in model)
                             {
                                 entities.Remove(item);
                             }
                         });

            mockSet.Setup(m => m.Update(It.IsAny<T>()))
                          .Callback((T model) =>
                          {
                              var index = entities.IndexOf(model);
                              entities.Insert(index, model);
                          });

            mockSet.Setup(m => m.FindAsync(It.IsAny<int>()))
                          .ReturnsAsync((object[] id) => { return entities.FirstOrDefault(x => x.Id.Equals(id[0])); });

            return mockSet;
        }
    }
}
