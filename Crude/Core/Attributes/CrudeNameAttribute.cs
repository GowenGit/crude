namespace Crude.Core.Attributes
{
    public sealed class CrudeNameAttribute : CrudePropertyAttribute
    {
        public string Name { get; }

        public CrudeNameAttribute(string name)
        {
            Name = name;
        }
    }
}