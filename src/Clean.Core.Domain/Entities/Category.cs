using Clean.Core.Domain.Base;
using System.Collections;
using System.Collections.Generic;

namespace Clean.Core.Domain.Entities
{
    public class Category : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<ToDo> ToDoItems { get; set; }
    }
}
