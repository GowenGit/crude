using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Forms;

namespace Crude.Models.FieldFragments
{
    internal class StringFragment : FieldFragment
    {
        private readonly string _value;

        private string Value { get; set; } = string.Empty;

        internal StringFragment(string value)
        {
            _value = value;
        }

        public override RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<InputText>(seq++);

            Expression<Func<string>> expression = () => Value;

            //builder.AddAttribute(seq++, "Placeholder", _value.ToString(context.Formatter));
            builder.AddAttribute(seq++, "Value", _value);
            builder.AddAttribute(seq++, "ValueChanged", EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, value => Value = value, Value)));
            builder.AddAttribute(seq++, "ValueExpression", expression);
            //builder.AddAttribute(seq++, "EditContext", new EditContext(this));

            builder.CloseComponent();
        };
    }
}