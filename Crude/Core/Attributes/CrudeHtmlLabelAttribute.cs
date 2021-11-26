using System;

namespace Crude.Core.Attributes
{
    public sealed class CrudeHtmlLabelAttribute : CrudePropertyAttribute
    {
        public string Html { get; }

        public CrudeHtmlLabelAttribute(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                throw new ArgumentNullException(nameof(html));
            }

            Html = html;
        }
    }
}