using System;

namespace Crude.Models.Formatters
{
    public abstract class BaseRenderFormatter : IFormatProvider, ICustomFormatter
    {
        public object? GetFormat(Type? formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }

        public abstract string Format(string? format, object? arg, IFormatProvider? formatProvider);
    }
}