using Clean.Core.Domain.Entities;
using Clean.Infraestructure.Persistence;
using Clean.Infraestructure.Persistence.Repositories;
using Clean.Tests.UnitTests.Utilities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Clean.Tests.UnitTests.Persistence
{
    public class RepositoryTests
    {
        Mock<ApplicationDbContext> context;

        public RepositoryTests()
        {
            context = new Mock<ApplicationDbContext>();
        }

        [Fact]
        public async Task Add_ShouldBy_Call()
        {
            // Arrange
            var testObject = new ToDo();
            var entities = new List<ToDo> { testObject };

            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo, int>(testObject);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);

            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            await repository.AddAsync(testObject, CancellationToken.None);

            //Assert
            dbSetMock.Verify(x => x.AddAsync(It.Is<ToDo>(y => y == testObject), CancellationToken.None));
        } 

        [Fact]
        public async Task AddRange_ShouldBy_Call()
        {
            // Arrange
            var entities = new List<ToDo> {
                new ToDo { Id = 1, Name = "Test 1",Description="Description 1" },
                new ToDo { Id = 2, Name = "Test 2",Description="Description 2" },
                new ToDo { Id = 3, Name = "Test 3",Description="Description 3" },
            };

            var newEntities = new List<ToDo> {
                new ToDo { Id = 4, Name = "Test 4",Description="Description 4" },
                new ToDo { Id = 5, Name = "Test 5",Description="Description 5" },
                new ToDo { Id = 6, Name = "Test 6",Description="Description 6" },
            };

            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo, int>(entities);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            await repository.AddRangeAsync(newEntities, token);

            //Assert
            dbSetMock.Verify(x => x.AddRangeAsync(It.IsAny<IEnumerable<ToDo>>(), token));
        }

        [Fact]
        public async Task Delete_ShouldBy_Call()
        {
            // Arrange
            var todo1 = new ToDo { Id = 1, Name = "Test 1", Description = "Description 1" };
            var entities = new List<ToDo> {
                todo1,
                new ToDo { Id = 2, Name = "Test 2",Description="Description 2" },
                new ToDo { Id = 3, Name = "Test 3",Description="Description 3" },
            };

            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo, int>(entities);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);

            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            await repository.DeleteAsync(todo1, CancellationToken.None);

            //Assert
            dbSetMock.Verify(x => x.Remove(It.Is<ToDo>(y => y == todo1)));
        }

        [Fact]
        public async Task Update_ShouldBy_Call()
        {
            var testObject = new ToDo { Id = 3, Name = "Test 3", Description = "Description 3" };

            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo, int>(testObject);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);

            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            testObject.Name = "Test 3 Updated";
            await repository.UpdateAsync(testObject, CancellationToken.None);
            //Assert
            dbSetMock.Verify(x => x.Update(It.Is<ToDo>(y => y == testObject)));
        }

        [Fact]
        public async Task GetById_ShouldBy_Call()
        {
            // Arrange
            var entities = new List<ToDo> {
                new ToDo { Id = 1, Name = "Test 1",Description="Description 1" },
                new ToDo { Id = 2, Name = "Test 2",Description="Description 2" },
                new ToDo { Id = 3, Name = "Test 3",Description="Description 3" },
            };

            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo, int>(entities);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            await repository.GetByIdAsync(1);
            //Assert
            dbSetMock.Verify(x => x.FindAsync(It.IsAny<int>()));
        }
    }
}
