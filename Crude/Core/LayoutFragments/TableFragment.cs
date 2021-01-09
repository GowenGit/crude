using Crude.Core.Fragments;
using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq.Expressions;

namespace Crude.Core.LayoutFragments
{
    internal class TableFragment<T> : IFragment
        where T : class
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

            if (_table.IsSearchable)
            {
                builder.OpenElement(seq++, "crude-table-fragment-header");

                BuildSearchBox(ref seq, context, builder);

                builder.CloseElement();
            }

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

            var elements = _table.GetElements();

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

            var elementCount = _table.GetTotalElementCount();

            if (elementCount > _table.TablePageSize)
            {
                builder.OpenElement(seq++, "crude-table-fragment-footer");

                BuildPaginationButtons(ref seq, elementCount, context, builder);

                builder.CloseElement();
            }

            builder.CloseElement();
        };

        private void BuildPaginationButtons(ref int seq, ulong elementCount, RenderContext context, RenderTreeBuilder builder)
        {
            var maxPageIndex = elementCount / _table.TablePageSize - (elementCount % _table.TablePageSize > 0 ? 0UL : 1UL);
            var minPage = (ulong)Math.Max(0, (long)_table.Page - _table.TablePageLookahead);
            var maxPage = Math.Min(maxPageIndex, _table.Page + _table.TablePageLookahead);

            const string tablePaginationButtonCss = "crude-pagination-button";

            if (_table.Page != 0)
            {
                var button = CreateButtonFragment(
                    "«",
                    () =>
                    {
                        _table.Page = 0;
                    },
                    tablePaginationButtonCss,
                    false,
                    context);

                builder.AddContent(seq++, button.Render(context));
            }

            for (var i = minPage; i <= maxPage; i++)
            {
                var pageIndex = i;

                var button = CreateButtonFragment(
                    (i + 1).ToString(),
                    () =>
                    {
                        _table.Page = pageIndex;
                    },
                    tablePaginationButtonCss,
                    _table.Page == i,
                    context);

                builder.AddContent(seq++, button.Render(context));
            }

            if (_table.Page < maxPageIndex)
            {
                var button = CreateButtonFragment(
                    "»",
                    () =>
                    {
                        _table.Page = maxPageIndex;
                    },
                    tablePaginationButtonCss,
                    false,
                    context);

                builder.AddContent(seq++, button.Render(context));
            }
        }

        private void BuildSearchBox(ref int seq, RenderContext context, RenderTreeBuilder builder)
        {
            const string tableSearchButtonCss = "crude-search-button";

            builder.OpenComponent<InputText>(seq++);

            Expression<Func<string?>> expression = () => _table.UnescapedSearchTerm;

            // TODO: A bug with DOM distroying elements when this event is called.
            // void OnEnter(KeyboardEventArgs e)
            // {
            //     // If enter is pressed invoke StateHasChanged event to trigger
            //     // component render so we fetch table results again
            //     // otherwise make sure that we do not re-render
            //     if (e.Code == "Enter" || e.Code == "NumpadEnter")
            //     {
            //         context.StateHasChanged();
            //     }
            // }
            // builder.AddAttribute(seq++, "onkeydown", (Action<KeyboardEventArgs>)OnEnter);
            builder.AddAttribute(seq++, "Placeholder", _table.TableSearchPlaceholder);
            builder.AddAttribute(seq++, "Value", _table.UnescapedSearchTerm);
            builder.AddAttribute(seq++, "ValueChanged", EventCallback.Factory.Create<string>(this, value => _table.UnescapedSearchTerm = value));
            builder.AddAttribute(seq++, "ValueExpression", expression);

            builder.CloseComponent();

            var button = CreateButtonFragment(_table.TableFindButton, () => { _table.Page = 0UL; }, tableSearchButtonCss, false, context);

            builder.AddContent(seq++, button.Render(context));
        }

        private static IFragment CreateButtonFragment(string name, Action action, string cssClass, bool disabled, RenderContext context)
        {
            return new ButtonFragment(name, context.CreateEvent(action), cssClass, disabled);
        }

        private static RenderFragment GetValue(CrudeProperty property, RenderContext context)
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