using Crude.Models;
using Crude.Models.Fragments;
using Microsoft.AspNetCore.Components;
using System;
using Crude.Models.LayoutFragments;

namespace Crude
{
    internal class CrudeTreeRenderer
    {
        private readonly object _viewModel;

        public CrudeTreeRenderer(object viewModel)
        {
            _viewModel = viewModel;
        }

        public RenderFragment Render(RenderOptions options) => builder =>
        {
            var items = CrudePropertyFactory.Create(_viewModel);

            var seq = 0;

            foreach (var item in items)
            {
                if (item.Type == CrudePropertyType.Field)
                {
                    var fragment = CreateFieldFragment(item);

                    if (fragment == null)
                    {
                        continue;
                    }

                    builder.AddContent(seq++, fragment.Render(options));
                }

                if (item.Type == CrudePropertyType.Table)
                {
                    var fragment = CreateTableFragment(item);

                    if (fragment == null)
                    {
                        continue;
                    }

                    builder.AddContent(seq++, fragment.Render(options));
                }
            }
        };

        private static FieldFragment? CreateFieldFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Field)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            var labelFragment = new LabelFragment(property.Name);
            var valueFragment = CrudeFragmentFactory.Create(property);

            return new FieldFragment(labelFragment, valueFragment);
        }


        private static ICrudeFragment? CreateTableFragment(CrudeProperty property)
        {
            if (property.Type != CrudePropertyType.Table)
            {
                throw new ArgumentException($"This method can not be called for {property.Type} fragments");
            }

            var viewModelType = property.Value.GetType().BaseType?.GetGenericArguments()[0];

            if (viewModelType == null)
            {
                throw new ArgumentException($"Could not resolve view model type for property {property.Name}");
            }

            var type = typeof(TableFragment<>).MakeGenericType(viewModelType);

            var fragment = Activator.CreateInstance(type, property.Value);

            return (ICrudeFragment?) fragment;
        }
    }
}