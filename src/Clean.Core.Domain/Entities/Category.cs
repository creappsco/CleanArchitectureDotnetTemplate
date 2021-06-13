using Clean.Core.Domain.Base;

namespace Clean.Core.Domain.Entities
{
    public class Category : EntityBase<int>
    {
        public string Name { get; set; }
    }
}
