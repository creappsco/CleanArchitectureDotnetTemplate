using Clean.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Clean.Core.Application.Contracts.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<ToDo> ToDoItems { get; set; }

        DbSet<Category> ToDoLists { get; set; }
    }
}
