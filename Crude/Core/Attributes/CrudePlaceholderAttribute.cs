namespace Crude.Core.Attributes
{
    public sealed class CrudePlaceholderAttribute : CrudePropertyAttribute
    {
        public string Text { get; } = string.Empty;

        public CrudePlaceholderAttribute(string text)
        {
            Text = text;
        }

        public CrudePlaceholderAttribute() { }
    }
}