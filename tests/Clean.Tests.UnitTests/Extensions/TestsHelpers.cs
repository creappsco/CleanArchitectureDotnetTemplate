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

    public static class TestsHelpers
    {

        internal static Mock<DbSet<T>> GetMockDbSet<T>(IList<T> entities) where T : class
        {
            return PrepareDbSet(entities);
        }
        internal static Mock<DbSet<T>> GetMockDbSet<T>(T entity) where T : class
        {
            var entities = new List<T> { entity };
            return PrepareDbSet(entities);
        }
        private static Mock<DbSet<T>> PrepareDbSet<T>(IList<T> entities) where T : class
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

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>((T model) => { entities.Add(model); });


            mockSet.Setup(_ => _.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                           .Callback((T model, CancellationToken token) => { entities.Add(model); });

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
            return mockSet;
        }
    }
}
