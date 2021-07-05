using AutoMapper;
using Clean.Core.Application.Features.ToDos.Queries.GetCategories;
using Clean.Core.Application.Features.ToDos.Queries.GetCategoryDetails;
using Clean.Core.Application.Profiles;
using Clean.Core.Domain.Entities;
using Clean.Tests.UnitTests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Clean.Tests.UnitTests.Features
{
    public class GetCategoriesTests
    {
        private readonly IMapper _mapper;

        public GetCategoriesTests()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async void GetCategories_Should_Return_Data()
        {
            //Arrange
            var testObjects = new List<Category>{
                new Category { Id = 1, Name = "", Description = "" },
                new Category { Id = 2, Name = "", Description = "" },
                new Category { Id = 3, Name = "", Description = "" },
                new Category { Id = 4, Name = "", Description = "" },
                };
            var repository = TestsHelpers.GetRepository<Category, int>(testObjects);
            var command = new GetCategoriesQuery { };
            //Act
            var handler = new GetCategoriesQueryHandler(repository, _mapper);
            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Categories);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async void GetCategoryDetails_Should_Return_Data()
        {
            //Arrange
            var testObjects = new List<Category>{
                new Category { Id = 1, Name = "", Description = "",ToDoItems=new List<ToDo>{ new ToDo { }, new ToDo { }, new ToDo { } } },
                new Category { Id = 2, Name = "", Description = "",ToDoItems=new List<ToDo>{ new ToDo { }, new ToDo { }, new ToDo { }, new ToDo { } } },
                };
            var repository = TestsHelpers.GetRepository<Category, int>(testObjects);
            var command = new GetCategoryDetailsQuery { CategoryId = 1 };
            //Act
            var handler = new GetCategoryDetailsQueryHandler(repository, _mapper);
            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Category);
            Assert.Equal(1, result.Category.Id);
            Assert.NotEmpty(result.ToDos);
            Assert.Equal(3, result.Count);
        }
    }
}
