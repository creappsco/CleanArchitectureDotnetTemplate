using AutoMapper;
using Clean.Core.Application.Contracts.Persistence.Base;
using Clean.Core.Application.Models.Dto;
using Clean.Core.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Core.Application.Features.ToDos.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<CategoriesViewModel>
    {

    }

    public class CategoriesViewModel
    {
        public IList<CategoryDto> Categories { get; set; }
        public int Count => Categories?.Count ?? 0;
    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CategoriesViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Category, int> _repository;

        public GetCategoriesQueryHandler(IRepository<Category, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoriesViewModel> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var data = await this._repository.Query().ToListAsync();
            return new CategoriesViewModel
            {
                Categories = _mapper.Map<IList<CategoryDto>>(data)
            };
        }
    }
}
