using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
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
    public class UpdateCategoryTests
    {
        private readonly IMapper _mapper;

        public UpdateCategoryTests()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async void UpdateCategory_Should_Fail_On_Error_In_Data()
        {
            //Arrange
            var testObject = new Category { Id = 1, Name = "", Description = "" };
            var repository = TestsHelpers.GetRepository<Category, int>(testObject);
            var command = new UpdateCategoryCommand { Name = testObject.Name, Description = testObject.Description };
            //Act
            var handler = new UpdateCategoryCommandHandler(repository, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Equal(2, result.ValidationErrors.Count);
            Assert.Equal("Name is required.", result.ValidationErrors[0]);
        }

        [Fact]
        public async void UpdateCategory_Should_Work()
        {
            //Arrange

            var testObject = new Category { Id = 1, Name = "New Todo List Category", Description = "New This list a is demo" };
            var repository = TestsHelpers.GetRepository<Category, int>(testObject);
            var command = new UpdateCategoryCommand { CategoryId = testObject.Id, Name = testObject.Name, Description = testObject.Description };
            //Act 

            var handler = new UpdateCategoryCommandHandler(repository, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Category);
            Assert.Equal(testObject.Name, result.Category.Name);
        }
    }
}
