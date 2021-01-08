using System;
using Crude.Core.Fragments;
using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;

namespace Crude.Core.LayoutFragments
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

            var items = ViewModelParser.ParseProperties(headerInstance);

            foreach (var item in items)
            {
                builder.OpenElement(seq++, "th");

                CrudeEvent? sortEvent = null;

                if (_table.IsSortable)
                {
                    sortEvent = new CrudeEvent(() =>
                    {
                        if (_table.SortColumn == item.Info.Name)
                        {
                            _table.SortDescending = !_table.SortDescending;
                        }
                        else
                        {
                            _table.SortColumn = item.Info.Name;
                            _table.SortDescending = false;
                        }
                    });
                }

                var header = new ActionDecoratorFragment(item.Name, sortEvent);

                builder.AddContent(seq++, header.Render(context));

                if (item.Info.Name == _table.SortColumn)
                {
                    builder.OpenElement(seq++, "crude-table-sort-icon");
                    builder.AddAttribute(seq++, "class", _table.SortDescending ? "sort-desc" : "sort-asc");
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(seq++, "tbody");

            var elements = _table.GetElements(_table.Page, context.TablePageSize);

            foreach (var element in elements)
            {
                builder.OpenElement(seq++, "tr");

                var rowItems = ViewModelParser.ParseProperties(element);

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