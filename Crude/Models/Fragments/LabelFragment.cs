using Microsoft.AspNetCore.Components;
using System;

namespace Crude.Models.Fragments
{
    internal class LabelFragment : ICrudeFragment
    {
        private readonly string _name;

        public LabelFragment(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _name = name;
        }

        public RenderFragment Render(RenderOptions options) => builder =>
        {
            builder.AddContent(0, _name.ToString(options.Formatter));
        };
    }
}
