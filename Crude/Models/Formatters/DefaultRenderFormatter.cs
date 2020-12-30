using System;
using System.Globalization;

namespace Crude.Models.Formatters
{
    public class DefaultRenderFormatter : BaseRenderFormatter
    {
        private readonly CultureInfo _culture;

        public DefaultRenderFormatter(CultureInfo culture)
        {
            _culture = culture;
        }

        public override string Format(string? format, object? arg, IFormatProvider? formatProvider)
        {
            if (arg == null)
            {
                return RenderContext.EmptyPlaceholder;
            }

            if (arg is double d)
            {
                return d.ToString(format, _culture);
            }

            if (arg is IFormattable formattable)
            {
                return formattable.ToString(format, formatProvider);
            }

            return arg.ToString() ?? RenderContext.EmptyPlaceholder;
        }
    }
}