using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
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
    public class CreateCategoryTests
    {
        private readonly IMapper _mapper;

        public CreateCategoryTests()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async void CreateCategory_Should_Fail_On_Error_In_Data()
        {
            //Arrange
            var testObject = new Category { Id = 1, Name = "", Description = "" };
            var repository = TestsHelpers.GetRepository<Category, int>(testObject);
            var command = new CreateCategoryCommand { Name = testObject.Name, Description = testObject.Description };
            //Act
            var handler = new CreateCategoryCommandHandler(repository, _mapper);

            var result = await handler.Handle(command, CancellationToken.None);
            //Assert
            Assert.NotEmpty(result.ValidationErrors);
            Assert.Equal(2, result.ValidationErrors.Count);
            Assert.Equal("Name is required.", result.ValidationErrors[0]);
        }
    }
}
