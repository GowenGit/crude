using Microsoft.AspNetCore.Components;
using System;
using Crude.Models.Fragments;

namespace Crude.Models.LayoutFragments
{
    internal class TableFragment<T> : IFragment where T : class
    {
        private readonly CrudeTable<T> _table;

        public TableFragment(CrudeTable<T> table)
        {
            _table = table;
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "crude-table-fragment");

            builder.OpenElement(seq++, "table");

            builder.OpenElement(seq++, "thead");
            builder.OpenElement(seq++, "tr");

            var headerInstance = Activator.CreateInstance(typeof(T));

            if (headerInstance == null)
            {
                throw new ArgumentException($"Failed to construct table headers for type {typeof(T)}. Make sure you have an empty constructor");
            }

            var items = CrudePropertyFactory.Create(headerInstance);

            foreach (var item in items)
            {
                builder.OpenElement(seq++, "th");
                builder.AddContent(seq++, item.Name.ToString(context.Formatter));
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(seq++, "tbody");

            var elements = _table.GetElements(_table.Page, context.TablePageSize);

            foreach (var element in elements)
            {
                builder.OpenElement(seq++, "tr");

                var rowItems = CrudePropertyFactory.Create(element);

                foreach (var item in rowItems)
                {
                    builder.OpenElement(seq++, "td");
                    builder.AddContent(seq++, GetValue(item, context));
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();

            builder.CloseElement();
            builder.CloseElement();
        };

        public static RenderFragment GetValue(CrudeProperty property, RenderContext context)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            string value;

            switch (property.GetValue())
            {
                case IFormattable formattable:
                    value = formattable.ToString(null, context.Formatter);
                    break;
                case string stringValue:
                    value = stringValue.ToString(context.Formatter);
                    break;
                default:
                    value = RenderContext.EmptyPlaceholder;
                    break;
            }

            var fragment = new ActionDecoratorFragment(value, property.OnClick);

            return fragment.Render(context);
        }
    }
}