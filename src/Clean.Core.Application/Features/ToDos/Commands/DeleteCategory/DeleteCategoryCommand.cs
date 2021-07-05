using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Exceptions;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
using Clean.Core.Application.Models.Dto;
using Clean.Core.Application.Responses;
using Clean.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Core.Application.Features.ToDos.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<DeleteCategoryResponse>
    {
        public int CategoryId { get; set; }
    }

    public class DeleteCategoryResponse : BaseResponse
    {
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResponse>
    {
        private readonly IRepository<Category, int> _repository;

        public DeleteCategoryCommandHandler(IRepository<Category, int> repository, IMapper mapper)
        {
            _repository = repository;
        }

        public async Task<DeleteCategoryResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteCategoryResponse { Success = true };

            var oldCategory = await this._repository.Query()
                                .Include(x => x.ToDoItems)
                                .FirstOrDefaultAsync(x => x.Id == request.CategoryId);

            if (oldCategory is null)
            {
                throw new NotFoundException("Category", request.CategoryId);
            }

            if (oldCategory.ToDoItems is not null && oldCategory.ToDoItems.Count() > 0)
            {
                throw new Exception("This Category have associated tasks");
            }

            await _repository.DeleteAsync(oldCategory, cancellationToken);

            return response;

        }
    }
}
