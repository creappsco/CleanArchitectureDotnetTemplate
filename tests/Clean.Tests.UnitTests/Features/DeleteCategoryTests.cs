using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Exceptions;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
using Clean.Core.Application.Features.ToDos.Commands.DeleteCategory;
using Clean.Core.Application.Features.ToDos.Commands.UpdateCategory;
using Clean.Core.Application.Profiles;
using Clean.Core.Domain.Entities;
using Clean.Infraestructure.Persistence;
using Clean.Tests.UnitTests.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Clean.Tests.UnitTests.Features
{
    public class DeleteCategoryTests
    {
        private readonly IMapper _mapper;

        public DeleteCategoryTests()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async void DeleteCategory_Should_Fail_On_Not_Exist_Id()
        {
            //Arrange
            var listCategories = new List<Category> { new Category { Id = 1, Name = "", Description = "" } };
            var testObject = new Category { Id = 2, Name = "", Description = "" };
            var repository = TestsHelpers.GetRepository<Category, int>(listCategories);

            var command = new DeleteCategoryCommand { CategoryId = testObject.Id };
            //Act
            var handler = new DeleteCategoryCommandHandler(repository, _mapper);

            //Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async void DeleteCategory_Should_Fail_On_Category_With_Tasks()
        {
            //Arrange
            var listCategories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "",
                    Description = "",
                    ToDoItems=new List<ToDo>{ new ToDo { Id = 1} }
                } 
            };

            var repository = TestsHelpers.GetRepository<Category, int>(listCategories);

            var command = new DeleteCategoryCommand { CategoryId = 1 };
            //Act
            var handler = new DeleteCategoryCommandHandler(repository, _mapper);

            //Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("This Category have associated tasks", ex.Message);
        }

        [Fact]
        public async void DeleteCategory_Should_Work()
        {
            //Arrange
            var listCategories = new List<Category> { new Category { Id = 1, Name = "", Description = "" } };
            var testObject = new Category { Id = 1, Name = "", Description = "" };
            var repository = TestsHelpers.GetRepository<Category, int>(listCategories);

            var command = new DeleteCategoryCommand { CategoryId = testObject.Id };
            //Act
            var handler = new DeleteCategoryCommandHandler(repository, _mapper);
            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
