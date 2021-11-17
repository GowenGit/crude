using Crude.Core.Formatters;
using System.Globalization;

namespace Crude
{
    public class CrudeOptions
    {
        internal BaseRenderFormatter Formatter { get; }

        public CrudeOptions(BaseRenderFormatter formatter)
        {
            Formatter = formatter;
        }

        public CrudeOptions(CultureInfo culture)
        {
            Formatter = new DefaultRenderFormatter(culture);
        }
    }
}