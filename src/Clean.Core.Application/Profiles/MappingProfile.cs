using AutoMapper;
using Clean.Core.Application.Features.ToDos.Commands.CreateCategory;
using Clean.Core.Application.Models.Dto;
using Clean.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Application.Profiles
{
    /// <summary>
    /// Configure AutoMapper 
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ToDoDto, ToDo>().ReverseMap();
        }
    }
}