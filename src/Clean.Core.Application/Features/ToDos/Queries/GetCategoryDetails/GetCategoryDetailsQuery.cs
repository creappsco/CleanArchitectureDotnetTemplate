using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Models.Dto;
using Clean.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Core.Application.Features.ToDos.Queries.GetCategoryDetails
{
    public class GetCategoryDetailsQuery : IRequest<CategoryDetails>
    {
        public int CategoryId { get; set; }
    }

    public class CategoryDetails
    {
        public CategoryDto Category { get; set; }
        public IList<ToDoDto> ToDos { get; set; }
        public int Count => ToDos?.Count ?? 0;
    }

    public class GetCategoryDetailsQueryHandler : IRequestHandler<GetCategoryDetailsQuery, CategoryDetails>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category, int> _repository;

        public GetCategoryDetailsQueryHandler(IRepository<Category, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoryDetails> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            var category = await this._repository.Query()
                                                .Include(x => x.ToDoItems)
                                        .FirstOrDefaultAsync(x => x.Id == request.CategoryId);

            return new CategoryDetails
            {
                Category = _mapper.Map<CategoryDto>(category),
                ToDos = _mapper.Map<IList<ToDoDto>>(category?.ToDoItems ?? new List<ToDo>())
            };
        }
    }
}
