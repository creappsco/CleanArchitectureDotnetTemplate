namespace Clean.Core.Domain.Base
{
    public interface ITypeCombo
    {
        public string Name { get; set; }
    }

    public class TypeCombo<Tid> : EntityBase<Tid>, ITypeCombo
    {
        public string Name { get; set; }
    }
}
