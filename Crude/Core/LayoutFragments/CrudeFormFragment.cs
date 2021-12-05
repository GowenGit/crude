using Crude.Core.Attributes;
using Crude.Core.Fragments;
using Crude.Core.Models;
using Crude.Core.Parsers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Crude.Core.LayoutFragments
{
    internal class CrudeFormFragment : IFragment
    {
        public RenderFragment Render(RenderContext context) => builder =>
        {
            var seq = 0;

            builder.OpenComponent<EditForm>(seq++);
            builder.AddAttribute(seq++, "class", "crude-form");
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

            builder.OpenElement(seq++, "crude-form-header");

            builder.OpenComponent<DataAnnotationsValidator>(seq++);
            builder.CloseComponent();

            builder.OpenComponent<ValidationSummary>(seq++);
            builder.CloseComponent();

            builder.CloseElement();

            foreach (var item in items)
            {
                var fragment = new FieldGroupFragment(item);

                builder.AddContent(seq++, fragment.Render(context));
            }

            builder.OpenElement(seq++, "crude-form-footer");

            if (onSubmit != null)
            {
                builder.OpenElement(seq++, "input");
                builder.AddAttribute(seq++, "type", "submit");
                builder.AddAttribute(seq++, "value", onSubmit.Name);
                builder.CloseElement();
            }

            foreach (var onButtonClick in GetOnClickButtons(context))
            {
                var button = new ButtonFragment(onButtonClick.Name, onButtonClick.Callback, string.Empty, false);

                builder.AddContent(seq++, button.Render(context));
            }

            builder.CloseElement();
        };

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