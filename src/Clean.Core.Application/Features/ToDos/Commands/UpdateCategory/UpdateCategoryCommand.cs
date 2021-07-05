using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
using Clean.Core.Application.Models.Dto;
using Clean.Core.Application.Responses;
using Clean.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Core.Application.Features.ToDos.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<UpdateCategoryResponse>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryResponse : BaseResponse
    {
        public CategoryDto Category { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, UpdateCategoryResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category, int> _repository;

        public UpdateCategoryCommandHandler(IRepository<Category, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UpdateCategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateCategoryResponse();

            var validator = new UpdateCategoryCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                response.Success = false;
                response.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    response.ValidationErrors.Add(error.ErrorMessage);
                }
            }

            if (response.Success)
            {
                var oldCategory = await this._repository.GetByIdAsync(request.CategoryId);
                oldCategory.Name = request.Name;
                oldCategory.Description = request.Description;
                await _repository.UpdateAsync(oldCategory);
                response.Category = _mapper.Map<CategoryDto>(oldCategory);
            }

            return response;

        }
    }
}
