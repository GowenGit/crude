using Crude.Core.Attributes;
using Crude.Core.Fragments;
using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Crude.Core.LayoutFragments
{
    internal class CrudeTreeFragment : IFragment
    {
        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<EditForm>(seq++);
            builder.AddAttribute(seq++, "EditContext", context.EditContext);

            var onSubmit = GetOnSubmitButton(context);

            if (onSubmit != null)
            {
                builder.AddAttribute(seq++, "OnValidSubmit", onSubmit.Callback);
            }

            builder.AddAttribute(seq++, "ChildContent", RenderFormContents(context, onSubmit));
            builder.CloseComponent();
        };

        private static RenderFragment<EditContext> RenderFormContents(RenderContext context, CrudeButton<EditContext>? onSubmit) => ctx => builder =>
        {
            var items = ViewModelParser.ParseProperties(context.ViewModel);

            var seq = 0;

            builder.OpenElement(seq++, "crude-tree");
            builder.OpenElement(seq++, "crude-tree-header");

            builder.OpenComponent<DataAnnotationsValidator>(seq++);
            builder.CloseComponent();

            builder.OpenComponent<ValidationSummary>(seq++);
            builder.CloseComponent();

            builder.CloseElement();

            foreach (var item in items)
            {
                if (item.Type == CrudePropertyType.Field)
                {
                    var fragment = CreateFieldFragment(item);

                    builder.AddContent(seq++, fragment.Render(context));
                }

                if (item.Type == CrudePropertyType.Table)
                {
                    var fragment = CreateTableFragment(item);

                    if (fragment == null)
                    {
                        continue;
                    }

                    builder.AddContent(seq++, fragment.Render(context));
                }
            }

            builder.OpenElement(seq++, "crude-tree-footer");

            if (onSubmit != null)
            {
                builder.OpenElement(seq++, "button");
                builder.AddAttribute(seq++, "type", "submit");
                builder.AddContent(seq++, onSubmit.Name);
                builder.CloseElement();
            }

            foreach (var onButtonClick in GetOnClickButtons(context))
            {
                var button = new ButtonFragment(onButtonClick.Name, onButtonClick.Callback, string.Empty, false);

                builder.AddContent(seq++, button.Render(context));
            }

            builder.CloseElement();
            builder.CloseElement();
        };

        private static FieldGroupFragment CreateFieldFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            return new FieldGroupFragment(property);
        }

        private static IFragment? CreateTableFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Table)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            var viewModelType = property.Info.PropertyType.BaseType?.GetGenericArguments()[0];

            if (viewModelType == null)
            {
                throw new ArgumentException($"Could not resolve view model type for property {property.Name}");
            }

            var type = typeof(TableFragment<>).MakeGenericType(viewModelType);

            var fragment = Activator.CreateInstance(type, property.GetValue());

            return (IFragment?)fragment;
        }

        private static IEnumerable<CrudeButton> GetOnClickButtons(RenderContext context)
        {
            var methods = ViewModelParser.ParseMethods(context.ViewModel);

            var result = new List<CrudeButton>();

            foreach (var method in methods)
            {
                if (method.Attributes.FirstOrDefault(x => x is CrudeOnButtonClickAttribute) is CrudeOnButtonClickAttribute crudeOnSubmitAttribute)
                {
                    var button = new CrudeButton(
                         context.CreateEvent(() => method.MethodInfo.Invoke(context.ViewModel, new object[] { context.EditContext })),
                         crudeOnSubmitAttribute.Name);

                    result.Add(button);
                }
            }

            return result;
        }

        private static CrudeButton<EditContext>? GetOnSubmitButton(RenderContext context)
        {
            var methods = ViewModelParser.ParseMethods(context.ViewModel);

            CrudeButton<EditContext>? button = null;

            foreach (var method in methods)
            {
                if (method.Attributes.FirstOrDefault(x => x is CrudeOnSubmitAttribute) is CrudeOnSubmitAttribute crudeOnSubmitAttribute)
                {
                    button = new CrudeButton<EditContext>(
                        context.CreateEvent<EditContext>(ctx => method.MethodInfo.Invoke(context.ViewModel, new object[] { ctx })),
                        crudeOnSubmitAttribute.Name);
                }
            }

            return button;
        }
    }
}