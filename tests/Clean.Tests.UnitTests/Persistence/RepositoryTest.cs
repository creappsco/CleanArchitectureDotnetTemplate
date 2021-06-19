using Clean.Core.Domain.Entities;
using Clean.Infraestructure.Persistence;
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
    public class RepositoryTest
    {
        Mock<ApplicationDbContext> context;

        public RepositoryTest()
        {
            context = new Mock<ApplicationDbContext>();
        }
        [Fact]
        public void ShouldBy_Call_Add_Method()
        {
            // Arrange
            var testObject = new ToDo();

            
            var dbSetMock = TestsHelpers.GetMockDbSet<ToDo>(testObject);
            context.Setup(x => x.Set<ToDo>()).Returns(dbSetMock.Object);
            // Act
            var repository = new Repository<ToDo, int>(context.Object);
            repository.Add(testObject);

            //Assert
            context.Verify(x => x.Set<TodoItem>());
            dbSetMock.Verify(x => x.Add(It.Is<TodoItem>(y => y == testObject)));
        }

        [Fact]
        public async Task ShouldBy_Call_AddAsync_Method()
        {
            // Arrange
            var testObject = new TodoItem();
            var entities = new List<TodoItem> { testObject };
             
            var dbSetMock = TestsHelpers.GetMockDbSet<TodoItem>(testObject);
            context.Setup(x => x.Set<TodoItem>()).Returns(dbSetMock.Object);

            // Act
            var repository = new Repository<TodoItem, int>(context.Object);
            await repository.AddAsync(testObject);

            //Assert
            dbSetMock.Verify(x => x.AddAsync(It.Is<TodoItem>(y => y == testObject), CancellationToken.None));
        }

        [Fact]
        public void ShouldBy_Get_Query()
        {
            // Arrange
            var entities = new List<TodoItem> {
                new TodoItem { Id = 1, Name = "Test 1" },
                new TodoItem { Id = 2, Name = "Test 2" },
                new TodoItem { Id = 3, Name = "Test 3" },
            };
             
            var dbSetMock = TestsHelpers.GetMockDbSet<TodoItem>(entities);
            context.Setup(x => x.Set<TodoItem>()).Returns(dbSetMock.Object);

            // Act
            var repository = new Repository<TodoItem, int>(context.Object);
            var results = repository.Query();
            var list = results.ToList();
            //Assert
            Assert.NotEmpty(list);
            Assert.True(list.Count == 3);
        }

        [Fact]
        public void ShouldBy_Call_RemoveRange_Method()
        {
            // Arrange
            var entities = new List<TodoItem> {
                           new TodoItem { Id = 1, Name = "Test 1" },
                    new TodoItem { Id = 2, Name = "Test 2" },
                new TodoItem { Id = 3, Name = "Test 3" },
            };
             
            var dbSetMock = TestsHelpers.GetMockDbSet<TodoItem>(entities);
            context.Setup(x => x.Set<TodoItem>()).Returns(dbSetMock.Object);
            // Act
            var repository = new Repository<TodoItem, int>(context.Object);
            repository.RemoveRange(entities.Where(x => x.Id <= 2).ToList());
            var noExist = repository.Query().FirstOrDefault(x => x.Id == 1);
            var exist = repository.Query().FirstOrDefault(x => x.Id == 3);
            //Assert
            dbSetMock.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<TodoItem>>()));
            Assert.Null(noExist);
            Assert.NotNull(exist);
        }
        [Fact]
        public void ShouldBy_Call_Update_Method()
        {
            var testObject = new TodoItem { Id = 3, Name = "Test 3" };
            var entities = new List<TodoItem>
            {
                new TodoItem { Id = 1, Name = "Test 1" },
                new TodoItem { Id = 2, Name = "Test 2" },
                testObject
            };
             
            var dbSetMock = TestsHelpers.GetMockDbSet<TodoItem>(testObject);
            context.Setup(x => x.Set<TodoItem>()).Returns(dbSetMock.Object);
            // Act
            var repository = new Repository<TodoItem, int>(context.Object);
            testObject.Name = "Test 3 Updated";
            repository.Update(testObject);
            var exist = repository.Query().FirstOrDefault(x => x.Id == 3);
            //Assert
            dbSetMock.Verify(x => x.Update(It.Is<TodoItem>(y => y == testObject)));
            Assert.Equal("Test 3 Updated", exist.Name);

        }
    }
}
