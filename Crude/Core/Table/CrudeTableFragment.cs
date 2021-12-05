using Crude.Core.Fragments;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crude.Core.Table
{
    internal class CrudeTableFragment<T> : IFragment
        where T : class
    {
        private readonly CrudeTableModel<T> _table;

        private IEnumerable<T> _elements = Array.Empty<T>();

        private ulong _elementCount;

        public CrudeTableFragment(CrudeTableModel<T> table)
        {
            _table = table;
        }

        public async Task LoadData()
        {
            _elements = await _table.GetElementsAsync();
            _elementCount = await _table.GetTotalElementCountAsync();
        }

        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenElement(seq++, "crude-table");

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

                Action? sortEvent = null;

                if (_table.IsSortable)
                {
                    sortEvent = () =>
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
                    };
                }

                var header = new ActionDecoratorFragment(item.Name, CreateCallback(context, sortEvent));

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

            foreach (var element in _elements)
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

            if (_elementCount > _table.TablePageSize)
            {
                builder.OpenElement(seq++, "crude-table-fragment-footer");

                BuildPaginationButtons(ref seq, _elementCount, context, builder);

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

            builder.OpenElement(seq++, "input");

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
            builder.AddAttribute(seq++, "value", BindConverter.FormatValue(_table.UnescapedSearchTerm));
            builder.AddAttribute(seq++, "onchange", EventCallback.Factory.CreateBinder<string?>(this, value => _table.UnescapedSearchTerm = value, _table.UnescapedSearchTerm));

            builder.CloseElement();

            var button = CreateButtonFragment(_table.TableFindButton, () => { _table.Page = 0UL; }, tableSearchButtonCss, false, context);

            builder.AddContent(seq++, button.Render(context));
        }

        private IFragment CreateButtonFragment(string name, Action action, string cssClass, bool disabled, RenderContext context)
        {
            return new ButtonFragment(name, CreateCallback(context, action)!.Value, cssClass, disabled);
        }

        private RenderFragment GetValue(CrudeProperty property, RenderContext context)
        {
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

            var action = property.OnClick?.Callback;

            var fragment = new ActionDecoratorFragment(value, CreateCallback(context, action));

            return fragment.Render(context);
        }

        private EventCallback? CreateCallback(RenderContext context, Action? action)
        {
            if (action == null)
            {
                return null;
            }

            return context.CreateEvent(async () =>
            {
                action.Invoke();

                await LoadData();
            });
        }
    }
}