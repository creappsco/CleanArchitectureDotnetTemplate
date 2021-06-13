using Clean.Core.Domain.Base; 

namespace Clean.Core.Domain.Entities
{
    public class ToDo : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
