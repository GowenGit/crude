using Crude.Core.Formatters;
using System.Globalization;

namespace Crude
{
    public class CrudeOptions
    {
        internal BaseRenderFormatter Formatter { get; }

        internal string Action { get; set; } = string.Empty;

        internal string Method { get; set; } = string.Empty;

        public CrudeOptions(BaseRenderFormatter formatter)
        {
            Formatter = formatter;
        }

        public CrudeOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }

        public CrudeOptions(
            CultureInfo culture,
            string action,
            string method)
        {
            Formatter = new DefaultRenderFormatter(culture);
            Action = action;
            Method = method;
        }
    }
}